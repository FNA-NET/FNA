#region License
/* FNA - XNA4 Reimplementation for Desktop Platforms
 * Copyright 2021-2024 ryancheung
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */
#endregion

#region Using Statements
using System;
using SDL3;
using Microsoft.Xna.Framework.Input.Touch;
#endregion

namespace Microsoft.Xna.Framework.Input
{
	/// <summary>
	/// Traditional Winform-like event based input system.
	/// </summary>
	public static partial class InputEventEXT
	{
		#region Static Events

		/// <summary>
		/// Buffered keyboard KeyDown event.
		/// </summary>
		public static event Action<KeyEventArgs> KeyDown;

		/// <summary>
		/// Buffered keyboard KeyPress event.
		/// </summary>
		public static event Action<KeyPressEventArgs> KeyPress;

		/// <summary>
		/// Buffered keyboard KeyUp event.
		/// </summary>
		public static event Action<KeyEventArgs> KeyUp;

		/// <summary>
		/// Pointer Down event.
		/// </summary>
		public static event Action<PointerEventArgs> PointerDown;

		/// <summary>
		/// Pointer Up event.
		/// </summary>
		public static event Action<PointerEventArgs> PointerUp;

		/// <summary>
		/// Pointer Move event.
		/// </summary>
		public static event Action<PointerEventArgs> PointerMove;

		/// <summary>
		/// Mouse wheel scroll event.
		/// </summary>
		public static event Action<PointerEventArgs> PointerScroll;

		#endregion

		#region Internal Static Methods

		internal static void OnKeyDown(Keys key, ref SDL.SDL_Event evt)
		{
			if (KeyDown == null) return;

			char character = (char)evt.key.key;
			var alt = (evt.key.mod & SDL.SDL_Keymod.SDL_KMOD_ALT) != 0;
			var control = (evt.key.mod & SDL.SDL_Keymod.SDL_KMOD_CTRL) != 0;
			var shift = (evt.key.mod & SDL.SDL_Keymod.SDL_KMOD_SHIFT) != 0;
			var gui = (evt.key.mod & SDL.SDL_Keymod.SDL_KMOD_GUI) != 0;

			KeyDown.Invoke(new KeyEventArgs(key, alt, control, shift, gui));

			if (KeyPress != null)
				if (char.IsControl(character) || KeysHelper.GetKeyChar(key, shift, out character))
					KeyPress.Invoke(new KeyPressEventArgs(key, character));
		}

		internal static void OnKeyUp(Keys key, ref SDL.SDL_Event evt)
		{
			if (KeyUp == null) return;

			var alt = (evt.key.mod & SDL.SDL_Keymod.SDL_KMOD_ALT) != 0;
			var control = (evt.key.mod & SDL.SDL_Keymod.SDL_KMOD_CTRL) != 0;
			var shift = (evt.key.mod & SDL.SDL_Keymod.SDL_KMOD_SHIFT) != 0;
			var gui = (evt.key.mod & SDL.SDL_Keymod.SDL_KMOD_GUI) != 0;

			KeyUp.Invoke(new KeyEventArgs(key, alt, control, shift, gui));
		}

		static Vector2 _lastPointerPosition;
		static DateTime _lastClickTime;
		internal static void OnMouseDown(ref SDL.SDL_Event evt)
		{
			_lastClickTime = DateTime.Now;

			var position = new Vector2(evt.button.x, evt.button.y);

			// Scale the mouse coordinates for the faux-backbuffer
			position.X = (int) ((double) position.X * Mouse.INTERNAL_BackBufferWidth / Mouse.INTERNAL_WindowWidth);
			position.Y = (int) ((double) position.Y * Mouse.INTERNAL_BackBufferHeight / Mouse.INTERNAL_WindowHeight);
			_lastPointerPosition = position;

			if (PointerDown == null) return;

			PointerDown.Invoke(new PointerEventArgs
			{
				ClickTime = _lastClickTime,
				Button = (InputButton)(1 << (evt.button.button - 1)),
				Delta = position - _lastPointerPosition,
				Position = position,
			});
		}

