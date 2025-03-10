using NTDLS.DatagramMessaging;

namespace SecureChat.Library.DatagramMessages
{
    public class InitiateNetworkAddressTranslationMessage : IDmNotification
    {
        public Guid ConnectionId { get; set; }
        public Guid PeerToPeerId { get; set; }

        public InitiateNetworkAddressTranslationMessage(Guid peerToPeerId, Guid connectionId)
        {
            PeerToPeerId = peerToPeerId;
            ConnectionId = connectionId;
        }
    }
}
