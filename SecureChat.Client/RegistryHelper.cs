using Microsoft.Win32;
using SecureChat.Library;

namespace SecureChat.Client
{
    class RegistryHelper
    {
        public static void AddApplicationToStartup()
        {
            using var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            key?.SetValue(ScConstants.AppName, $"\"{Application.ExecutablePath}\"");
        }

        public static void RemoveApplicationFromStartup()
        {
            using var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            key?.DeleteValue(ScConstants.AppName, false);
        }
    }
}
