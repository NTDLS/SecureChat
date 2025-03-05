using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class FileTransmissionChunk
        : IRmNotification
    {
        public Guid FileId { get; set; }
        public Guid ConnectionId { get; set; }
        public byte[] Bytes { get; set; }
        public Guid PeerToPeerId { get; set; }

        public FileTransmissionChunk(Guid peerToPeerId, Guid connectionId, Guid fileId, byte[] bytes)
        {
            PeerToPeerId = peerToPeerId;
            ConnectionId = connectionId;
            FileId = fileId;
            Bytes = bytes;
        }
    }
}
