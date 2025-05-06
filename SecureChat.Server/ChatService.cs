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
        private readonly Dictionary<Guid, AccountConnection> _forwardLookup = new();
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
            var session = GetSessionByConnectionId(context.ConnectionId);

            DeregisterSession(context.ConnectionId);

            if (session != null && session.AccountId != null)
            {
                _dbRepository.UpdateAccountState(session.AccountId.Value, ScOnlineState.Offline);
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
            var session = new AccountConnection(connectionId, peerConnectionId, baselineCryptographyProvider);

            _forwardLookup.Add(connectionId, session);
        }

        public void DeregisterSession(Guid connectionId)
        {
            _forwardLookup.Remove(connectionId);
        }

        /// <summary>
        /// Gets the session by the ReliableMessaging ConnectionID at the remote peer.
        /// </summary>
        public AccountConnection? GetSessionByPeerConnectionId(Guid peerConnectionId)
        {
            return _forwardLookup.SingleOrDefault(x => x.Value.PeerConnectionId == peerConnectionId).Value;
        }

        /// <summary>
        /// Gets the session by the ReliableMessaging ConnectionID at this server.
        /// </summary>
        public AccountConnection? GetSessionByConnectionId(Guid connectionId)
        {
            _forwardLookup.TryGetValue(connectionId, out var session);
            return session;
        }

        public AccountConnection? GetSessionByAccountId(Guid accountId)
        {
            foreach (var session in _forwardLookup)
            {
                if (session.Value.AccountId == accountId)
                {
                    return session.Value;
                }
            }
            return null;
        }
    }
}
