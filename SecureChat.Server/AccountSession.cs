using SecureChat.Library;

namespace SecureChat.Server
{
    /// <summary>
    /// Used to store information about connected client.
    /// </summary>
    internal class AccountSession
    {
        public Guid ConnectionId { get; private set; }
        public Guid? AccountId { get; private set; } = null;
        public ReliableCryptographyProvider ServerClientCryptographyProvider { get; set; }

        public AccountSession(Guid connectionId, ReliableCryptographyProvider serverClientCryptographyProvider)
        {
            ConnectionId = connectionId;
            ServerClientCryptographyProvider = serverClientCryptographyProvider;
        }

        public void SetAccountId(Guid accountId)
        {
            AccountId = accountId;
        }
    }
}
