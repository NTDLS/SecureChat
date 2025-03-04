namespace SecureChat.Library.Models
{
    public class AutoLoginModel
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public AutoLoginModel(string username, string passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;
        }
    }
}
