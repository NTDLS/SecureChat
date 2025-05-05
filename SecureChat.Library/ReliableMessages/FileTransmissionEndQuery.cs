using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class FileTransmissionEndQuery
        : IRmQuery<FileTransmissionEndQueryReply>
    {
        public Guid FileId { get; set; }

        /// <summary>
        /// The connection id of the remote peer that this message is being sent to.
        /// </summary>
        public Guid PeerConnectionId { get; set; }

        /// <summary>
        /// Identifies a unique session. If a session is ended and a new one is started, it will have a different PeerToPeerId.
        /// </summary>
        public Guid PeerToPeerId { get; set; }

        public FileTransmissionEndQuery(Guid peerToPeerId, Guid peerConnectionId, Guid fileId)
        {
            PeerToPeerId = peerToPeerId;
            PeerConnectionId = peerConnectionId;
            FileId = fileId;
        }
    }

    public class FileTransmissionEndQueryReply
    : IRmQueryReply
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public FileTransmissionEndQueryReply(Exception exception)
        {
            IsSuccess = false;
            ErrorMessage = exception.GetBaseException().Message;
        }

        public FileTransmissionEndQueryReply()
        {
            IsSuccess = true;
        }
    }
}
