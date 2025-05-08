using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class FileTransmissionBeginQuery
        : IRmQuery<FileTransmissionBeginQueryReply>
    {
        public Guid FileId { get; set; }
        public long FileSize { get; set; }

        /// <summary>
        /// The connection id of the remote peer that this message is being sent to.
        /// </summary>
        public Guid PeerConnectionId { get; set; }

        public bool IsImage { get; set; }

        public string FileName { get; set; }

        /// <summary>
        /// Identifies this chat session. This is used to identify the chat session when sending messages.
        /// If the session is ended and a new one is started, it will have a different SessionId - even if it is the same contact.
        /// </summary>
        public Guid SessionId { get; set; }

        public FileTransmissionBeginQuery(Guid sessionId, Guid peerConnectionId, Guid fileId, string fileName, long fileSize, bool isImage)
        {
            SessionId = sessionId;
            PeerConnectionId = peerConnectionId;
            FileId = fileId;
            FileSize = fileSize;
            FileName = fileName;
            IsImage = isImage;
        }
    }

    public class FileTransmissionBeginQueryReply
        : IRmQueryReply
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public FileTransmissionBeginQueryReply(Exception exception)
        {
            IsSuccess = false;
            ErrorMessage = exception.GetBaseException().Message;
        }

        public FileTransmissionBeginQueryReply()
        {
            IsSuccess = true;
        }
    }
}
