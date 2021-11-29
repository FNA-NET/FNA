#if ANDROID

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
	public abstract class AndroidGameActivity : SDLActivity
	{
		public override void LoadLibraries() {
			base.LoadLibraries();

			SDL2.SDL2Main.SetMain(SDLMain);
		}

		protected abstract void SDLMain();
    }
}

#endif
