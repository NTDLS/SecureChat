using SecureChat.Library;
using System.IO;

namespace SecureChat.Client
{
    /// <summary>
    /// Contains information about a file that is being sent to the server.
    /// </summary>
    internal class FileOutboundTransfer : IDisposable
    {
        public Guid FileId { get; private set; } = Guid.NewGuid();
        public long FileSize { get; private set; }
        public string FileName { get; private set; }
        public Stream Stream { get; private set; }

        public bool IsImage { get; private set; }

        //TODO: Use the timestamp to cleanup abandoned file transfers.
        public DateTime BeginTimestamp { get; private set; } = DateTime.UtcNow;

        public FileOutboundTransfer(string fileName, long fileSize, Stream stream)
        {
            IsImage = ScConstants.ImageFileTypes.Contains(Path.GetExtension(fileName), StringComparer.InvariantCultureIgnoreCase);
            FileName = fileName;
            FileSize = fileSize;
            Stream = stream;
        }

        public byte[] GetFileBytes()
        {
            if (Stream is MemoryStream memoryStream)
            {
                return memoryStream.ToArray();
            }
            if (Stream is FileStream)
            {
                return File.ReadAllBytes(FileName);
            }
            throw new Exception("Stream type not supported for full materialization.");
        }

        public void Dispose()
        {
            Stream.Dispose();
        }
    }
}
