#region License
/* FNA - XNA4 Reimplementation for Desktop Platforms
 * Copyright 2009-2024 Ethan Lee and the MonoGame Team
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */
#endregion

#region Using Statements
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Audio;
using NVorbis;
#endregion

namespace Microsoft.Xna.Framework.Media
{
	public static class DynamicMediaPlayer
	{
		#region Public Static Properties

		public static bool GameHasControl
		{
			get
			{
				/* This is based on whether or not the player is playing custom
				 * music, rather than yours.
				 * -flibit
				 */
				return true;
			}
		}

		public static bool IsMuted
		{
			get
			{
				return INTERNAL_isMuted;
			}

			set
			{
				INTERNAL_isMuted = value;
				audioStream.Volume = INTERNAL_isMuted ?  0.0f : INTERNAL_volume;
			}
		}

		public static bool IsRepeating
		{
			get;
			set;
		}

		public static bool IsShuffled
		{
			get;
			set;
		}

		public static TimeSpan PlayPosition
		{
			get
			{
				return timer.Elapsed;
			}
		}

		public static MediaQueue Queue 
		{
			get;
			private set;
		}

		public static MediaState State
		{
			get
			{
				return INTERNAL_state;
			}

			private set
			{
				if (INTERNAL_state != value)
				{
					INTERNAL_state = value;
					FrameworkDispatcher.MediaStateChanged = true;
				}
			}
		}

		public static float Volume
		{
			get
			{
				return INTERNAL_volume;
			}
			set
			{
				INTERNAL_volume = MathHelper.Clamp(
					value,
					0.0f,
					1.0f
				);
				audioStream.Volume = IsMuted ? 0.0f : INTERNAL_volume;
			}
		}

		public static bool IsVisualizationEnabled
		{
			get
			{
				return FAudio.XNA_VisualizationEnabled() == 1;
			}
			set
			{
				FAudio.XNA_EnableVisualization((uint) (value ? 1 : 0));
			}
		}

		#endregion

		#region Public Static Variables

		public static event EventHandler<EventArgs> ActiveSongChanged;
		public static event EventHandler<EventArgs> MediaStateChanged;

		#endregion

		#region Private Static Variables

		private static bool INTERNAL_isMuted = false;
		private static MediaState INTERNAL_state = MediaState.Stopped;
		private static float INTERNAL_volume = 1.0f;


		/* Need to hold onto this to keep track of how many songs
		 * have played when in shuffle mode.
		 */
		private static int numSongsInQueuePlayed = 0;

		/* FIXME: Ideally we'd be using the stream offset to track position,
		 * but usually you end up with a bit of stairstepping...
		 *
		 * For now, just use a timer. It's not 100% accurate, but it'll at
		 * least be consistent.
		 * -flibit
		 */
		private static Stopwatch timer = new Stopwatch();

		private static readonly Random random = new Random();

		#endregion

		private const int AUDIO_BUFFER_SIZE = 4096 * 2;
		private static readonly float[] _sampleBuffer = new float[AUDIO_BUFFER_SIZE];
		private static DynamicSoundEffectInstance audioStream;
		private static VorbisReader _vorbisReader;

		#region Static Constructor

		static DynamicMediaPlayer()
		{
			Queue = new MediaQueue();
		}

		#endregion

		#region Public Static Methods

		public static void MoveNext()
		{
			NextSong(1);
		}

		public static void MovePrevious()
		{
			NextSong(-1);
		}

		public static void Pause()
		{
			if (State != MediaState.Playing || Queue.ActiveSong == null)
			{
				return;
			}

			audioStream.Pause();
			timer.Stop();

			State = MediaState.Paused;
		}

		/// <summary>
		/// The Play method clears the current playback queue and queues the specified song
		/// for playback. Playback starts immediately at the beginning of the song.
		/// </summary>
		public static void Play(Song song)
		{
			Song previousSong = Queue.Count > 0 ? Queue[0] : null;

			Queue.Clear();
			numSongsInQueuePlayed = 0;
			LoadSong(song);
			Queue.ActiveSongIndex = 0;

			PlaySong(song);

			if (previousSong != song)
			{
				FrameworkDispatcher.ActiveSongChanged = true;
			}
		}

		public static void Play(SongCollection songs)
		{
			Play(songs, 0);
		}

		public static void Play(SongCollection songs, int index)
		{
			Queue.Clear();
			numSongsInQueuePlayed = 0;

			foreach (Song song in songs)
			{
				LoadSong(song);
			}

			Queue.ActiveSongIndex = index;

			PlaySong(Queue.ActiveSong);
		}

