using System.Text.Json;

namespace Talkster.Library.Models
{
    public class AccountModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public DateTime? LastSeen { get; set; }
        public string State { get; set; } = "Offline";
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
    }
}
