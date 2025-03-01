using NTDLS.ReliableMessaging;

namespace SecureChat.Client.Models
{
    internal class LoginResult
    {
        public RmClient Client { get; private set; }
        public string DisplayName { get; private set; }

        public LoginResult(RmClient client, string displayName)
        {
            Client = client;
            DisplayName = displayName;
        }
    }
}
