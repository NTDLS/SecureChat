using SecureChat.Library;

namespace SecureChat.Client
{
    /// <summary>
    /// State that is saved to disk.
    /// </summary>
    internal class PersistedSettings
    {
        public string ServerAddress { get; set; } = ScConstants.DefaultServerAddress;
        public int ServerPort { get; set; } = ScConstants.DefaultServerPort;

        /// <summary>
        /// Per user state that is saved to disk.
        /// </summary>
        public Dictionary<string, PersistedUserState> Users = new(StringComparer.CurrentCultureIgnoreCase);
    }
}
