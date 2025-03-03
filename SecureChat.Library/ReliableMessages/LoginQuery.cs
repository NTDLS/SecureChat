using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class LoginQuery
        : IRmQuery<LoginQueryReply>
    {
        public bool ExplicitAway { get; set; }
        public string PasswordHash { get; set; }
        public string Username { get; set; }

        public LoginQuery(string username, string passwordHash, bool explicitAway)
        {
            Username = username;
            PasswordHash = passwordHash;
            ExplicitAway = explicitAway;
        }
    }

    public class LoginQueryReply
        : IRmQueryReply
    {
        public bool IsSuccess { get; set; }
        public Guid AccountId { get; set; }
        public string DisplayName { get; set; }
        public string ProfileJson { get; set; }
        public string Username { get; set; }
        public string? ErrorMessage { get; set; }

        public LoginQueryReply(Exception exception)
        {
            AccountId = Guid.Empty;
            DisplayName = string.Empty;
            ErrorMessage = exception.GetBaseException().Message;
            IsSuccess = false;
            ProfileJson = string.Empty;
            Username = string.Empty;
        }

        public LoginQueryReply(Guid accountId, string username, string displayName, string profileJson)
        {
            AccountId = accountId;
            DisplayName = displayName;
            IsSuccess = true;
            ProfileJson = profileJson;
            Username = username;
        }

        public LoginQueryReply()
        {
            IsSuccess = false;
            AccountId = Guid.Empty;
            DisplayName = string.Empty;
            ProfileJson = string.Empty;
            Username = string.Empty;
        }
    }
}
