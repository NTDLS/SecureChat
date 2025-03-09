using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class RequestVoiceCallNotification
        : IRmNotification
    {
        public Guid ConnectionId { get; set; }
        public Guid PeerToPeerId { get; set; }

        public RequestVoiceCallNotification(Guid peerToPeerId, Guid connectionId)
        {
            PeerToPeerId = peerToPeerId;
            ConnectionId = connectionId;
        }
    }
}
