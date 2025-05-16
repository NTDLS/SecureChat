using NTDLS.ReliableMessaging;

namespace Talkster.Library.ReliableMessages
{
    public class CreateAccountQuery
        : IRmQuery<CreateAccountQueryReply>
    {
        public string PasswordHash { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }

        public CreateAccountQuery(string username, string displayName, string passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;
            DisplayName = displayName;
        }
    }

    public class CreateAccountQueryReply
        : IRmQueryReply
    {
        public bool IsSuccess { get; set; }
        public Guid AccountId { get; set; }

        public string? ErrorMessage { get; set; }

        public CreateAccountQueryReply(Exception exception)
        {
            AccountId = Guid.Empty;
            ErrorMessage = exception.GetBaseException().Message;
            IsSuccess = false;
        }

        public CreateAccountQueryReply()
        {
            IsSuccess = true;
        }
    }
}
