using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class FileTransmissionEndQuery
        : IRmQuery<FileTransmissionEndQueryReply>
    {
        public Guid FileId { get; set; }
        public Guid ConnectionId { get; set; }
        public Guid PeerToPeerId { get; set; }

        public FileTransmissionEndQuery(Guid peerToPeerId, Guid connectionId, Guid fileId)
        {
            PeerToPeerId = peerToPeerId;
            ConnectionId = connectionId;
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
