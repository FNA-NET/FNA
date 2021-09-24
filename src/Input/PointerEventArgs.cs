// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Numerics;

namespace Microsoft.Xna.Framework.Input
{
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
    public class PointerEventArgs : BaseInputEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PointerEventArgs"/> class.
        /// </summary>
        public PointerEventArgs()
        {
            ClickTime = DateTime.Now;
        }

        /// <summary>
        /// Gets a unique identifier of the pointer. See remarks.
        /// </summary>
        /// <value>The pointer id.</value>
        /// <remarks>The default mouse pointer will always be affected to the PointerId 0. On a tablet, a pen or each fingers will get a unique identifier.</remarks>
        public int PointerId { get; internal set; }

        /// <summary>
        /// The <see cref="InputButton"/> for this event.
        /// </summary>
        public InputButton Button { get; internal set; }

        /// <summary>
        /// The last time a click event was sent. Used for double click.
        /// </summary>
        public DateTime ClickTime { get; internal set; }

        /// <summary>
        /// Pointer delta since last update.
        /// </summary>
        public Vector2 Delta { get; set; }

        /// <summary>
        /// Gets the screen position of the pointer.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The amount of scroll since the last update.
        /// </summary>
        public Vector2 ScrollDelta { get; internal set; }
    }
}

