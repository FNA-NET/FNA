#region License
/* FNA - XNA4 Reimplementation for Desktop Platforms
 * Copyright 2009-2021 Ethan Lee and the MonoGame Team
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */
#endregion

#region Using Statements
using System;
using System.ComponentModel;

using Microsoft.Xna.Framework.Input;
#endregion

namespace Microsoft.Xna.Framework
{
	public abstract class GameWindow
	{
		#region Public Properties

		[DefaultValue(false)]
		public abstract bool AllowUserResizing
		{ 
			get;
			set;
		}

		public abstract Rectangle ClientBounds
		{
			get;
		}

		public abstract DisplayOrientation CurrentOrientation
		{
			get;
			internal set;
		}

		public abstract IntPtr Handle
		{
			get;
		}

		public abstract string ScreenDeviceName
		{
			get;
		}

		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				if (_title != value)
				{
					SetTitle(value);
					_title = value;
				}
			}
		}

		/// <summary>
		/// Determines whether the border of the window is visible.
		/// </summary>
		/// <exception cref="System.NotImplementedException">
		/// Thrown when trying to use this property on an unsupported platform.
		/// </exception>
		public virtual bool IsBorderlessEXT
		{
			get
			{
				return false;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		#endregion

		/// <summary>
		/// IME service to handle text compositions and inputs. Only works for Desktop Platforms
		/// </summary>
		public ImmService ImmService { get; internal set; }

		#region Private Variables

		private string _title;

		#endregion

		#region Protected Constructors

		protected GameWindow()
		{
		}

		#endregion

		#region Events

		public event EventHandler<EventArgs> ClientSizeChanged;
		public event EventHandler<EventArgs> OrientationChanged;
		public event EventHandler<EventArgs> ScreenDeviceNameChanged;

		/// <summary>
		/// Buffered keyboard KeyDown event.
		/// </summary>
		public event EventHandler<KeyEventArgs> KeyDown;

		/// <summary>
		/// Buffered keyboard KeyPress event.
		/// </summary>
		public event EventHandler<KeyPressEventArgs> KeyPress;

		/// <summary>
		/// Buffered keyboard KeyUp event.
		/// </summary>
		public event EventHandler<KeyEventArgs> KeyUp;

		/// <summary>
		/// Pointer Down event.
		/// </summary>
		public event EventHandler<PointerEventArgs> PointerDown;

		/// <summary>
		/// Pointer Up event.
		/// </summary>
		public event EventHandler<PointerEventArgs> PointerUp;

		/// <summary>
		/// Pointer Move event.
		/// </summary>
		public event EventHandler<PointerEventArgs> PointerMove;

		/// <summary>
		/// Pointer Scroll event.
		/// </summary>
		public event EventHandler<PointerEventArgs> PointerScroll;

		#endregion

		#region Public Methods

		public abstract void BeginScreenDeviceChange(bool willBeFullScreen);

		public abstract void EndScreenDeviceChange(
			string screenDeviceName,
			int clientWidth,
			int clientHeight
		);

		public void EndScreenDeviceChange(string screenDeviceName)
		{
			EndScreenDeviceChange(
				screenDeviceName,
				ClientBounds.Width,
				ClientBounds.Height
			);
		}

		#endregion

		#region Protected Methods

		protected void OnActivated()
		{
		}

		protected void OnClientSizeChanged()
		{
			if (ClientSizeChanged != null)
			{
				ClientSizeChanged(this, EventArgs.Empty);
			}
		}

		protected void OnDeactivated()
		{
		}

		protected void OnOrientationChanged()
		{
			if (OrientationChanged != null)
			{
				OrientationChanged(this, EventArgs.Empty);
			}
		}

		protected void OnPaint()
		{
		}

		protected void OnScreenDeviceNameChanged()
		{
			if (ScreenDeviceNameChanged != null)
			{
				ScreenDeviceNameChanged(this, EventArgs.Empty);
			}
		}

		internal void OnKeyDown(KeyEventArgs e)
		{
			EventHelpers.Raise(this, KeyDown, e);
		}

		internal void OnKeyPress(KeyPressEventArgs e)
		{
			EventHelpers.Raise(this, KeyPress, e);
		}

		internal void OnKeyUp(KeyEventArgs e)
		{
			EventHelpers.Raise(this, KeyUp, e);
		}

		internal void OnPointerDown(PointerEventArgs e)
		{
			EventHelpers.Raise(this, PointerDown, e);
		}

		internal void OnPointerUp(PointerEventArgs e)
		{
			EventHelpers.Raise(this, PointerUp, e);
		}

		internal void OnPointerMove(PointerEventArgs e)
		{
			EventHelpers.Raise(this, PointerMove, e);
		}

		internal void OnPointerScroll(PointerEventArgs e)
		{
			EventHelpers.Raise(this, PointerScroll, e);
		}

		protected internal abstract void SetSupportedOrientations(
			DisplayOrientation orientations
		);

		protected abstract void SetTitle(string title);

		#endregion
	}
}