		internal static void OnMouseUp(ref SDL.SDL_Event evt)
		{
			var position = new Vector2(evt.button.x, evt.button.y);

			// Scale the mouse coordinates for the faux-backbuffer
			position.X = (int) ((double) position.X * Mouse.INTERNAL_BackBufferWidth / Mouse.INTERNAL_WindowWidth);
			position.Y = (int) ((double) position.Y * Mouse.INTERNAL_BackBufferHeight / Mouse.INTERNAL_WindowHeight);
			_lastPointerPosition = position;

			if (PointerUp == null) return;

			PointerUp.Invoke(new PointerEventArgs
			{
				ClickTime = _lastClickTime,
				Button = (InputButton)(1 << (evt.button.button - 1)),
				Delta = position - _lastPointerPosition,
				Position = position,
			});
		}

		internal static void OnMouseMove(ref SDL.SDL_Event evt)
		{
			var position = new Vector2(evt.motion.x, evt.motion.y);

			// Scale the mouse coordinates for the faux-backbuffer
			position.X = (int) ((double) position.X * Mouse.INTERNAL_BackBufferWidth / Mouse.INTERNAL_WindowWidth);
			position.Y = (int) ((double) position.Y * Mouse.INTERNAL_BackBufferHeight / Mouse.INTERNAL_WindowHeight);
			_lastPointerPosition = position;

			if (PointerMove == null) return;

			PointerMove.Invoke(new PointerEventArgs
			{
				ClickTime = _lastClickTime,
				Button = (InputButton)evt.motion.state,
				Delta = position - _lastPointerPosition,
				Position = position,
			});
		}

		internal static void OnMouseWheel(ref SDL.SDL_Event evt)
		{
			if (PointerScroll == null) return;

			// 120 units per notch. Because reasons.
			const int wheelDelta = 120;

			if (evt.wheel.y != 0)
			{
				PointerScroll.Invoke(new PointerEventArgs {
					ClickTime = _lastClickTime,
					Button = InputButton.Middle,
					Position = _lastPointerPosition,
					ScrollDelta = new Vector2(0, evt.wheel.y * wheelDelta)
				});
			}
			if (evt.wheel.x != 0)
			{
				PointerScroll.Invoke(new PointerEventArgs {
					ClickTime = _lastClickTime,
					Button = InputButton.Middle,
					Position = _lastPointerPosition,
					ScrollDelta = new Vector2(evt.wheel.x * wheelDelta, 0)
				});
			}
		}

		internal static void OnFingerDown(ref SDL.SDL_Event evt)
		{
			_lastClickTime = DateTime.Now;

			if (PointerDown == null) return;

			PointerDown.Invoke(new PointerEventArgs
			{
				ClickTime = _lastClickTime,
				PointerId = (int) evt.tfinger.fingerID,
				Button = InputButton.Left,
				Delta = Vector2.Zero,
				Position = new Vector2((float) Math.Round(evt.tfinger.x * TouchPanel.DisplayWidth), (float) Math.Round(evt.tfinger.y * TouchPanel.DisplayHeight))
			});
		}

		internal static void OnFingerMove(ref SDL.SDL_Event evt)
		{
			if (PointerMove == null) return;

			PointerMove.Invoke(new PointerEventArgs
			{
				ClickTime = _lastClickTime,
				PointerId = (int) evt.tfinger.fingerID,
				Button = InputButton.Left,
				Delta = new Vector2((float) Math.Round(evt.tfinger.dx * TouchPanel.DisplayWidth), (float) Math.Round(evt.tfinger.dy * TouchPanel.DisplayHeight)),
				Position = new Vector2((float) Math.Round(evt.tfinger.x * TouchPanel.DisplayWidth), (float) Math.Round(evt.tfinger.y * TouchPanel.DisplayHeight))
			});
		}

