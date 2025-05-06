using NTDLS.DatagramMessaging;
using SecureChat.Library;

namespace SecureChat.Server
{
    /// <summary>
    /// Used to store information about connected client.
    /// </summary>
    internal class AccountSession
    {
        public Guid ConnectionId { get; private set; }
        public Guid PeerConnectionId { get; private set; }

        public Guid? AccountId { get; private set; } = null;
        public ReliableCryptographyProvider ServerClientCryptographyProvider { get; set; }

        public DmContext? DmContext { get; private set; }

        public void SetDmContext(DmContext context)
        {
            DmContext = context;
        }

        public AccountSession(Guid connectionId, Guid peerConnectionId, ReliableCryptographyProvider serverClientCryptographyProvider)
        {
            ConnectionId = connectionId;
            PeerConnectionId = peerConnectionId;
            ServerClientCryptographyProvider = serverClientCryptographyProvider;
        }

        public void SetAccountId(Guid accountId)
        {
            AccountId = accountId;
        }
    }
}
