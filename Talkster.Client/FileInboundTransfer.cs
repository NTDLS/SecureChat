using NTDLS.Helpers;
using NTDLS.Permafrost;

namespace Talkster.Client
{
    internal class FileInboundTransfer : IDisposable
    {
        public Guid FileId { get; private set; }
        public long FileSize { get; private set; }
        public long ReceivedByteCount { get; private set; }
        public int PercentComplete => (int)((ReceivedByteCount / (double)FileSize) * 100.0);
        public string? SaveAsFileName { get; private set; }

        /// <summary>
        /// Name of the file as reported by the sender.
        /// </summary>
        public string FileName { get; private set; }
        public bool IsImage { get; set; }

        //TODO: Use the timestamp to cleanup abandoned file transfers.
        public DateTime BeginTimestamp { get; private set; } = DateTime.UtcNow;

        private readonly Stream _stream;
        private readonly PermafrostCipher _crypto;
        private readonly List<OrderedChunk> _chunkBuffer = new();
        private int _lastChunkNumber = -1;

        /// <summary>
        /// Buffered file data.
        /// </summary>
        public FileInboundTransfer(byte[] sharedSecret, Guid fileId, string fileName, long fileSize, bool isImage)
        {
            _crypto = new PermafrostCipher(sharedSecret, PermafrostMode.Continuous);

            FileId = fileId;
            FileName = fileName;
            FileSize = fileSize;
            IsImage = isImage;

            _stream = new MemoryStream();
        }

        /// <summary>
        /// Physical file data.
        /// </summary>
        public FileInboundTransfer(byte[] sharedSecret, Guid fileId, string fileName, long fileSize, bool isImage, string saveAsFileName)
        {
            _crypto = new PermafrostCipher(sharedSecret, PermafrostMode.Continuous);

            SaveAsFileName = saveAsFileName;
            FileId = fileId;
            FileName = fileName;
            FileSize = fileSize;
            IsImage = isImage;

            _stream = new FileStream(saveAsFileName, FileMode.Create, FileAccess.Write);
        }

        /// <summary>
        /// Appends the received chunk to the stream.
        /// If the chunk is out of order, it will be buffered until the previous chunk is received.
        /// </summary>
        /// <returns>True when the file is fully received, otherwise false.</returns>
        public bool AppendChunk(byte[] data, int chunkNumber)
        {
            lock (_chunkBuffer)
            {
                if (chunkNumber == _lastChunkNumber + 1)
                {
                    //We can write this chunk immediately because it is the next chunk in order.
                    ReceivedByteCount += data.Length;
                    _stream.Write(_crypto.Cipher(data), 0, data.Length);
                    _lastChunkNumber = chunkNumber;
                }
                else
                {
                    //The chunk is out of order, so we need to buffer it until we receive the previous chunk.
                    _chunkBuffer.Add(new OrderedChunk(chunkNumber, data));
                }

                OrderedChunk? bufferedChunk; //Flush out any buffered chunks that are now in order.
                while ((bufferedChunk = _chunkBuffer.FirstOrDefault(x => x.ChunkNumber == _lastChunkNumber + 1)) != null)
                {
                    ReceivedByteCount += bufferedChunk.Bytes.Length;
                    _stream.Write(_crypto.Cipher(bufferedChunk.Bytes), 0, bufferedChunk.Bytes.Length);
                    _lastChunkNumber = bufferedChunk.ChunkNumber;
                    _chunkBuffer.Remove(bufferedChunk);
                }

                if (ReceivedByteCount == FileSize)
                {
                    return true;
                }
            }

            return false;
        }

        public byte[] GetFileBytes()
        {
            //Since we sent file chunks asynchronously, we need to wait for the stream to be fully received.
            for (int retry = 0; _stream.Length != FileSize && retry < 100; retry++)
            {
                Thread.Sleep(100);
            }

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
            Exceptions.Ignore(() => _stream.Flush());
            Exceptions.Ignore(() => _stream.Close());
            Exceptions.Ignore(() => _stream.Dispose());
            Exceptions.Ignore(() => _crypto.Dispose());
            Exceptions.Ignore(() => _chunkBuffer.Clear());
        }
    }
}
