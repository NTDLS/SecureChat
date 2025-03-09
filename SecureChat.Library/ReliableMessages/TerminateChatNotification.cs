using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class TerminateChatNotification
        : IRmNotification
    {
        /// <summary>
        /// ConnectionId of the remote client to terminate the chat with.
        /// </summary>
        public Guid ConnectionId { get; set; }
        public Guid PeerToPeerId { get; set; }

        public TerminateChatNotification(Guid connectionId, Guid peerToPeerId)
        {
            ConnectionId = connectionId;
            PeerToPeerId = peerToPeerId;
        }
    }
}
