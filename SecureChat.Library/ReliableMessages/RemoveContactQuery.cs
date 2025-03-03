using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class RemoveContactQuery
        : IRmQuery<RemoveContactQueryReply>
    {
        public Guid AccountId { get; set; }

        public RemoveContactQuery(Guid accountId)
        {
            AccountId = accountId;
        }
    }

    public class RemoveContactQueryReply
        : IRmQueryReply
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public RemoveContactQueryReply(Exception exception)
        {
            ErrorMessage = exception.GetBaseException().Message;
            IsSuccess = false;
        }

        public RemoveContactQueryReply()
        {
            IsSuccess = true;
        }
    }
}
