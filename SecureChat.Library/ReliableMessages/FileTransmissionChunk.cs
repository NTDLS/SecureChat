using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class FileTransmissionChunk
        : IRmNotification
    {
        public Guid FileId { get; set; }
        public Guid ConnectionId { get; set; }
        public Guid AccountId { get; set; }
        public byte[] Bytes { get; set; }

        public FileTransmissionChunk(Guid connectionId, Guid accountId, Guid fileId, byte[] bytes)
        {
            AccountId = accountId;
            ConnectionId = connectionId;
            FileId = fileId;
            Bytes = bytes;
        }
    }
}
