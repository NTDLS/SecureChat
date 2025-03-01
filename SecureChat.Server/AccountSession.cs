using SecureChat.Library;

namespace SecureChat.Server
{
    /// <summary>
    /// Used to store information about connected client.
    /// </summary>
    internal class AccountSession
    {
        public Guid ConnectionId { get; private set; }
        public int? AccountId { get; private set; } = null;
        public ServerClientCryptographyProvider ServerClientCryptographyProvider { get; set; }

        public AccountSession(Guid connectionId, ServerClientCryptographyProvider serverClientCryptographyProvider)
        {
            ConnectionId = connectionId;
            ServerClientCryptographyProvider = serverClientCryptographyProvider;
        }

        public void SetAccountId(int accountId)
        {
            AccountId = accountId;
        }
    }
}
