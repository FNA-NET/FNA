#region License
/* FNA - XNA4 Reimplementation for Desktop Platforms
 * Copyright 2021-2024 ryancheung
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */
#endregion

#region Using Statements
using System;
using SDL3;
using Microsoft.Xna.Framework.Input;
#endregion

namespace Microsoft.Xna.Framework
{
	public static class GameWindowEXT
	{
		/// <summary>
		/// Gets or sets position of game-window.
		/// </summary>
		public static Point Position
		{
			get
			{
				var windowBounds = FNAPlatform.GetWindowBounds(Mouse.WindowHandle);
				return windowBounds.Location;
			}
			set
			{
				SDL.SDL_SetWindowPosition(Mouse.WindowHandle, value.X, value.Y);
			}
		}
	}
}
