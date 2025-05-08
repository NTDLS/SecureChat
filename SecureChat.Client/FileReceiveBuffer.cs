using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SecureChat.Client
{
    internal class FileReceiveBuffer : IDisposable
    {
        public Guid FileId { get; private set; }
        public long FileSize { get; private set; }

        /// <summary>
        /// Name of the file as reported by the sender.
        /// </summary>
        public string FileName { get; private set; }
        public bool IsImage { get; set; }

        //TODO: Use the timestamp to cleanup abandoned file transfers.
        public DateTime BeginTimestamp { get; private set; } = DateTime.UtcNow;

        private readonly Stream _stream;

        public FileReceiveBuffer(Guid fileId, string fileName, long fileSize, bool isImage)
        {
            FileId = fileId;
            FileName = fileName;
            FileSize = fileSize;
            IsImage = isImage;

            _stream = new MemoryStream();
        }

        public void AppendData(byte[] data)
        {
            _stream.Write(data, 0, data.Length);
        }

        public byte[] GetFileBytes()
        {
            if (_stream.Length != FileSize)
            {
                throw new Exception("File corruption detected.");
            }

            if (_stream is MemoryStream memoryStream)
            {
                return memoryStream.ToArray();
            }

            throw new Exception("Stream type not supported for full materialization.");
        }

        public void Dispose()
        {
            _stream.Dispose();
        }
    }
}
