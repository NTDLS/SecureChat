using Microsoft.Extensions.Configuration;
using NTDLS.DatagramMessaging;
using NTDLS.ReliableMessaging;
using SecureChat.Library;
using Serilog;
using static SecureChat.Library.ScConstants;

namespace SecureChat.Server
{
    /// <summary>
    /// The main server class.
    /// </summary>
    internal class ChatService
    {
        private readonly RmServer _rmServer;
        private readonly DmServer _dmServer;
        private readonly IConfiguration _configuration;
        private readonly DatabaseRepository _dbRepository;
        private readonly Dictionary<Guid, AccountConnection> _accountConnections = new();
        public delegate void OnLogEvent(ChatService server, ScErrorLevel errorLevel, string message, Exception? ex = null);

        public RmServer RmServer { get => _rmServer; }
        public DmServer DmServer { get => _dmServer; }
        public event OnLogEvent? OnLog;

        public ChatService(IConfiguration configuration)
        {
            _configuration = configuration;

            _rmServer = new RmServer();
            _rmServer.OnException += (RmContext? context, Exception ex, IRmPayload? payload) =>
                OnLog?.Invoke(this, ScErrorLevel.Error, "Reliable messaging exception.", ex);
            _rmServer.OnDisconnected += RmServer_OnDisconnected;
            _rmServer.AddHandler(new ServerReliableMessageHandlers(configuration, this));

            _dmServer = new DmServer();
            _dmServer.AddHandler(new ServerDatagramMessageHandlers(configuration, this));

            _dmServer.OnException += (DmContext? context, Exception ex) =>
            {
                Console.WriteLine($"OnException: {ex.Message}");
            };

            _dbRepository = new DatabaseRepository(configuration);
        }

        internal void InvokeOnLog(ScErrorLevel errorLevel, string message)
            => OnLog?.Invoke(this, errorLevel, message);

        private void RmServer_OnDisconnected(RmContext context)
        {
            var accountConnection = GetAccountConnectionByConnectionId(context.ConnectionId);

            DeregisterSession(context.ConnectionId);

            if (accountConnection != null && accountConnection.AccountId != null)
            {
                _dbRepository.UpdateAccountState(accountConnection.AccountId.Value, ScOnlineState.Offline);
            }
        }

        public void Start()
        {
            var listenPort = _configuration.GetValue<int>("ListenPort");

            Log.Information($"Starting TCP/IP service on port: {listenPort:n0}.");
            _rmServer.Start(listenPort);
            Log.Information($"Starting UDP service on port: {listenPort:n0}.");
            _dmServer.Start(listenPort);
            Log.Information("Service started successfully.");
        }

        public void Stop()
        {
            Log.Information("Stopping service.");
            _rmServer.Stop();
            _dmServer.Stop();
            Log.Information("Message stopped successfully.");
        }

        public void RegisterSession(Guid connectionId, Guid peerConnectionId, ReliableCryptographyProvider baselineCryptographyProvider)
        {
            var accountConnection = new AccountConnection(connectionId, peerConnectionId, baselineCryptographyProvider);

            _accountConnections.Add(connectionId, accountConnection);
        }

        public void DeregisterSession(Guid connectionId)
        {
            _accountConnections.Remove(connectionId);
        }

        /// <summary>
        /// Reverse lookup by the ConnectionId of the ReliableMessaging at the remote peer.
        /// We know the remote ConnectionId because it is supplied in via ExchangePublicKeyQuery
        ///     when the client makes the initial connect to the server.
        /// </summary>
        public AccountConnection? GetAccountConnectionByPeerConnectionId(Guid peerConnectionId)
        {
            return _accountConnections.SingleOrDefault(x => x.Value.PeerConnectionId == peerConnectionId).Value;
        }

        /// <summary>
        /// Forward lookup by the local ReliableMessaging ConnectionId.
        /// </summary>
        public AccountConnection? GetAccountConnectionByConnectionId(Guid connectionId)
        {
            if (_accountConnections.TryGetValue(connectionId, out var accountConnection))
            {
                accountConnection.LastActivityUTC = DateTime.UtcNow;
            }
            return accountConnection;
        }

        public AccountConnection? GetAccountConnectionByAccountId(Guid accountId)
        {
            foreach (var accountConnection in _accountConnections)
            {
                if (accountConnection.Value.AccountId == accountId)
                {
                    return accountConnection.Value;
                }
            }
            return null;
        }
    }
}
