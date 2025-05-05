using Microsoft.Extensions.Configuration;
using NTDLS.DatagramMessaging;
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

        Dictionary<Guid, IPEndPoint> _ipEndPoints = new();

        public void VoicePacketMessage(DmContext context, VoicePacketMessage datagram)
        {
            if (_ipEndPoints.TryGetValue(datagram.PeerConnectionId, out var endpoint))
            {
                Console.WriteLine($"Received {datagram.Bytes.Length} bytes from {endpoint.Address}:{endpoint.Port}.");
            }
            else
            {
                _ipEndPoints.Add(datagram.PeerConnectionId, new IPEndPoint(IPAddress.Parse(datagram.PeerToPeerId.ToString()), 0));
            }

            //context.Endpoint

            //_chatService.DmServer.Client.Dispatch(

            //context.WriteReplyBytes(bytes.Bytes); //Echo the payload back to the sender.
            Console.WriteLine($"Received {datagram.Bytes.Length} bytes from {datagram.PeerConnectionId}.");
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
