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
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.InteropServices;
#endregion

namespace Microsoft.Xna.Framework.Input
{
	/// <summary>
	/// Traditional Winform-like event based input system.
	/// </summary>
	public static class MouseCursorEXT
	{
		/// <summary>
		/// Sets the cursor image to the specified MouseCursor.
		/// </summary>
		/// <param name="cursor">Mouse cursor to use for the cursor image.</param>
		public static void SetCursor(MouseCursor cursor)
		{
			SDL.SDL_SetCursor(cursor.Handle);
		}

		/// <summary>
		/// Creates a mouse cursor from the specified texture.
		/// </summary>
		/// <param name="texture">Texture to use as the cursor image.</param>
		/// <param name="originx">X cordinate of the image that will be used for mouse position.</param>
		/// <param name="originy">Y cordinate of the image that will be used for mouse position.</param>
		public static MouseCursor CreateFromTexture2D(Texture2D texture, int originx, int originy)
		{
			if (texture.Format != SurfaceFormat.Color)
				throw new ArgumentException("Only Color textures are accepted for mouse cursors", "texture");

			IntPtr surface = IntPtr.Zero;
			IntPtr handle = IntPtr.Zero;
			try
			{
				var bytes = new byte[texture.Width * texture.Height * 4];
				texture.GetData(bytes);

				GCHandle gcHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
				surface = SDL.SDL_CreateSurfaceFrom(texture.Width, texture.Height, SDL.SDL_PixelFormat.SDL_PIXELFORMAT_XRGB8888, gcHandle.AddrOfPinnedObject(), texture.Width * 4);
				gcHandle.Free();
				if (surface == IntPtr.Zero)
					throw new InvalidOperationException("Failed to create surface for mouse cursor: " + SDL.SDL_GetError());

				handle = SDL.SDL_CreateColorCursor(surface, originx, originy);
				if (handle == IntPtr.Zero)
					throw new InvalidOperationException("Failed to set surface for mouse cursor: " + SDL.SDL_GetError());
			}
			finally
			{
				if (surface != IntPtr.Zero)
					SDL.SDL_DestroySurface(surface);
			}

			return new MouseCursor(handle);
		}
	}

	public class MouseCursor : IDisposable
	{
		/// <summary>
		/// Gets the default arrow cursor.
		/// </summary>
		public static readonly MouseCursor Arrow;

		/// <summary>
		/// Gets the cursor that appears when the mouse is over text editing regions.
		/// </summary>
		public static readonly MouseCursor IBeam;

		/// <summary>
		/// Gets the waiting cursor that appears while the application/system is busy.
		/// </summary>
		public static readonly MouseCursor Wait;

		/// <summary>
		/// Gets the crosshair ("+") cursor.
		/// </summary>
		public static readonly MouseCursor Crosshair;

		/// <summary>
		/// Gets the cross between Arrow and Wait cursors.
		/// </summary>
		public static readonly MouseCursor WaitArrow;

		/// <summary>
		/// Gets the northwest/southeast ("\") cursor.
		/// </summary>
		public static readonly MouseCursor SizeNWSE;

		/// <summary>
		/// Gets the northeast/southwest ("/") cursor.
		/// </summary>
		public static readonly MouseCursor SizeNESW;

		/// <summary>
		/// Gets the horizontal west/east ("-") cursor.
		/// </summary>
		public static readonly MouseCursor SizeWE;

		/// <summary>
		/// Gets the vertical north/south ("|") cursor.
		/// </summary>
		public static readonly MouseCursor SizeNS;

		/// <summary>
		/// Gets the size all cursor which points in all directions.
		/// </summary>
		public static readonly MouseCursor SizeAll;

		/// <summary>
		/// Gets the cursor that points that something is invalid, usually a cross.
		/// </summary>
		public static readonly MouseCursor No;

		/// <summary>
		/// Gets the hand cursor, usually used for web links.
		/// </summary>
		public static readonly MouseCursor Hand;

		public IntPtr Handle { get; private set; }

		private bool _disposed;

		static MouseCursor()
		{
			Arrow = new MouseCursor(SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_DEFAULT);
			IBeam = new MouseCursor(SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_TEXT);
			Wait = new MouseCursor(SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_WAIT);
			Crosshair = new MouseCursor(SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_CROSSHAIR);
			WaitArrow = new MouseCursor(SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_PROGRESS);
			SizeNWSE = new MouseCursor(SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_NWSE_RESIZE);
			SizeNESW = new MouseCursor(SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_NESW_RESIZE);
			SizeWE = new MouseCursor(SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_EW_RESIZE);
			SizeNS = new MouseCursor(SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_NS_RESIZE);
			SizeAll = new MouseCursor(SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_MOVE);
			No = new MouseCursor(SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_NOT_ALLOWED);
			Hand = new MouseCursor(SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_POINTER);
		}

		internal MouseCursor(IntPtr handle)
		{
			Handle = handle;
		}

		public void Dispose()
		{
			if (_disposed)
				return;

			if (Handle == IntPtr.Zero)
				return;

			SDL.SDL_DestroyCursor(Handle);
			Handle = IntPtr.Zero;

			_disposed = true;
		}
		private MouseCursor(SDL.SDL_SystemCursor cursor)
		{
			Handle = SDL.SDL_CreateSystemCursor(cursor);
		}
	}
}
