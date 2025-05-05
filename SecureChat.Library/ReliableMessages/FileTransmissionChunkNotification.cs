using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class FileTransmissionChunkNotification
        : IRmNotification
    {
        public Guid FileId { get; set; }

        /// <summary>
        /// The connection id of the remote peer that this message is being sent to.
        /// </summary>
        public Guid PeerConnectionId { get; set; }
        public byte[] Bytes { get; set; }
        /// <summary>
        /// Identifies a unique session. If a session is ended and a new one is started, it will have a different PeerToPeerId.
        /// </summary>
        public Guid PeerToPeerId { get; set; }

        public FileTransmissionChunkNotification(Guid peerToPeerId, Guid peerConnectionId, Guid fileId, byte[] bytes)
        {
            PeerToPeerId = peerToPeerId;
            PeerConnectionId = peerConnectionId;
            FileId = fileId;
            Bytes = bytes;
        }
    }
}
