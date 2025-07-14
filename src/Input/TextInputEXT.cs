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
#endregion

namespace Microsoft.Xna.Framework.Input
{
	public static class TextInputEXT
	{
		#region Event

		/// <summary>
		/// Use this event to retrieve text for objects like textboxes.
		/// This event is not raised by noncharacter keys.
		/// This event also supports key repeat.
		/// For more information this event is based off:
		/// http://msdn.microsoft.com/en-AU/library/system.windows.forms.control.keypress.aspx
		/// </summary>
		public static event Action<char> TextInput;

		/// <summary>
		/// This event notifies you of in-progress text composition happening in an IME or other tool
		///  and allows you to display the draft text appropriately before it has become input.
		/// For more information, see SDL's tutorial: https://wiki.libsdl.org/Tutorials-TextInput
		/// </summary>
		public static event Action<string, int, int> TextEditing;

		#endregion

		#region Public Properties

		static IntPtr windowHandle;
		public static IntPtr WindowHandle
		{
			get => windowHandle;
			set
			{
#if WINDOWS7_0_OR_GREATER && !WINDOWS10_0_17763_0_OR_GREATER
				if (value != IntPtr.Zero && windowHandle != value)
					AdvancedImeInit(value);
#endif
				windowHandle = value;
			}
		}

		#endregion

		#region Public Static Methods

		/// <summary>
		/// Returns if text input state is active
		///
		/// Note: For on-screen keyboard, this may remain true on
		/// some platforms if an external event closed the keyboard.
		/// In this case, check IsScreenKeyboardShow instead.
		/// </summary>
		/// <returns>True if text input state is active</returns>
		public static bool IsTextInputActive()
		{
#if WINDOWS7_0_OR_GREATER && !WINDOWS10_0_17763_0_OR_GREATER
			return ImeSharp.InputMethod.Enabled;
#else
			return FNAPlatform.IsTextInputActive(WindowHandle);
#endif
		}

		public static bool IsScreenKeyboardShown()
		{
			return FNAPlatform.IsScreenKeyboardShown(WindowHandle);
		}

		public static bool IsScreenKeyboardShown(IntPtr window)
		{
			return FNAPlatform.IsScreenKeyboardShown(window);
		}

		public static void StartTextInput()
		{
#if WINDOWS7_0_OR_GREATER && !WINDOWS10_0_17763_0_OR_GREATER
			// Need to ensure SDL2 text input is stopped
			FNAPlatform.StopTextInput(WindowHandle);
			ImeSharp.InputMethod.Enabled = true;
#else
			FNAPlatform.StartTextInput(WindowHandle);
#endif
		}

		public static void StopTextInput()
		{
#if WINDOWS7_0_OR_GREATER && !WINDOWS10_0_17763_0_OR_GREATER
			ImeSharp.InputMethod.Enabled = false;
#else
			FNAPlatform.StopTextInput(WindowHandle);
#endif
		}

		/// <summary>
		/// Sets the location within the game window where the text input is located.
		/// This is used to set the location of the IME suggestions
		/// </summary>
		/// <param name="rectangle">Text input location relative to GameWindow.ClientBounds</param>
		public static void SetInputRectangle(Rectangle rectangle)
		{
#if WINDOWS7_0_OR_GREATER && !WINDOWS10_0_17763_0_OR_GREATER
			if (ImeSharp.InputMethod.Enabled)
				ImeSharp.InputMethod.SetTextInputRect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
#else
			FNAPlatform.SetTextInputRectangle(WindowHandle, rectangle);
#endif
		}

		#endregion

		#region Internal Event Access Method

		internal static void OnTextInput(char c)
		{
			if (TextInput != null)
			{
				TextInput(c);
			}
		}

		internal static void OnTextEditing(string text, int start, int length)
		{
			if (TextEditing != null)
			{
				TextEditing(text, start, length);
			}
		}

		#endregion

#if WINDOWS7_0_OR_GREATER && !WINDOWS10_0_17763_0_OR_GREATER
		internal static void AdvancedImeInit(IntPtr sdlWindowHandle)
		{
			var windowProps = SDL3.SDL.SDL_GetWindowProperties(sdlWindowHandle);
			nint hwnd = SDL3.SDL.SDL_GetPointerProperty(windowProps, SDL3.SDL.SDL_PROP_WINDOW_WIN32_HWND_POINTER, IntPtr.Zero);

			// Only initialize InputMethod once
			if (ImeSharp.InputMethod.WindowHandle == IntPtr.Zero)
				ImeSharp.InputMethod.Initialize(hwnd, ShowOSImeWindow);

            ImeSharp.InputMethod.TextInputCallback = OnTextInput;
            ImeSharp.InputMethod.TextCompositionCallback = (compositionText, cursorPosition) => {
				OnTextEditing(compositionText, cursorPosition, 0);
			};
		}

		/// <summary>
		/// Show the IME Candidate window rendered by the OS. Defaults to true.<br/>
		/// Set to <c>false</c> if you want to render the IME candidate list yourself.<br/>
		/// Note there's no way to toggle this option while game running! Please set this value main function or static initializer.<br/>
		/// **This is a Windows only API.**
		/// </summary>
		public static bool ShowOSImeWindow;

		/// <summary>
		/// The candidate text list for the current composition.<br/>
		/// If the composition string does not generate candidates, the candidate page size is zero.
		/// This array is fixed length of 16.<br/>
		/// **This property is only supported on Windows.**
		/// </summary>
		public static ImeSharp.IMEString[] CandidateList => ImeSharp.InputMethod.CandidateList;

		/// <summary>
		/// IME Candidate page size.<br/>
		/// **This property is only supported on Windows.**
		/// </summary>
		public static int CandidatePageSize => ImeSharp.InputMethod.CandidatePageSize;

		/// <summary>
		/// The selected IME candidate index.<br/>
		/// **This property is only supported on Windows.**
		/// </summary>
		public static int CandidateSelection => ImeSharp.InputMethod.CandidateSelection;
#endif
	}
}
