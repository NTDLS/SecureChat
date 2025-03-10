using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class AcceptVoiceCallNotification
        : IRmNotification
    {
        public Guid ConnectionId { get; set; }
        public Guid PeerToPeerId { get; set; }

        public AcceptVoiceCallNotification(Guid peerToPeerId, Guid connectionId)
        {
            PeerToPeerId = peerToPeerId;
            ConnectionId = connectionId;
        }
    }
}
