using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class DatagramStreamReadyNotification
        : IRmNotification
    {
        public Guid ConnectionId { get; set; }
        public Guid PeerToPeerId { get; set; }

        public DatagramStreamReadyNotification(Guid peerToPeerId, Guid connectionId)
        {
            PeerToPeerId = peerToPeerId;
            ConnectionId = connectionId;
        }
    }
}
