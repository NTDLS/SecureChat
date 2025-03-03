namespace SecureChat.Server.Models
{
    public class AcquaintanceModel
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        /// <summary>
        /// Online, Offline or Away
        /// </summary>
        public string State { get; set; } = "Offline";
        /// <summary>
        /// Status text set by user.
        /// </summary>
        public string Status { get; set; } = string.Empty;
        public bool IsAccepted { get; set; }
        public DateTime? LastSeen { get; set; }
    }
}
