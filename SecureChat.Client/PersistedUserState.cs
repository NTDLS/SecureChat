namespace SecureChat.Client
{
    /// <summary>
    /// Per user state that is saved to disk.
    /// </summary>
    internal class PersistedUserState
    {
        /// <summary>
        /// Whether the locally logged in user has set their status to away.
        /// </summary>
        public bool ExplicitAway { get; set; } = false;
        public DateTime LastLogin { get; set; } = DateTime.UtcNow;
    }
}
