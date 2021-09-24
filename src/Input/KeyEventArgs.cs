// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Microsoft.Xna.Framework.Input
{
    /// <summary>
    /// Provides data for the <see cref="GameWindow.KeyDown"/> or <see cref="GameWindow.KeyUp"/> event.
    /// </summary>
    public class KeyEventArgs : BaseInputEventArgs
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
        /// Gets the keyboard code for a KeyDown or KeyUp event.
        /// </summary>
        public Keys Key { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the ALT key was pressed.
        /// </summary>
        public bool Alt { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the CTRL key was pressed.
        /// </summary>
        public bool Control { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the SHIFT key was pressed.
        /// </summary>
        public bool Shift { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the GUI key(often the Windows key) was pressed.
        /// </summary>
        public bool Gui { get; private set; }
    }
}
