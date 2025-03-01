namespace SecureChat.Server.Models
{
    public class AcquaintancesModel
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string Status { get; set; } = "Offline";
        public int IsAccepted { get; set; }
        public DateTime? LastSeen { get; set; }
    }
}
