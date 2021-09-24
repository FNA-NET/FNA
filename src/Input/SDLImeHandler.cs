using System;

namespace Microsoft.Xna.Framework.Input
{
    /// <summary>
    /// Integrate IME to SDL2 platform.
    /// </summary>
    internal class SDLImeHandler : ImmService
    {
        public SDLImeHandler()
        {
        }

        public override event EventHandler<TextCompositionEventArgs> TextComposition;
        public override event EventHandler<TextInputEventArgs> TextInput;

        public void OnTextInput(char charInput, Keys key)
        {
            if (TextInput != null)
                TextInput.Invoke(this, new TextInputEventArgs(charInput, key));
        }

        public void OnTextComposition(IMEString compositionText, int cursorPosition)
        {
            if (TextComposition != null)
                TextComposition.Invoke(this, new TextCompositionEventArgs(compositionText, cursorPosition));
        }

        public override void StartTextInput()
        {
            if (IsTextInputActive)
                return;

			FNAPlatform.StartTextInput();
            IsTextInputActive = true;
        }

        public override void StopTextInput()
        {
			FNAPlatform.StopTextInput();
            IsTextInputActive = false;
        }

        public override void SetTextInputRect(Rectangle rect)
        {
            if (!IsTextInputActive)
                return;

			FNAPlatform.SetTextInputRectangle(rect);
        }
    }
}