		internal static void OnFingerUp(ref SDL.SDL_Event evt)
		{
			if (PointerUp == null) return;

			PointerUp.Invoke(new PointerEventArgs
			{
				ClickTime = _lastClickTime,
				PointerId = (int) evt.tfinger.fingerID,
				Button = InputButton.Left,
				Delta = Vector2.Zero,
				Position = new Vector2((float) Math.Round(evt.tfinger.x * TouchPanel.DisplayWidth), (float) Math.Round(evt.tfinger.y * TouchPanel.DisplayHeight))
			});
		}

		#endregion
	}

	/// <summary>
	/// Provides data for the <see cref="InputEventEXT.KeyDown"/> or <see cref="InputEventEXT.KeyUp"/> event.
	/// </summary>
	public class KeyEventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="KeyEventArgs"/> class.
		/// </summary>
		public KeyEventArgs(Keys key, bool alt, bool control, bool shift, bool gui = false)
		{
			Key = key;
			Alt = alt;
			Control = control;
			Shift = shift;
			Gui = gui;
		}

		/// <summary>
		/// Gets or sets a value indicating whether the event was handled.
		/// </summary>
		public bool Handled;

		/// <summary>
		/// Gets the keyboard code for a KeyDown or KeyUp event.
		/// </summary>
		public Keys Key;

		/// <summary>
		/// Gets a value indicating whether the ALT key was pressed.
		/// </summary>
		public bool Alt;

		/// <summary>
		/// Gets a value indicating whether the CTRL key was pressed.
		/// </summary>
		public bool Control;

		/// <summary>
		/// Gets a value indicating whether the SHIFT key was pressed.
		/// </summary>
		public bool Shift;

		/// <summary>
		/// Gets a value indicating whether the GUI key(often the Windows key) was pressed.
		/// </summary>
		public bool Gui;
	}

	/// <summary>
	/// Provides data for the <see cref="InputEventEXT.KeyPress"/> event.
	/// </summary>
	public class KeyPressEventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="KeyPressEventArgs"/> class.
		/// </summary>
		public KeyPressEventArgs(Keys key, char keyChar)
		{
			Key = key;
			KeyChar = keyChar;
		}

		/// <summary>
		/// Gets or sets a value indicating whether the event was handled.
		/// </summary>
		public bool Handled;

		/// <summary>
		/// Gets the Key for a KeyPress event.
		/// </summary>
		public Keys Key;

		/// <summary>
		/// Gets the keyboard code for a KeyPress event.
		/// </summary>
		public char KeyChar;
	}

	/// <summary>
	/// Input press tracking.
	/// </summary>
	public enum InputButton
	{
		/// <summary>
		/// None.
		/// </summary>
		None = 0,

		/// <summary>
		/// Left button.
		/// </summary>
		Left = 1 << 0,

		/// <summary>
		/// Middle button.
		/// </summary>
		Middle = 1 << 1,

		/// <summary>
		/// Right button.
		/// </summary>
		Right = 1 << 2,

		/// <summary>
		/// X1 button.
		/// </summary>
		X1 = 1 << 3,

		/// <summary>
		/// X2 button.
		/// </summary>
		X2 = 1 << 4,
	}

	/// <summary>
	/// Provides data for the pointer events.
	/// </summary>
	public struct PointerEventArgs
	{
		/// <summary>
		/// Gets a unique identifier of the pointer. See remarks.
		/// </summary>
		/// <value>The pointer id.</value>
		/// <remarks>The default mouse pointer will always be affected to the PointerId 0. On a tablet, a pen or each fingers will get a unique identifier.</remarks>
		public int PointerId;

		/// <summary>
		/// The <see cref="InputButton"/> for this event.
		/// </summary>
		public InputButton Button;

		/// <summary>
		/// The last time a click event was sent. Used for double click.
		/// </summary>
		public DateTime ClickTime;

		/// <summary>
		/// Pointer delta since last update.
		/// </summary>
		public Vector2 Delta;

		/// <summary>
		/// Gets the screen position of the pointer.
		/// </summary>
		public Vector2 Position;

		/// <summary>
		/// The amount of scroll since the last update.
		/// </summary>
		public Vector2 ScrollDelta;
	}
}
