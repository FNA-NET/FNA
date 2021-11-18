// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.ObjectModel;

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// Interface to handle text composition.
    /// </summary>
    public abstract class ImmService
    {
        /// <summary>
        /// Show the IME Candidate window rendered by the OS. Defaults to true.
        /// Set to <c>false</c> if you want to render the IME candidate list yourself.
        /// This is a Windows only API.
		/// Note there's no way to toggle this option while game running! Please set this value main function or static initializer.
        /// </summary>
        public static bool ShowOSImeWindow { get; set; } = true;

        /// <summary>
        /// Enable the system IMM service to support composited character input.
        /// This should be called when you expect text input from a user and you support languages
        /// that require an IME (Input Method Editor).
        /// </summary>
        public abstract void StartTextInput();

        /// <summary>
        /// Stop the system IMM service.
        /// </summary>
        public abstract void StopTextInput();

        /// <summary>
        /// Returns true if text input is enabled, else returns false.
        /// </summary>
        public bool IsTextInputActive { get; protected set; }

        /// <summary>
        /// Set the position of the candidate window rendered by the OS.
        /// Let the OS render the candidate window by setting <see cref="ShowOSImeWindow"/> to <c>true</c>.
        ///
        /// This API is supported on Desktop platforms.
        /// </summary>
        public virtual void SetTextInputRect(Rectangle rect) {}

        /// <summary>
        /// Invoked when the IMM service is enabled and a character composition is changed.
        /// </summary>
        public abstract event EventHandler<TextCompositionEventArgs> TextComposition;

        /// <summary>
        /// Invoked when the IMM service generates a composition result.
        /// </summary>
        public abstract event EventHandler<TextInputEventArgs> TextInput;

#if NET
        /// <summary>
        /// The candidate text list for the current composition.
        /// This property is only supported on Windows.
        /// If the composition string does not generate candidates this array is empty.
        /// NOTE This array is fixed size of 16. Use <see cref="CandidatePageSize"/> to get actual canddiate size.
        /// </summary>
        public virtual ReadOnlySpan<IMEString> CandidateList() { return ReadOnlySpan<IMEString>.Empty; }

        /// <summary>
        /// The selected candidate index.
        /// </summary>
        public virtual int CandidateSelection { get; }
#endif
    }
}
