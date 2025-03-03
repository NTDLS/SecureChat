using NTDLS.Persistence;
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

        public string ServerAddress { get; set; } = "127.0.0.1";
        public string Font { get; set; } = "Cascadia Mono SemiLight";
        public float FontSize { get; set; } = 10.0f;
        public int ServerPort { get; set; } = 13265;
        public int AutoAwayIdleSeconds { get; set; } = 600;
        public int MaxMessages { get; set; } = 100;
        public int FileTransmissionChunkSize { get; set; } = 1024 * 8;
        public int MaxFileTransmissionSize { get; set; } = 1024 * 1024 * 10;

        /// <summary>
        /// Per user state that is saved to disk.
        /// </summary>
        public Dictionary<string, PersistedUserState> Users = new(StringComparer.CurrentCultureIgnoreCase);
    }
}
