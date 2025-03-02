using System.Runtime.InteropServices;

namespace SecureChat.Client
{
    class Interop
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }

        [DllImport("user32.dll")]
        public static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        public static TimeSpan GetIdleTime()
        {
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            if (GetLastInputInfo(ref lastInputInfo))
            {
                uint lastInputTick = lastInputInfo.dwTime;
                uint currentTick = unchecked((uint)Environment.TickCount);
                uint idleTicks = currentTick - lastInputTick;
                return TimeSpan.FromMilliseconds(idleTicks);
            }
            else
            {
                throw new Exception("Error retrieving last input info.");
            }
        }
    }
}
