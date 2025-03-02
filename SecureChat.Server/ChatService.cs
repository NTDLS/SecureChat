using Microsoft.Extensions.Configuration;
using NTDLS.ReliableMessaging;
using NTDLS.SqliteDapperWrapper;
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
        private readonly IConfiguration _configuration;
        private readonly ManagedDataStorageFactory _dbFactory;
        private readonly Dictionary<Guid, AccountSession> _sessions = new();
        public delegate void OnLogEvent(ChatService server, ScErrorLevel errorLevel, string message, Exception? ex = null);
        public RmServer RmServer { get => _rmServer; }
        public event OnLogEvent? OnLog;

        public ChatService(IConfiguration configuration)
        {
            _configuration = configuration;
            _rmServer = new RmServer();

            _rmServer.OnException += (RmContext? context, Exception ex, IRmPayload? payload) =>
                OnLog?.Invoke(this, ScErrorLevel.Error, "Reliable messaging exception.", ex);

            _rmServer.OnDisconnected += RmServer_OnDisconnected;

            _rmServer.AddHandler(new ServerReliableMessageHandlers(configuration, this));

            var sqliteConnection = _configuration.GetValue<string>("SQLiteConnection");
            _dbFactory = new ManagedDataStorageFactory($"Data Source={sqliteConnection}");
        }

        internal void InvokeOnLog(ScErrorLevel errorLevel, string message)
            => OnLog?.Invoke(this, errorLevel, message);

        private void RmServer_OnDisconnected(RmContext context)
        {
            var session = GetSessionByConnectionId(context.ConnectionId);

            DeregisterSession(context.ConnectionId);

            if (session != null)
            {
                _dbFactory.Execute(@"SqlQueries\UpdateAccountState.sql", new
                {
                    AccountId = session.AccountId,
                    State = ScConstants.ScOnlineState.Offline.ToString()
                });
            }
        }

        public void Start()
        {
            var listenPort = _configuration.GetValue<int>("ListenPort");

            Log.Verbose($"Starting service on port: {listenPort:n0}.");
            _rmServer.Start(listenPort);
            Log.Verbose("Message started.");
        }

        public void Stop()
        {
            Log.Verbose("Stopping service.");
            _rmServer.Stop();
            Log.Verbose("Message stopped.");
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
