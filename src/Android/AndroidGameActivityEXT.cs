#if ANDROID

using System;
using System.Runtime.Versioning;
using System.Runtime.InteropServices;
using Android.App;
using Android.Widget;
using Android.OS;
using Org.Libsdl.App;
using Android.Views;

namespace Microsoft.Xna.Framework
{
	[Activity(
		MainLauncher = true,
		HardwareAccelerated = true
	)]
	public abstract class AndroidGameActivityEXT : SDLActivity
	{
		public delegate void MainFunc();

		[DllImport("main")]
		static extern void SetMain(MainFunc main);

		[SupportedOSPlatform("android21.0")]
		public override void LoadLibraries() {
			base.LoadLibraries();

			SetMain(SDLMain);
		}

		protected abstract void SDLMain();
	}

}

#endif
