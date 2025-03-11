using Microsoft.Extensions.Configuration;
using NTDLS.DatagramMessaging;
using SecureChat.Library;
using SecureChat.Library.DatagramMessages;
using SecureChat.Library.ReliableMessages;

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

        public void ProcessFrameNotificationCallback(DmContext context, DmBytesDatagram bytes)
        {
            //context.WriteReplyBytes(bytes.Bytes); //Echo the payload back to the sender.
            //Console.WriteLine($"Received {bytes.Bytes.Length} bytes.");
        }

        public void InitiateNetworkAddressTranslationMessage(DmContext context, InitiateNetworkAddressTranslationMessage datagram)
        {
            var session = _chatService.GetSessionByConnectionId(datagram.ConnectionId)
                ?? throw new Exception("Session not found.");

            //Tell the datagram messaging context to use the private key-pair from the reliable connection.
            context.SetCryptographyProvider(new DatagramCryptographyProvider(session.ServerClientCryptographyProvider.PublicPrivateKeyPair));

            //Let the client know that we received the initialization request and have completed it.
            //We have to do this because we cant communicate on the UPD stream until we know encryption is established.
            _chatService.RmServer.Notify(session.ConnectionId, new DatagramStreamReadyNotification(datagram.PeerToPeerId, datagram.ConnectionId));

            //context.WriteReplyMessage(payload); //Echo the payload back to the sender.
            Console.WriteLine($"{datagram.ConnectionId}->{datagram.PeerToPeerId}");
        }
    }
}
