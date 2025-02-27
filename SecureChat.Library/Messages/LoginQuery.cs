using NTDLS.ReliableMessaging;

namespace SecureChat.Library.Messages
{
    public class LoginQuery
        : IRmQuery<LoginQueryReply>
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public LoginQuery(string username, string password)
        {
            Username = username;
            PasswordHash = password;
        }

    }

    public class LoginQueryReply
        : IRmQueryReply
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public LoginQueryReply(Exception exception)
        {
            IsSuccess = false;
            ErrorMessage = exception.GetBaseException().Message;
        }

        public LoginQueryReply(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public LoginQueryReply()
        {
        }
    }
}
