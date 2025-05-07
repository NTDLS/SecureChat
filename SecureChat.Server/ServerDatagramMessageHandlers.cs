using Microsoft.Extensions.Configuration;
using NTDLS.DatagramMessaging;
using NTDLS.DatagramMessaging.Framing;
using SecureChat.Library.DatagramMessages;

namespace SecureChat.Server
{
    internal class ServerDatagramMessageHandlers : IDmDatagramHandler
    {
        private readonly ChatService _chatService;
        private readonly IConfiguration _configuration;

        public ServerDatagramMessageHandlers(IConfiguration configuration, ChatService chatService)
        {
            _configuration = configuration;
            _chatService = chatService;
        }

        public void VoicePacketDatagram(DmContext context, VoicePacketDatagram datagram)
        {
            var accountConnection = _chatService.GetAccountConnectionByConnectionId(datagram.PeerConnectionId)
                ?? throw new Exception("Session not found.");

            if (_chatService.DmServer.Client != null && accountConnection.DmEndpoint != null)
            {
                _chatService.DmServer.Client.Dispatch(context, accountConnection.DmEndpoint, datagram);
            }
        }

        /// <summary>
        /// The message is sent by the client after the reliable message connection is established.
        /// NAT should now be established, so reply to the UDP packet so that the client knows we received it.
        /// This serves two purposes:
        /// 1) Allows us to associate the UPD endpoint with a session.
        /// 2) This functions as a two-way keepalive.
        /// </summary>
        public void ConnectionKeepAliveDatagram(DmContext context, ConnectionKeepAliveDatagram datagram)
        {
            var accountConnection = _chatService.GetAccountConnectionByPeerConnectionId(datagram.PeerConnectionId)
                ?? throw new Exception("Session not found.");

            if (context.Endpoint != null && accountConnection.DmEndpoint != context.Endpoint)
            {
                //Save the DmEndpoint for this session.
                accountConnection.SetDmEndpoint(context.Endpoint);
            }

            context.Dispatch(datagram); //Echo the hello packet back to the sender.

            //Console.WriteLine($"UDP keep-alive from: {context.Endpoint}, Peer: {datagram.PeerConnectionId}");
        }
    }
}
