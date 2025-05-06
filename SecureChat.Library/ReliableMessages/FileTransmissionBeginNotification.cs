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
        /// Identifies this chat session. This is used to identify the chat session when sending messages.
        /// If the session is ended and a new one is started, it will have a different SessionId - even if it is the same contact.
        /// </summary>
        public Guid SessionId { get; set; }

        public FileTransmissionBeginNotification(Guid sessionId, Guid peerConnectionId, Guid fileId, string fileName, int fileSize)
        {
            SessionId = sessionId;
            PeerConnectionId = peerConnectionId;
            FileId = fileId;
            FileSize = fileSize;
            FileName = fileName;
        }
    }
}