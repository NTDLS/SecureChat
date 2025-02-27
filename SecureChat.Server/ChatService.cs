using NTDLS.ReliableMessaging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SecureChat.Library.Constants;

namespace SecureChat.Server
{
    internal class ChatService
    {
        private readonly RmServer _rmServer;

        public delegate void OnLogEvent(ChatService server, ScErrorLevel errorLevel, string message, Exception? ex = null);

        /// <summary>
        /// Event used to notify of server exceptions.
        /// </summary>
        public event OnLogEvent? OnLog;

        public ChatService()
        {
            _rmServer = new RmServer();
            _rmServer.OnException += _rmServer_OnException;
            //_rmServer.OnDisconnected += RmServer_OnDisconnected;

            _rmServer.AddHandler(new QueryHandlers(this));
        }

        internal void InvokeOnLog(ScErrorLevel errorLevel, string message)
            => OnLog?.Invoke(this, errorLevel, message);

        private void _rmServer_OnException(RmContext? context, Exception ex, IRmPayload? payload)
        {
            OnLog?.Invoke(this, ScErrorLevel.Error, "Reliable messaging exception.", ex);
        }

    }
}
