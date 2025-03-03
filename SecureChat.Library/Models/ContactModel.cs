using System.Text.Json;

namespace SecureChat.Library.Models
{
    public class ContactModel
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        /// <summary>
        /// Online, Offline or Away
        /// </summary>
        public string State { get; set; } = "Offline";

        /// <summary>
        /// Json containing the user's profile information deserialize into an AccountProfile object.
        /// </summary>
        public string ProfileJson { get; set; } = string.Empty;
        public bool IsAccepted { get; set; }
        public bool RequestedByMe { get; set; }
        public DateTime? LastSeen { get; set; }

        private AccountProfile? _profile = null;
        public AccountProfile Profile
        {
            get
            {
                _profile ??= JsonSerializer.Deserialize<AccountProfile>(ProfileJson) ?? new AccountProfile();
                return _profile;
            }
        }

    }
}
