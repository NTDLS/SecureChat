using SecureChat.Library;
using System.Net;

namespace SecureChat.Server
{
    /// <summary>
    /// Used to store information about connected client.
    /// </summary>
    internal class AccountConnection
    {
        /// <summary>
        /// The connection ID of the client.
        /// </summary>
        public Guid ConnectionId { get; private set; }

        /// <summary>
        /// The remote connection ID of the client.
        /// </summary>
        public Guid PeerConnectionId { get; private set; }

        public Guid? AccountId { get; private set; } = null;
        public ReliableCryptographyProvider ServerClientCryptographyProvider { get; set; }

        /// <summary>
        /// The datagram endpoint of the client.
        /// </summary>
        public IPEndPoint? DmEndpoint { get; private set; }

        public void SetDmEndpoint(IPEndPoint dmEndpoint)
        {
            DmEndpoint = dmEndpoint;
        }

        public AccountConnection(Guid connectionId, Guid peerConnectionId, ReliableCryptographyProvider serverClientCryptographyProvider)
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
