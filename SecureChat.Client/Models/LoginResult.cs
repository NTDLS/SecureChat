using NTDLS.ReliableMessaging;
using SecureChat.Library;
using System.Text.Json;

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
        public string Username { get; private set; }
        public string ProfileJson { get; set; } = string.Empty;

        private AccountProfile? _profile = null;
        public AccountProfile Profile
        {
            get
            {
                _profile ??= JsonSerializer.Deserialize<AccountProfile>(ProfileJson) ?? new AccountProfile();
                return _profile;
            }
        }

        public LoginResult(RmClient client, Guid accountId, string username, string displayName, string profileJson)
        {
            AccountId = accountId;
            Client = client;
            DisplayName = displayName;
            ProfileJson = profileJson;
            Username = username;
        }
    }
}
