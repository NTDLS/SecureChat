using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class DeclineVoiceCallNotification
        : IRmNotification
    {
        public Guid ConnectionId { get; set; }
        public Guid PeerToPeerId { get; set; }

        public DeclineVoiceCallNotification(Guid peerToPeerId, Guid connectionId)
        {
            PeerToPeerId = peerToPeerId;
            ConnectionId = connectionId;
        }
    }
}
