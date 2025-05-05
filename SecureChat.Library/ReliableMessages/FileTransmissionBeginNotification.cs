using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class FileTransmissionBeginNotification
        : IRmNotification
    {
        public Guid FileId { get; set; }
        public int FileSize { get; set; }

        /// <summary>
        /// The connection id of the remote peer that this message is being sent to.
        /// </summary>
        public Guid PeerConnectionId { get; set; }

        public string FileName { get; set; }
        /// <summary>
        /// Identifies a unique session. If a session is ended and a new one is started, it will have a different PeerToPeerId.
        /// </summary>
        public Guid PeerToPeerId { get; set; }

        public FileTransmissionBeginNotification(Guid peerToPeerId, Guid peerConnectionId, Guid fileId, string fileName, int fileSize)
        {
            PeerToPeerId = peerToPeerId;
            PeerConnectionId = peerConnectionId;
            FileId = fileId;
            FileSize = fileSize;
            FileName = fileName;
        }
    }
}