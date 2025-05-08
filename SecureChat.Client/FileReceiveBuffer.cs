namespace SecureChat.Client
{
    internal class FileReceiveBuffer : IDisposable
    {
        public Guid FileId { get; private set; }
        public long FileSize { get; private set; }
        public string FileName { get; private set; }
        public bool IsImage { get; set; }

        //TODO: Use the timestamp to cleanup abandoned file transfers.
        public DateTime BeginTimestamp { get; private set; } = DateTime.UtcNow;

        private readonly MemoryStream _memoryStream = new();

        public FileReceiveBuffer(Guid fileId, string fileName, long fileSize, bool isImage)
        {
            FileId = fileId;
            FileName = fileName;
            FileSize = fileSize;
            IsImage = isImage;
        }

        public void AppendData(byte[] data)
        {
            _memoryStream.Write(data, 0, data.Length);
        }

        public byte[] GetFileBytes()
        {
            if (_memoryStream.Length != FileSize)
            {
                throw new InvalidOperationException("File corruption detected.");
            }
            return _memoryStream.ToArray();
        }

        public void Dispose()
        {
            _memoryStream.Dispose();
        }
    }
}
