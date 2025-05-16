namespace Talkster.Client
{
    public class OrderedChunk
    {
        public int ChunkNumber { get; private set; }
        public byte[] Bytes { get; private set; }

        public OrderedChunk(int chunkNumber, byte[] bytes)
        {
            ChunkNumber = chunkNumber;
            Bytes = bytes;
        }
    }
}
