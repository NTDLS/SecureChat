using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    /// <summary>
    /// Used to request the start of a file transmission.
    /// This is only used for large or binary files where we need to give the remote client a chance to select a save location.
    /// </summary>
    public class FileTransmissionBeginRequestNotification
        : IRmNotification
    {
        public Guid FileId { get; set; }
        public long FileSize { get; set; }

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

        public FileTransmissionBeginRequestNotification(Guid sessionId, Guid peerConnectionId, Guid fileId, string fileName, long fileSize)
        {
            SessionId = sessionId;
            PeerConnectionId = peerConnectionId;
            FileId = fileId;
            FileSize = fileSize;
            FileName = fileName;
        }
    }
}
