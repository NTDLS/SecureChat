using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class TerminateChat
        : IRmNotification
    {
        /// <summary>
        /// ConnectionId of the remote client to terminate the chat with.
        /// </summary>
        public Guid ConnectionId { get; set; }

        /// <summary>
        /// The AccountId of the local client.
        /// </summary>
        public Guid AccountId { get; set; }

        public TerminateChat(Guid connectionId, Guid accountId)
        {
            ConnectionId = connectionId;
            AccountId = accountId;
        }
    }
}
