using NTDLS.ReliableMessaging;

namespace SecureChat.Client.Models
{
    /// <summary>
    /// Result class for the FormLogin.
    /// </summary>
    internal class LoginResult
    {
        public RmClient Client { get; private set; }
        public Guid AccountId { get; set; }

        public string Username { get; private set; }
        public string DisplayName { get; private set; }

        public LoginResult(RmClient client, Guid accountId, string username, string displayName)
        {
            Client = client;
            AccountId = accountId;
            Username = username;
            DisplayName = displayName;
        }
    }
}
