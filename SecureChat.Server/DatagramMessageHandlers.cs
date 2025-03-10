using Microsoft.Extensions.Configuration;
using NTDLS.DatagramMessaging;
using SecureChat.Library;
using SecureChat.Library.DatagramMessages;

namespace SecureChat.Server
{
    internal class DatagramMessageHandlers : IDmMessageHandler
    {
        private readonly ChatService _chatService;
        private readonly IConfiguration _configuration;

        public DatagramMessageHandlers(IConfiguration configuration, ChatService chatService)
        {
            _configuration = configuration;
            _chatService = chatService;
        }

        public void ProcessFrameNotificationCallback(DmContext context, DmNotificationBytes bytes)
        {
            //context.WriteReplyBytes(bytes.Bytes); //Echo the payload back to the sender.
            //Console.WriteLine($"Received {bytes.Bytes.Length} bytes.");
        }

        public void InitiateNetworkAddressTranslationMessage(DmContext context, InitiateNetworkAddressTranslationMessage payload)
        {
            //Sanity check, not necessary.
            var session = _chatService.GetSessionByConnectionId(payload.ConnectionId)
                ?? throw new Exception("Session not found.");

            //Find the reliable messaging connection for the connection in the payload.
            var rmContext = _chatService.RmServer.GetContext(payload.ConnectionId)
               ?? throw new Exception("Reliable connection was not found.");

            //Obtain the public and private key-pair from the reliable connection so we can use it for the datagram messaging.
            var rmCryptographyProvider = rmContext.GetCryptographyProvider() as ReliableCryptographyProvider
                ?? throw new Exception("Reliable cryptography has not been initialized.");

            //Tell the datagram messaging context to use the private key-pair from the reliable connection.
            context.SetCryptographyProvider(new DatagramCryptographyProvider(rmCryptographyProvider.PublicPrivateKeyPair));

            //context.WriteReplyMessage(payload); //Echo the payload back to the sender.
            Console.WriteLine($"{payload.ConnectionId}->{payload.PeerToPeerId}");
        }
    }
}
