using NTDLS.Persistence;
using NTDLS.ReliableMessaging;
using SecureChat.Library;

namespace SecureChat.Client.Models
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

        public string ServerAddress { get; set; } = ScConstants.DefaultServerAddress;
        public string Font { get; set; } = ScConstants.DefaultFont;
        public float FontSize { get; set; } = ScConstants.DefaultFontSize;
        public int ServerPort { get; set; } = ScConstants.DefaultServerPort;
        public int AutoAwayIdleSeconds { get; set; } = ScConstants.DefaultAutoAwayIdleSeconds;
        public int MaxMessages { get; set; } = ScConstants.DefaultMaxMessages;
        public int FileTransmissionChunkSize { get; set; } = ScConstants.DefaultFileTransmissionChunkSize;
        public int MaxFileTransmissionSize { get; set; } = ScConstants.DefaultMaxFileTransmissionSize;

        /// <summary>
        /// Per user state that is saved to disk.
        /// </summary>
        public Dictionary<string, PersistedUserState> Users = new(StringComparer.CurrentCultureIgnoreCase);

        public RmClient CreateClient()
        {
            var client = new RmClient();
#if DEBUG
            client.Connect("127.0.0.1", ServerPort);
#else
            client.Connect(ServerAddress, ServerPort);
#endif
            return client;
        }
    }
}
