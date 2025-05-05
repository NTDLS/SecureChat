using NTDLS.DatagramMessaging;

namespace SecureChat.Library.DatagramMessages
{
    public class VoicePacketMessage : IDmDatagram
    {
        public Guid ConnectionId { get; set; }
        public Guid PeerToPeerId { get; set; }
        public byte[] Bytes { get; set; }

        public VoicePacketMessage(Guid peerToPeerId, Guid connectionId, byte[] bytes)
        {
            PeerToPeerId = peerToPeerId;
            ConnectionId = connectionId;
            Bytes = bytes;
        }
    }
}