		public static void Resume()
		{
			if (State != MediaState.Paused)
			{
				return;
			}

			audioStream.Resume();
			timer.Start();
			State = MediaState.Playing;
		}

		public static void Stop()
		{
			if (State == MediaState.Stopped)
			{
				return;
			}

			audioStream.Stop();
			timer.Stop();
			timer.Reset();

			for (int i = 0; i < Queue.Count; i += 1)
			{
				Queue[i].PlayCount = 0;
			}

			State = MediaState.Stopped;
		}

		public static void GetVisualizationData(VisualizationData data)
		{
			FAudio.XNA_GetSongVisualizationData(
				data.freq,
				data.samp,
				VisualizationData.Size
			);
		}

		#endregion

		#region Internal Static Methods

		internal static void Update()
		{
			if (	Queue == null ||
				Queue.ActiveSong == null ||
				State != MediaState.Playing)
			{
				// Nothing to do... yet...
				return;
			}

			numSongsInQueuePlayed += 1;

			if (numSongsInQueuePlayed >= Queue.Count)
			{
				numSongsInQueuePlayed = 0;
				if (!IsRepeating)
				{
					Stop();

					FrameworkDispatcher.ActiveSongChanged = true;

					return;
				}
			}

			MoveNext();
		}

		internal static void DisposeIfNecessary()
		{
			// Dispose the DynamicSoundEffectInstance
			if (audioStream != null)
			{
				audioStream.Dispose();
				audioStream = null;
			}
		}

		internal static void OnActiveSongChanged()
		{
			if (ActiveSongChanged != null)
			{
				ActiveSongChanged(null, EventArgs.Empty);
			}
		}

		internal static void OnMediaStateChanged()
		{
			if (MediaStateChanged != null)
			{
				MediaStateChanged(null, EventArgs.Empty);
			}
		}

		#endregion

		#region Private Static Methods

		private static void LoadSong(Song song)
		{
			/* Believe it or not, XNA duplicates the Song object
			 * and then assigns a bunch of stuff to it at Play time.
			 * -flibit
			 */
			Queue.Add(new Song(song.handle, song.Name));
		}

		private static void NextSong(int direction)
		{
			Stop();
			if (IsRepeating && Queue.ActiveSongIndex >= Queue.Count - 1)
			{
				Queue.ActiveSongIndex = 0;

				/* Setting direction to 0 will force the first song
				 * in the queue to be played.
				 * if we're on "shuffle", then it'll pick a random one
				 * anyway, regardless of the "direction".
				 */
				direction = 0;
			}

			if (IsShuffled)
			{
				Queue.ActiveSongIndex = random.Next(Queue.Count);
			}
			else
			{
				Queue.ActiveSongIndex = (int) MathHelper.Clamp(
					Queue.ActiveSongIndex + direction,
					0,
					Queue.Count - 1
				);
			}

			Song nextSong = Queue[Queue.ActiveSongIndex];
			if (nextSong != null)
			{
				PlaySong(nextSong);
			}

			FrameworkDispatcher.ActiveSongChanged = true;
		}

		private static void PlaySong(Song song)
		{
			InitializeAudioStream(song);

			audioStream.Play();
			timer.Start();
			State = MediaState.Playing;
		}

		private static void InitializeAudioStream(Song song)
		{
			if (_vorbisReader != null)
				_vorbisReader.Dispose();

			_vorbisReader = new VorbisReader(song.handle);

			var channels = _vorbisReader.Channels;
			var sampleRate = _vorbisReader.SampleRate;

			song.Duration = _vorbisReader.TotalTime;

			if (audioStream != null)
				audioStream.Dispose();

			audioStream = new DynamicSoundEffectInstance(
				sampleRate, (AudioChannels)channels
			);
			audioStream.BufferNeeded += OnBufferRequest;

			// Fill and queue the buffers.
			for (int i = 0; i < 4; i += 1)
			{
				OnBufferRequest(audioStream, EventArgs.Empty);
				if (audioStream.PendingBufferCount == i)
				{
					break;
				}
			}
		}

		private static void OnBufferRequest(object sender, EventArgs args)
		{
			int samples = _vorbisReader.ReadSamples(_sampleBuffer, 0, AUDIO_BUFFER_SIZE);

			if (samples > 0)
			{
				audioStream.SubmitFloatBufferEXT(
					_sampleBuffer,
					0,
					samples
				);
			}
			else if (_vorbisReader.IsEndOfStream)
			{
				// Okay, we ran out. No need for this!
				audioStream.BufferNeeded -= OnBufferRequest;
			}
		}

		#endregion

	}
}
