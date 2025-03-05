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
        /// Same as State but as an enum.
        /// </summary>
        public ScConstants.ScOnlineState OnlineState
        {
            get => Enum.Parse<ScConstants.ScOnlineState>(State);
            set => State = value.ToString();
        }

        /// <summary>
        /// Json containing the user's profile information deserialize into an AccountProfile object.
        /// </summary>
        public string? ProfileJson { get; set; }
        public bool IsAccepted { get; set; }
        public bool RequestedByMe { get; set; }
        public DateTime? LastSeen { get; set; }

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
