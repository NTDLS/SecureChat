using System.Text.Json;
using Talkster.Library.Models;

namespace Talkster.Client.Models
{
    /// <summary>
    /// Result class for the FormLogin.
    /// </summary>
    internal class LoginResult
    {
        public Guid AccountId { get; set; }
        public NegotiatedConnection Connection { get; private set; }
        public string DisplayName { get; private set; }
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public string? ProfileJson { get; set; }

        private AccountProfileModel? _profile = null;
        public AccountProfileModel Profile
        {
            get
            {
                _profile ??= (string.IsNullOrEmpty(ProfileJson) ? null : JsonSerializer.Deserialize<AccountProfileModel>(ProfileJson)) ?? new AccountProfileModel();
                return _profile;
            }
        }

        public LoginResult(NegotiatedConnection connection, Guid accountId, string username, string passwordHash, string displayName, string profileJson)
        {
            AccountId = accountId;
            Connection = connection;
            DisplayName = displayName;
            ProfileJson = profileJson;
            Username = username;
            PasswordHash = passwordHash;
        }
    }
}
