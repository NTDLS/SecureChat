using Microsoft.Win32;
using SecureChat.Library;

namespace SecureChat.Client
{
    class RegistryHelper
    {
        public static void EnableAutoStart()
        {
            using var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            key?.SetValue(ScConstants.AppName, $"\"{Application.ExecutablePath}\"");
        }

        public static void DisableAutoStart()
        {
            using var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            key?.DeleteValue(ScConstants.AppName, false);
        }

        public static bool IsAutoStartEnabled()
        {
            using var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", false);
            return key?.GetValue(ScConstants.AppName) != null;
        }
    }
}
