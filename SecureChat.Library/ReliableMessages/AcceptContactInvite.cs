using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class AcceptContactInvite
        : IRmQuery<AcceptContactInviteReply>
    {
        public Guid AccountId { get; set; }

        public AcceptContactInvite(Guid accountId)
        {
            AccountId = accountId;
        }
    }

    public class AcceptContactInviteReply
        : IRmQueryReply
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public AcceptContactInviteReply(Exception exception)
        {
            ErrorMessage = exception.GetBaseException().Message;
            IsSuccess = false;
        }

        public AcceptContactInviteReply()
        {
            IsSuccess = true;
        }
    }
}
