using Microsoft.Win32;

namespace SecureChat.Client
{
    public static class Themes
    {
        /// <summary>
        /// Color used for display name when message are from the local client.
        /// </summary>
        public readonly static Color FromLocalColor = Color.DodgerBlue;

        /// <summary>
        /// Color used for display name when message are from remote clients.
        /// </summary>
        public readonly static Color FromRemoteColor = Color.DeepSkyBlue;

        public static bool IsWindowsDarkMode()
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
                if (key != null)
                {
                    var value = key.GetValue("AppsUseLightTheme");
                    if (value is int intValue)
                    {
                        return intValue == 0; // 0 means Dark Mode
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public static Color GetContrastingColor(Color bgColor)
        {
            // Perceived brightness formula
            double brightness = (0.299 * bgColor.R + 0.587 * bgColor.G + 0.114 * bgColor.B);

            return brightness > 186 ? Color.Black : Color.White;
        }

        public static Color InvertColor(Color color)
        {
            return Color.FromArgb(color.A, 255 - color.R, 255 - color.G, 255 - color.B);
        }

        public static Color AdjustBrightness(Color color, float factor)
        {
            // factor > 1.0 = lighter, < 1.0 = darker
            int r = Math.Min(255, (int)(color.R * factor));
            int g = Math.Min(255, (int)(color.G * factor));
            int b = Math.Min(255, (int)(color.B * factor));
            return Color.FromArgb(color.A, r, g, b);
        }

        public static bool IsThemeDark()
        {
            return Settings.Instance.Theme.ToString().Contains("Dark")
                || Settings.Instance.Theme.ToString().Contains("Black");
        }

        public static Color ChooseColor(Color ifLight, Color ifDark)
        {
            return IsThemeDark() ? ifDark : ifLight;
        }
    }
}
