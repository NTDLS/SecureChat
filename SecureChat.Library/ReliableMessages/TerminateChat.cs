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
        public Guid PeerToPeerId { get; set; }

        public TerminateChat(Guid connectionId, Guid peerToPeerId)
        {
            ConnectionId = connectionId;
            PeerToPeerId = peerToPeerId;
        }
    }
}
