#region Using Statements
using System;
using SDL2;
using Microsoft.Xna.Framework.Input.Touch;
#endregion

namespace Microsoft.Xna.Framework.Input
{
	partial class InputEventEXT
	{
		internal static void OnKeyDown(Keys key, ref SDL.SDL_Event evt)
		{
			if (KeyDown == null) return;

			char character = (char)evt.key.keysym.sym;
			var alt = (evt.key.keysym.mod & SDL.SDL_Keymod.KMOD_ALT) != 0;
			var control = (evt.key.keysym.mod & SDL.SDL_Keymod.KMOD_CTRL) != 0;
			var shift = (evt.key.keysym.mod & SDL.SDL_Keymod.KMOD_SHIFT) != 0;
			var gui = (evt.key.keysym.mod & SDL.SDL_Keymod.KMOD_GUI) != 0;

			KeyDown.Invoke(new KeyEventArgs(key, alt, control, shift, gui));

			if (KeyPress != null)
				if (char.IsControl(character) || KeysHelper.GetKeyChar(key, shift, out character))
					KeyPress.Invoke(new KeyPressEventArgs(key, character));
		}

		internal static void OnKeyUp(Keys key, ref SDL.SDL_Event evt)
		{
			if (KeyUp == null) return;

			var alt = (evt.key.keysym.mod & SDL.SDL_Keymod.KMOD_ALT) != 0;
			var control = (evt.key.keysym.mod & SDL.SDL_Keymod.KMOD_CTRL) != 0;
			var shift = (evt.key.keysym.mod & SDL.SDL_Keymod.KMOD_SHIFT) != 0;
			var gui = (evt.key.keysym.mod & SDL.SDL_Keymod.KMOD_GUI) != 0;

			KeyUp.Invoke(new KeyEventArgs(key, alt, control, shift, gui));
		}

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
				PointerId = (int) evt.tfinger.fingerId,
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
				PointerId = (int) evt.tfinger.fingerId,
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
				PointerId = (int) evt.tfinger.fingerId,
				Button = InputButton.Left,
				Delta = Vector2.Zero,
				Position = new Vector2((float) Math.Round(evt.tfinger.x * TouchPanel.DisplayWidth), (float) Math.Round(evt.tfinger.y * TouchPanel.DisplayHeight))
			});
		}
	}
}