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

namespace Microsoft.Xna.Framework.Graphics
{
	/// <summary>
	/// Defines the buffers for clearing when calling <see cref="GraphicsDevice.Clear(ClearOptions, Vector4, float, int)"/> operation.
	/// </summary>
	[Flags]
	public enum ClearOptions
	{
		/// <summary>
		/// Color buffer.
		/// </summary>
		Target = 1,
		/// <summary>
		/// Depth buffer.
		/// </summary>
		DepthBuffer = 2,
		/// <summary>
		/// Stencil buffer.
		/// </summary>
		Stencil = 4
	}
}
