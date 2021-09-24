// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Microsoft.Xna.Framework.Input
{
    /// <summary>
    /// Provides data for the <see cref="GameWindow.KeyPress"/> event.
    /// </summary>
    public class KeyPressEventArgs : BaseInputEventArgs
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
        /// Gets the Key for a KeyPress event.
        /// </summary>
        public Keys Key { get; private set; }

        /// <summary>
        /// Gets the keyboard code for a KeyPress event.
        /// </summary>
        public char KeyChar { get; set; }
    }
}
