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
using System.IO;
using System.Runtime.InteropServices;

using SDL3;
#endregion

namespace Microsoft.Xna.Framework
{
	/// <summary>
	/// A <see cref="Stream"/> backed by a native SDL3 <c>SDL_IOStream</c> handle.
	/// Supports read and seek operations. Dispose closes the underlying SDL_IOStream.
	/// </summary>
	internal sealed class SDL3IOStream : Stream
	{
		#region Private Fields

		private IntPtr _io;
		private string _filename;

		#endregion

		#region Constructor

		public SDL3IOStream(IntPtr io)
		{
			if (io == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(io));
			}
			_io = io;
		}

		public SDL3IOStream(string filename)
		{
			IntPtr io = SDL.SDL_IOFromFile(filename, "rb");
			if (io == IntPtr.Zero)
			{
				throw new FileNotFoundException(filename);
			}
			_io = io;
			_filename = filename;
		}

		#endregion

		#region Stream Properties

		public override bool CanRead => true;
		public override bool CanSeek => true;
		public override bool CanWrite => false;

		public override long Length => SDL.SDL_GetIOSize(_io);

		public override long Position
		{
			get => SDL.SDL_TellIO(_io);
			set => SDL.SDL_SeekIO(_io, value, SDL.SDL_IOWhence.SDL_IO_SEEK_SET);
		}

		#endregion

		#region Stream Methods

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (count == 0)
			{
				return 0;
			}
			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try
			{
				IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, offset);
				UIntPtr bytesRead = SDL.SDL_ReadIO(_io, ptr, (UIntPtr)(uint)count);
				return (int)(uint)bytesRead;
			}
			finally
			{
				handle.Free();
			}
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			SDL.SDL_IOWhence whence;
			switch (origin)
			{
				case SeekOrigin.Begin:   whence = SDL.SDL_IOWhence.SDL_IO_SEEK_SET; break;
				case SeekOrigin.Current: whence = SDL.SDL_IOWhence.SDL_IO_SEEK_CUR; break;
				case SeekOrigin.End:     whence = SDL.SDL_IOWhence.SDL_IO_SEEK_END; break;
				default: throw new ArgumentOutOfRangeException(nameof(origin));
			}
			return SDL.SDL_SeekIO(_io, offset, whence);
		}

		public override void Flush()
		{
			SDL.SDL_FlushIO(_io);
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		#endregion

		#region Dispose

		protected override void Dispose(bool disposing)
		{
			if (_io != IntPtr.Zero)
			{
				SDL.SDL_CloseIO(_io);
				_io = IntPtr.Zero;
			}
			base.Dispose(disposing);
		}

		#endregion
	}
}
