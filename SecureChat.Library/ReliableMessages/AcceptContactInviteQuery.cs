using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class AcceptContactInviteQuery
        : IRmQuery<AcceptContactInviteQueryReply>
    {
        public Guid AccountId { get; set; }

        public AcceptContactInviteQuery(Guid accountId)
        {
            AccountId = accountId;
        }
    }

    public class AcceptContactInviteQueryReply
        : IRmQueryReply
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public AcceptContactInviteQueryReply(Exception exception)
        {
            ErrorMessage = exception.GetBaseException().Message;
            IsSuccess = false;
        }

        public AcceptContactInviteQueryReply()
        {
            IsSuccess = true;
        }
    }
}
