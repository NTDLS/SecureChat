using NTDLS.DatagramMessaging;

namespace SecureChat.Library.DatagramMessages
{
    public class VoicePacketMessage : IDmDatagram
    {
        /// <summary>
        /// The connection id of the remote peer that this message is being sent to.
        /// </summary>
        public Guid PeerConnectionId { get; set; }

        /// <summary>
        /// Identifies a unique session. If a session is ended and a new one is started, it will have a different PeerToPeerId.
        /// </summary>
        public Guid PeerToPeerId { get; set; }
        public byte[] Bytes { get; set; }

        public VoicePacketMessage(Guid peerToPeerId, Guid peerConnectionId, byte[] bytes)
        {
            PeerToPeerId = peerToPeerId;
            PeerConnectionId = peerConnectionId;
            Bytes = bytes;
        }
    }
}
