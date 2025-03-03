using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class InviteContactQuery
        : IRmQuery<InviteContactQueryReply>
    {
        public Guid AccountId { get; set; }

        public InviteContactQuery(Guid accountId)
        {
            AccountId = accountId;
        }
    }

    public class InviteContactQueryReply
        : IRmQueryReply
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public InviteContactQueryReply(Exception exception)
        {
            ErrorMessage = exception.GetBaseException().Message;
            IsSuccess = false;
        }

        public InviteContactQueryReply()
        {
            IsSuccess = true;
        }
    }
}
