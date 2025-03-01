using SecureChat.Client.Forms;

namespace SecureChat.Client
{
    internal class ActiveChat
    {
        public byte[] SharedSecret { get; set; }
        public FormMessage? Form { get; set; }
        public Guid AccountId { get; set; }
        public string DisplayName { get; set; }
        public Guid ConnectionId { get; set; }

        public ActiveChat(Guid connectionId, Guid accountId, string displayName, byte[] sharedSecret)
        {
            ConnectionId = connectionId;
            AccountId = accountId;
            DisplayName = displayName;
            SharedSecret = sharedSecret;
        }
    }
}
