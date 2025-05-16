namespace Talkster.Client.Models
{
    public class AutoLogin
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public AutoLogin(string username, string passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;
        }
    }
}
