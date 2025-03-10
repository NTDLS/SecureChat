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
        private readonly DatagramMessenger _dmServer;
        private readonly IConfiguration _configuration;
        private readonly DatabaseRepository _dbRepository;
        private readonly Dictionary<Guid, AccountSession> _sessions = new();
        public delegate void OnLogEvent(ChatService server, ScErrorLevel errorLevel, string message, Exception? ex = null);

        public RmServer RmServer { get => _rmServer; }
        public DatagramMessenger DmServer { get => _dmServer; }
        public event OnLogEvent? OnLog;

        public ChatService(IConfiguration configuration)
        {
            _configuration = configuration;

            _rmServer = new RmServer();
            _rmServer.OnException += (RmContext? context, Exception ex, IRmPayload? payload) =>
                OnLog?.Invoke(this, ScErrorLevel.Error, "Reliable messaging exception.", ex);
            _rmServer.OnDisconnected += RmServer_OnDisconnected;
            _rmServer.AddHandler(new ServerReliableMessageHandlers(configuration, this));

            _dmServer = new DatagramMessenger();
            _dmServer.AddHandler(new DatagramMessageHandlers());

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

        public void RegisterSession(Guid connectionId, ServerClientCryptographyProvider baselineCryptographyProvider)
        {
            _sessions.Add(connectionId, new AccountSession(connectionId, baselineCryptographyProvider));
        }

        public void DeregisterSession(Guid connectionId)
        {
            _sessions.Remove(connectionId);
        }

        public AccountSession? GetSessionByConnectionId(Guid connectionId)
        {
            _sessions.TryGetValue(connectionId, out var session);
            return session;
        }

        public AccountSession? GetSessionByAccountId(Guid accountId)
        {
            foreach (var session in _sessions)
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
