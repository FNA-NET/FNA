#if ANDROID

using System;
using System.Runtime.InteropServices;

namespace SDL2
{
    public delegate void MainFunc();

    public static class SDL2Main
    {
        [DllImport("main")]
        public static extern void SetMain(MainFunc main);
    }
}

#endif