// Copyright (C) ryancheung
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// Arguments for the <see cref="ImmService.TextComposition" /> event.
    /// </summary>
    public struct TextCompositionEventArgs
    {
        /// <summary>
        /// Construct a text composition event.
        /// </summary>
        public TextCompositionEventArgs(IMEString compositionText, int cursorPosition)
        {
            CompositionText = compositionText;
            CursorPosition = cursorPosition;
        }

        /// <summary>
        /// The full string as it's composed by the IMM.
        /// </summary>    
        public readonly IMEString CompositionText;

        /// <summary>
        /// The position of the cursor inside the composed string.
        /// </summary>    
        public readonly int CursorPosition;
    }
}
