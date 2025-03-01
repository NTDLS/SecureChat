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
        public string? Username { get; set; }
        public string? DisplayName { get; set; }

        public LoginQueryReply(Exception exception)
        {
            IsSuccess = false;
            ErrorMessage = exception.GetBaseException().Message;
        }

        public LoginQueryReply(string username, string displayName)
        {
            Username = username;
            DisplayName = displayName;
            IsSuccess = true;
        }

        public LoginQueryReply()
        {
        }
    }
}
