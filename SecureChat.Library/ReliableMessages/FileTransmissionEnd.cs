using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class FileTransmissionEnd
        : IRmQuery<FileTransmissionEndReply>
    {
        public Guid FileId { get; set; }
        public Guid ConnectionId { get; set; }
        public Guid AccountId { get; set; }

        public FileTransmissionEnd(Guid connectionId, Guid accountId, Guid fileId)
        {
            AccountId = accountId;
            ConnectionId = connectionId;
            FileId = fileId;
        }
    }

    public class FileTransmissionEndReply
    : IRmQueryReply
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public FileTransmissionEndReply(Exception exception)
        {
            IsSuccess = false;
            ErrorMessage = exception.GetBaseException().Message;
        }

        public FileTransmissionEndReply()
        {
            IsSuccess = true;
        }
    }
}
