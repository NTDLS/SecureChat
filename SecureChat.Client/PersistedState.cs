namespace SecureChat.Client
{
    /// <summary>
    /// State that is saved to disk.
    /// </summary>
    internal class PersistedState
    {
        /// <summary>
        /// Per user state that is saved to disk.
        /// </summary>
        public Dictionary<string, PersistedUserState> Users = new(StringComparer.CurrentCultureIgnoreCase);
    }
}
