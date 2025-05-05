using NTDLS.DatagramMessaging;

namespace SecureChat.Library.DatagramMessages
{
    public class InitiateNetworkAddressTranslationMessage : IDmDatagram
    {
        /// <summary>
        /// The connection id of the remote peer that this message is being sent to.
        /// </summary>
        public Guid PeerConnectionId { get; set; }

        /// <summary>
        /// Identifies a unique session. If a session is ended and a new one is started, it will have a different PeerToPeerId.
        /// </summary>
        public Guid PeerToPeerId { get; set; }

        public InitiateNetworkAddressTranslationMessage(Guid peerToPeerId, Guid peerConnectionId)
        {
            PeerToPeerId = peerToPeerId;
            PeerConnectionId = peerConnectionId;
        }
    }
}
