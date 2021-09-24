using System.Threading;

namespace Microsoft.Xna.Framework
{
    public class Threading
    {
        static int _mainThreadId;

        static Threading()
        {
            _mainThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        internal static void ResetThread (int id)
        {
            _mainThreadId = id;
        }

        /// <summary>
        /// Checks if the code is currently running on the UI thread.
        /// </summary>
        /// <returns>true if the code is currently running on the UI thread.</returns>
        public static bool IsOnUIThread()
        {
            return _mainThreadId == Thread.CurrentThread.ManagedThreadId;
        }
    }
}
