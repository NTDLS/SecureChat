using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class LoginQuery
        : IRmQuery<LoginQueryReply>
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public LoginQuery(string username, string passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;
        }
    }

    public class LoginQueryReply
        : IRmQueryReply
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public Guid AccountId { get; set; }

        public LoginQueryReply(Exception exception)
        {
            IsSuccess = false;
            ErrorMessage = exception.GetBaseException().Message;
            Username = string.Empty;
            DisplayName = string.Empty;
            AccountId = Guid.Empty;
        }

        public LoginQueryReply(Guid accountId, string username, string displayName)
        {
            AccountId = accountId;
            Username = username;
            DisplayName = displayName;
            IsSuccess = true;
        }

        public LoginQueryReply()
        {
            Username = string.Empty;
            DisplayName = string.Empty;
            AccountId = Guid.Empty;
        }
    }
}
