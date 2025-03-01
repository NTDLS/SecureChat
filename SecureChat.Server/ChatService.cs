using Microsoft.Extensions.Configuration;
using NTDLS.ReliableMessaging;
using SecureChat.Library;
using Serilog;
using static SecureChat.Library.Constants;

namespace SecureChat.Server
{
    internal class ChatService
    {
        private readonly RmServer _rmServer;
        private readonly IConfiguration _configuration;
        private readonly Dictionary<Guid, AccountSession> _sessions = new();
        public delegate void OnLogEvent(ChatService server, ScErrorLevel errorLevel, string message, Exception? ex = null);
        public event OnLogEvent? OnLog;

        public ChatService(IConfiguration configuration)
        {
            _configuration = configuration;
            _rmServer = new RmServer();

            _rmServer.OnException += (RmContext? context, Exception ex, IRmPayload? payload) =>
                OnLog?.Invoke(this, ScErrorLevel.Error, "Reliable messaging exception.", ex);

            _rmServer.OnDisconnected += RmServer_OnDisconnected;

            _rmServer.AddHandler(new ReliableQueryHandlers(configuration, this));
        }

        internal void InvokeOnLog(ScErrorLevel errorLevel, string message)
            => OnLog?.Invoke(this, errorLevel, message);

        private void RmServer_OnDisconnected(RmContext context)
        {
            DeregisterSession(context.ConnectionId);
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

        public void RegisterSession(Guid connectionId, BaselineCryptographyProvider baselineCryptographyProvider)
        {
            _sessions.Add(connectionId, new AccountSession(connectionId, baselineCryptographyProvider));
        }

        public void DeregisterSession(Guid connectionId)
        {
            _sessions.Remove(connectionId);
        }

        public AccountSession? GetSession(Guid connectionId)
        {
            _sessions.TryGetValue(connectionId, out var session);
            return session;
        }
    }
}
