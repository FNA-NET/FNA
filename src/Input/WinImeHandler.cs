#if WINDOWS7_0

using System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using ImeSharp;

namespace Microsoft.Xna.Framework.Input
{
    /// <summary>
    /// Integrate IME to Windows platform with ImeSharp library.
    /// </summary>
    internal sealed class WinImeHandler : ImmService
    {
        private IntPtr _windowHandle;

        public override event EventHandler<TextCompositionEventArgs> TextComposition;
        public override event EventHandler<TextInputEventArgs> TextInput;

        public override ReadOnlySpan<IMEString> CandidateList() { return InputMethod.CandidateList.AsSpan(0, InputMethod.CandidatePageSize); }
        public override int CandidateSelection { get { return InputMethod.CandidateSelection; } }

        public override void StartTextInput()
        {
            // Need to ensure SDL2 text input is stopped
            FNAPlatform.StopTextInput();

            InputMethod.Enabled = true;
            IsTextInputActive = true;
        }

        public override void StopTextInput()
        {
            InputMethod.Enabled = false;
            IsTextInputActive = false;
        }

        /// <summary>
        /// Constructor, must be called when the window create.
        /// </summary>
        /// <param name="windowHandle">Handle of the window</param>
        internal WinImeHandler(IntPtr windowHandle)
        {
            _windowHandle = windowHandle;

            // Only initialize InputMethod once
            if (InputMethod.WindowHandle == IntPtr.Zero)
                InputMethod.Initialize(windowHandle, ShowOSImeWindow);

            InputMethod.TextInputCallback = OnTextInput;
            InputMethod.TextCompositionCallback = OnTextComposition;
        }

        public override void SetTextInputRect(Rectangle rect)
        {
            if (!InputMethod.Enabled)
                return;

            InputMethod.SetTextInputRect(rect.X, rect.Y, rect.Width, rect.Height);
        }

        private void OnTextInput(char charInput)
        {
            var key = Keys.None;
			if (!char.IsSurrogate(charInput))
				key = KeyboardUtil.ToXna(charInput);

            if (TextInput != null)
                TextInput.Invoke(this, new TextInputEventArgs(charInput, key));
        }

        private void OnTextComposition(IMEString compositionText, int cursorPosition)
        {
            if (TextComposition == null)
                return;

            TextComposition.Invoke(this, new TextCompositionEventArgs(compositionText, cursorPosition));
        }
    }
}

#endif
