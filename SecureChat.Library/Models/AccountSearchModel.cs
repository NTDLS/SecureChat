namespace SecureChat.Library.Models
{
    public class AccountSearchModel
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string State { get; set; } = "Offline";
        public DateTime? LastSeen { get; set; }
        public bool IsExitingContact { get; set; }
    }
}
