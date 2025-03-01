using SecureChat.Client.Forms;

namespace SecureChat.Client
{
    internal class ActiveChat
    {
        public byte[] SharedSecret { get; set; }
        public FormMessage? Form { get; set; }
        public Guid AccountId { get; set; }
        public Guid ConnectionId { get; set; }

        public ActiveChat(Guid connectionId, Guid accountId, byte[] sharedSecret)
        {
            ConnectionId = connectionId;
            AccountId = accountId;
            SharedSecret = sharedSecret;
        }
    }
}
