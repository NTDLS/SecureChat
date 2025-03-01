using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class ExchangePeerToPeerMessage
        : IRmNotification
    {
        public string Message { get; set; }
        public string DisplayName { get; set; }
        public Guid ConnectionId { get; set; }
        public Guid AccountId { get; set; }

        public ExchangePeerToPeerMessage(Guid connectionId, Guid accountId, string displayName, string message)
        {
            ConnectionId = connectionId;
            AccountId = accountId;
            DisplayName = displayName;
            Message = message;
        }
    }
}
