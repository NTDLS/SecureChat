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

        public void DmBytesDatagram(DmContext context, DmBytesDatagram bytes)
        {
            Console.WriteLine($"Received {bytes.Bytes.Length} bytes.");
        }

        public void VoicePacketMessage(DmContext context, VoicePacketMessage datagram)
        {
            var session = _chatService.GetSessionByConnectionId(datagram.PeerConnectionId)
                ?? throw new Exception("Session not found.");

            if (_chatService.DmServer.Client != null && session.DmEndpoint != null)
            {
                _chatService.DmServer.Client.Dispatch(context, session.DmEndpoint, datagram);
            }
        }

        /// <summary>
        /// The hello message is sent by the client after the reliable message connection is established.
        /// NAT should now be established, so reply to the UDP packet so that the client knows we received it.
        /// This serves two purposes:
        /// 1) Allows us to associate the UPD endpoint with a session.
        /// 2) This functions as a two-way keepalive.
        /// </summary>
        public void HelloPacketMessage(DmContext context, HelloPacketMessage datagram)
        {
            var session = _chatService.GetSessionByPeerConnectionId(datagram.PeerConnectionId)
                ?? throw new Exception("Session not found.");

            if (context.Endpoint != null && session.DmEndpoint != context.Endpoint)
            {
                //Save the DmEndpoint for this session.
                session.SetDmEndpoint(context.Endpoint);
            }

            //Echo the hello packet back to the sender.
            context.Dispatch(new HelloReplyMessage(datagram.PeerConnectionId));

            Console.WriteLine($"Hello received from: {context.Endpoint}, Peer: {datagram.PeerConnectionId}");
        }
    }
}
