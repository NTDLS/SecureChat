using SecureChat.Client.Models;

namespace SecureChat.Client
{
    public class Fonts
    {
        private static Fonts? _instance;
        internal static Fonts Instance
        {
            get
            {
                _instance ??= new Fonts();
                return _instance;
            }
        }

        public Font Italic { get; private set; }
        public Font Regular { get; private set; }
        public Font Bold { get; private set; }

        public Fonts()
        {
            Italic = new(Settings.Instance.Font, Settings.Instance.FontSize, FontStyle.Italic);
            Regular = new(Settings.Instance.Font, Settings.Instance.FontSize, FontStyle.Regular);
            Bold = new(Settings.Instance.Font, Settings.Instance.FontSize, FontStyle.Bold);
        }
    }
}
