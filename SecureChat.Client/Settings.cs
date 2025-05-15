using Krypton.Toolkit;
using NTDLS.Persistence;
using SecureChat.Client.Models;
using SecureChat.Library;

namespace SecureChat.Client
{
    /// <summary>
    /// Settings that are saved to disk.
    /// </summary>
    internal class Settings
    {
        private static Settings? _instance;
        internal static Settings Instance
        {
            get
            {
                _instance ??= LocalUserApplicationData.LoadFromDisk(ScConstants.AppName, new Settings());
                return _instance;
            }
            set
            {
                _instance = value;
                LocalUserApplicationData.SaveToDisk(ScConstants.AppName, _instance);
            }
        }

        public static void Save()
        {
            LocalUserApplicationData.SaveToDisk(ScConstants.AppName, Instance);
        }

        public PaletteMode Theme { get; set; } = Theming.IsWindowsDarkMode() ? PaletteMode.Microsoft365BlackDarkModeAlternate : PaletteMode.ProfessionalSystem;

        public string ServerAddress { get; set; } = ScConstants.DefaultServerAddress;
        public string Font { get; set; } = ScConstants.DefaultFont;
        public float FontSize { get; set; } = ScConstants.DefaultFontSize;
        public int ServerPort { get; set; } = ScConstants.DefaultServerPort;
        public int AutoAwayIdleSeconds { get; set; } = ScConstants.DefaultAutoAwayIdleSeconds;
        public int MaxMessages { get; set; } = ScConstants.DefaultMaxMessages;
        public int FileTransferChunkSize { get; set; } = ScConstants.DefaultFileTransferChunkSize;

        public int RsaKeySize { get; set; } = ScConstants.DefaultRsaKeySize;
        public int AesKeySize { get; set; } = ScConstants.DefaultAesKeySize;
        public int EndToEndKeySize { get; set; } = ScConstants.DefaultEndToEndKeySize;

        public bool AlertToastWhenContactComesOnline { get; set; } = true;
        public bool AlertToastWhenMessageReceived { get; set; } = true;
        public bool PlaySoundWhenContactComesOnline { get; set; } = true;
        public bool PlaySoundWhenMessageReceived { get; set; } = true;
        public bool FlashWindowWhenMessageReceived { get; set; } = true;

        /// <summary>
        /// Per user state that is saved to disk.
        /// </summary>
        public Dictionary<string, PersistedUserState> Users = new(StringComparer.CurrentCultureIgnoreCase);
    }
}
