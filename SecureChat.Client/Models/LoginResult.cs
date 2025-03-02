using NTDLS.ReliableMessaging;

namespace SecureChat.Client.Models
{
    /// <summary>
    /// Result class for the FormLogin.
    /// </summary>
    internal class LoginResult
    {
        public Guid AccountId { get; set; }
        public RmClient Client { get; private set; }
        public string DisplayName { get; private set; }
        public string Status { get; private set; }
        public string Username { get; private set; }

        public LoginResult(RmClient client, Guid accountId, string username, string displayName, string status)
        {
            AccountId = accountId;
            Client = client;
            DisplayName = displayName;
            Status = status;
            Username = username;
        }
    }
}
