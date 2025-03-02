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
        public string Status { get; set; }
        public string Username { get; set; }
        public string? ErrorMessage { get; set; }

        public LoginQueryReply(Exception exception)
        {
            AccountId = Guid.Empty;
            DisplayName = string.Empty;
            ErrorMessage = exception.GetBaseException().Message;
            IsSuccess = false;
            Status = string.Empty;
            Username = string.Empty;
        }

        public LoginQueryReply(Guid accountId, string username, string displayName, string status)
        {
            AccountId = accountId;
            DisplayName = displayName;
            IsSuccess = true;
            Status = status;
            Username = username;
        }

        public LoginQueryReply()
        {
            AccountId = Guid.Empty;
            DisplayName = string.Empty;
            Status = string.Empty;
            Username = string.Empty;
        }
    }
}
