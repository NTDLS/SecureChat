namespace SecureChat.Server.Models
{
    public class LoginModel
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Status { get; set; }
        public string? DisplayName { get; set; }
    }
}
