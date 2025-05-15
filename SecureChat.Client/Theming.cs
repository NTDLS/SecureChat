using Microsoft.Win32;

namespace SecureChat.Client
{
    public static class Theming
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

        public static Color ShiftToContrast(Color foreground, Color background)
        {
            float h, s, l;
            RgbToHsl(foreground, out h, out s, out l);

            // Calculate background brightness
            double bgBrightness = (0.299 * background.R + 0.587 * background.G + 0.114 * background.B) / 255.0;

            // Adjust only slightly — enough to increase contrast, not destroy identity
            float delta = 0.15f;

            // Light background → darken foreground slightly
            if (bgBrightness > 0.6)
                l = Math.Max(0f, l - delta);
            else
                l = Math.Min(1f, l + delta);

            Color result = HslToRgb(h, s, l, foreground.A);

            // Ensure minimum contrast ratio (optional, uses luminance)
            if (!HasSufficientContrast(result, background))
            {
                // Invert brightness shift if still too low contrast
                l = bgBrightness > 0.6 ? Math.Max(0f, l - delta * 2f) : Math.Min(1f, l + delta * 2f);
                result = HslToRgb(h, s, l, foreground.A);
            }

            return result;
        }

        // WCAG-style contrast checker
        public static bool HasSufficientContrast(Color fg, Color bg, double minRatio = 3.0)
        {
            double lum1 = GetRelativeLuminance(fg);
            double lum2 = GetRelativeLuminance(bg);
            double ratio = (Math.Max(lum1, lum2) + 0.05) / (Math.Min(lum1, lum2) + 0.05);
            return ratio >= minRatio;
        }

        public static double GetRelativeLuminance(Color c)
        {
            double R = c.R / 255.0;
            double G = c.G / 255.0;
            double B = c.B / 255.0;

            R = (R <= 0.03928) ? R / 12.92 : Math.Pow((R + 0.055) / 1.055, 2.4);
            G = (G <= 0.03928) ? G / 12.92 : Math.Pow((G + 0.055) / 1.055, 2.4);
            B = (B <= 0.03928) ? B / 12.92 : Math.Pow((B + 0.055) / 1.055, 2.4);

            return 0.2126 * R + 0.7152 * G + 0.0722 * B;
        }

        // Helper: Convert RGB to HSL
        public static void RgbToHsl(Color color, out float h, out float s, out float l)
        {
            float r = color.R / 255f;
            float g = color.G / 255f;
            float b = color.B / 255f;

            float max = Math.Max(r, Math.Max(g, b));
            float min = Math.Min(r, Math.Min(g, b));
            h = s = l = (max + min) / 2f;

            if (max == min)
            {
                h = s = 0f; // achromatic
            }
            else
            {
                float d = max - min;
                s = l > 0.5f ? d / (2f - max - min) : d / (max + min);

                if (max == r)
                    h = (g - b) / d + (g < b ? 6f : 0f);
                else if (max == g)
                    h = (b - r) / d + 2f;
                else
                    h = (r - g) / d + 4f;

                h /= 6f;
            }
        }

        // Helper: Convert HSL to RGB
        public static Color HslToRgb(float h, float s, float l, int alpha = 255)
        {
            float r, g, b;

            if (s == 0)
            {
                r = g = b = l; // achromatic
            }
            else
            {
                Func<float, float, float, float> hue2rgb = (p, q, t) =>
                {
                    if (t < 0f) t += 1f;
                    if (t > 1f) t -= 1f;
                    if (t < 1f / 6f) return p + (q - p) * 6f * t;
                    if (t < 1f / 2f) return q;
                    if (t < 2f / 3f) return p + (q - p) * (2f / 3f - t) * 6f;
                    return p;
                };

                float q = l < 0.5f ? l * (1f + s) : l + s - l * s;
                float p = 2f * l - q;

                r = hue2rgb(p, q, h + 1f / 3f);
                g = hue2rgb(p, q, h);
                b = hue2rgb(p, q, h - 1f / 3f);
            }

            return Color.FromArgb(alpha, (int)(r * 255), (int)(g * 255), (int)(b * 255));
        }

    }
}
