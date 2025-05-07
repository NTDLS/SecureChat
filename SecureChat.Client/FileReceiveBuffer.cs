namespace SecureChat.Client
{
    internal class FileReceiveBuffer : IDisposable
    {
        public Guid FileId { get; private set; }
        public long FileSize { get; private set; }
        public string FileName { get; private set; }

        //TODO: Use the timestamp to cleanup abandoned file transfers.
        public DateTime BeginTimestamp { get; private set; } = DateTime.UtcNow;

        private MemoryStream _memoryStream = new();

        public FileReceiveBuffer(Guid fileId, string fileName, long fileSize)
        {
            FileId = fileId;
            FileName = fileName;
            FileSize = fileSize;
        }

        public void AppendData(byte[] data)
        {
            _memoryStream.Write(data, 0, data.Length);
        }

        public byte[] GetFileBytes()
        {
            /*
            int retryCount = 0;
            while (_memoryStream.Length != FileSize && retryCount++ < 100)
            {
                Thread.Sleep(10);
            }
            */

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
