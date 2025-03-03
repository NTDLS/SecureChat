using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class FileTransmissionBegin
        : IRmNotification
    {
        public Guid FileId { get; set; }
        public int FileSize { get; set; }
        public Guid ConnectionId { get; set; }
        public Guid AccountId { get; set; }
        public string FileName { get; set; }

        public FileTransmissionBegin(Guid connectionId, Guid accountId, Guid fileId, string fileName, int fileSize)
        {
            AccountId = accountId;
            ConnectionId = connectionId;
            FileId = fileId;
            FileSize = fileSize;
            FileName = fileName;
        }
    }
}
