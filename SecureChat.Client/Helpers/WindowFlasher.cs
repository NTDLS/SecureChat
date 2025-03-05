using Microsoft.Extensions.Caching.Memory;
using SecureChat.Client.Models;
using System.Runtime.InteropServices;

namespace SecureChat.Client.Helpers
{
    public static class WindowFlasher
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct FLASHWINFO
        {
            public uint cbSize;
            public nint hwnd;
            public uint dwFlags;
            public uint uCount;
            public uint dwTimeout;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        private const uint FLASHW_ALL = 3;         // Flash both title and taskbar
        private const uint FLASHW_TIMERNOFG = 12;  // Flash continuously until focused

        private static void FlashWindow(nint hWnd, int count)
        {
            FLASHWINFO fw = new FLASHWINFO
            {
                cbSize = (uint)Marshal.SizeOf(typeof(FLASHWINFO)),
                hwnd = hWnd,
                dwFlags = FLASHW_ALL | FLASHW_TIMERNOFG,
                uCount = (uint)count, // Flash indefinitely
                dwTimeout = 0
            };
            FlashWindowEx(ref fw);
        }

        private static readonly IMemoryCache _flashCache = new MemoryCache(new MemoryCacheOptions());

        /// <summary>
        /// Flashes the window if it is not focused or visible.
        /// Enforces a 10-second cooldown between flashes.
        /// Returns true if the window was flashed.
        /// </summary>
        public static bool FlashWindow(Form form, int count = 5)
        {
            if (!form.Focused || form.WindowState == FormWindowState.Minimized)
            {
                _flashCache.TryGetValue(form.Handle, out DateTime? lastFlash);

                if (lastFlash == null || (DateTime.UtcNow - lastFlash.Value).TotalSeconds > 10)
                {
                    if (Settings.Instance.FlashWindowWhenMessageReceived)
                    {
                        FlashWindow(form.Handle, count);
                    }
                    _flashCache.Set(form.Handle, DateTime.UtcNow, TimeSpan.FromSeconds(60));
                    return true;
                }
            }
            return false;
        }
    }
}
