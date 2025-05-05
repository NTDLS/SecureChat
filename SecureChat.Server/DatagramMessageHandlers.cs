using Microsoft.Extensions.Configuration;
using NTDLS.DatagramMessaging;
using NTDLS.DatagramMessaging.Framing;
using SecureChat.Library;
using SecureChat.Library.DatagramMessages;
using SecureChat.Library.ReliableMessages;
using System.Net;

namespace SecureChat.Server
{
    internal class DatagramMessageHandlers : IDmDatagramHandler
    {
        private readonly ChatService _chatService;
        private readonly IConfiguration _configuration;

        public DatagramMessageHandlers(IConfiguration configuration, ChatService chatService)
        {
            _configuration = configuration;
            _chatService = chatService;
        }

        public void DmBytesDatagram(DmContext context, DmBytesDatagram bytes)
        {
            Console.WriteLine($"Received {bytes.Bytes.Length} bytes.");
        }

        private readonly Dictionary<Guid, HashSet<IPEndPoint>> _ipEndPoints = new();

        public void VoicePacketMessage(DmContext context, VoicePacketMessage datagram)
        {
            if (_ipEndPoints.TryGetValue(datagram.PeerToPeerId, out var endpoints))
            {
                if (context.Endpoint != null)
                {
                    //TODO: probably do not need to do this constantly.
                    endpoints.Add(context.Endpoint);
                }
                //Console.WriteLine($"Received {datagram.Bytes.Length} bytes from {context.Endpoint?.Address}:{context.Endpoint?.Port}.");
            }
            else
            {
                if (context.Endpoint != null)
                {
                    HashSet<IPEndPoint> newEndpoints = new();
                    newEndpoints.Add(context.Endpoint);
                    _ipEndPoints.Add(datagram.PeerToPeerId, newEndpoints);
                }
            }

            if (_chatService.DmServer.Client != null && endpoints != null)
            {
                //Find the other endpoint, assuming it has been set.
                var otherEndpoint = endpoints.SingleOrDefault(o => o != context.Endpoint);
                if (otherEndpoint != null)
                {
                    //Dispatch the UPD datagram to the other endpoint.
                    _chatService.DmServer.Client.Dispatch(context, otherEndpoint, datagram);
                    //Console.WriteLine($"Received {datagram.Bytes.Length} bytes from {datagram.PeerConnectionId}.");
                }
            }

            //context.WriteReplyBytes(bytes.Bytes); //Echo the payload back to the sender.
        }

        public void InitiateNetworkAddressTranslationMessage(DmContext context, InitiateNetworkAddressTranslationMessage datagram)
        {
            var session = _chatService.GetSessionByConnectionId(datagram.PeerConnectionId)
                ?? throw new Exception("Session not found.");

            //Tell the datagram messaging context to use the private key-pair from the reliable connection.
            context.SetCryptographyProvider(new DatagramCryptographyProvider(session.ServerClientCryptographyProvider.PublicPrivateKeyPair));

            //Let the client know that we received the initialization request and have completed it.
            //We have to do this because we cant communicate on the UPD stream until we know encryption is established.
            _chatService.RmServer.Notify(session.ConnectionId, new DatagramStreamReadyNotification(datagram.PeerToPeerId, datagram.PeerConnectionId));

            Console.WriteLine($"{datagram.PeerConnectionId}->{datagram.PeerToPeerId}");
        }
    }
}
