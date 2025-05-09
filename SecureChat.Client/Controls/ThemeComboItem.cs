using Krypton.Toolkit;

namespace SecureChat.Client.Controls
{
    internal class ThemeComboItem
    {
        public string Text { get; set; }
        public PaletteMode Mode { get; set; }

        public ThemeComboItem(string text, PaletteMode mode)
        {
            Text = text;
            Mode = mode;
        }

        public override string ToString()
        {
            return Text.ToString();
        }
    }
}
