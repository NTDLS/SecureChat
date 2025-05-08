using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class FileTransferChunkQuery
        : IRmQuery<FileTransferChunkQueryReply>
    {
        public Guid FileId { get; set; }

        /// <summary>
        /// The connection id of the remote peer that this message is being sent to.
        /// </summary>
        public Guid PeerConnectionId { get; set; }
        public byte[] Bytes { get; set; }

        /// <summary>
        /// Identifies this chat session. This is used to identify the chat session when sending messages.
        /// If the session is ended and a new one is started, it will have a different SessionId - even if it is the same contact.
        /// </summary>
        public Guid SessionId { get; set; }

        public FileTransferChunkQuery(Guid sessionId, Guid peerConnectionId, Guid fileId, byte[] bytes)
        {
            SessionId = sessionId;
            PeerConnectionId = peerConnectionId;
            FileId = fileId;
            Bytes = bytes;
        }
    }

    public class FileTransferChunkQueryReply
    : IRmQueryReply
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public FileTransferChunkQueryReply(Exception exception)
        {
            IsSuccess = false;
            ErrorMessage = exception.GetBaseException().Message;
        }

        public FileTransferChunkQueryReply()
        {
            IsSuccess = true;
        }
    }
}
