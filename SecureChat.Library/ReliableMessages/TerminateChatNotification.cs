using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class TerminateChatNotification
        : IRmNotification
    {
        /// <summary>
        /// ConnectionId of the remote client to terminate the chat with.
        /// </summary>
        public Guid PeerConnectionId { get; set; }

        /// <summary>
        /// Identifies a unique session. If a session is ended and a new one is started, it will have a different PeerToPeerId.
        /// </summary>
        public Guid PeerToPeerId { get; set; }

        public TerminateChatNotification(Guid peerToPeerId, Guid peerConnectionId)
        {
            PeerConnectionId = peerConnectionId;
            PeerToPeerId = peerToPeerId;
        }
    }
}
