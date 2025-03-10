using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class CancelVoiceCallRequestNotification
        : IRmNotification
    {
        public Guid ConnectionId { get; set; }
        public Guid PeerToPeerId { get; set; }

        public CancelVoiceCallRequestNotification(Guid peerToPeerId, Guid connectionId)
        {
            PeerToPeerId = peerToPeerId;
            ConnectionId = connectionId;
        }
    }
}
