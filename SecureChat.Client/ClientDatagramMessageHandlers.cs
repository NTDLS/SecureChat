using NTDLS.DatagramMessaging;
using SecureChat.Library.DatagramMessages;

namespace SecureChat.Client
{
    internal class ClientDatagramMessageHandlers : IDmDatagramHandler
    {
        public ClientDatagramMessageHandlers()
        {
            //_chatService = chatService;
        }

        public ActiveChat VerifyAndActiveChat(DmContext context, Guid sessionId)
        {
            if (ServerConnection.Current == null)
                throw new Exception("Local connection is not established.");

            if (context.GetCryptographyProvider() == null)
                throw new Exception("Cryptography has not been initialized.");

            var activeChat = ServerConnection.Current.GetActiveChat(sessionId)
                ?? throw new Exception("Chat session was not found.");

            return activeChat;
        }

        public void VoicePacketMessage(DmContext context, VoicePacketMessage datagram)
        {
            if (ServerConnection.Current == null)
                throw new Exception("Local connection is not established.");

            var activeChat = ServerConnection.Current.GetActiveChat(datagram.SessionId)
                ?? throw new Exception("Chat session was not found.");

            activeChat.PlayAudioPacket(datagram.Bytes);
        }

        /// <summary>
        /// We have received a reply to our UDP hello packet, this means that NAT transversal is complete.
        /// This functions as a two-way keepalive.
        /// </summary>
        public void HelloReplyMessage(DmContext context, HelloReplyMessage datagram)
        {
            Console.WriteLine($"Reply received from: {context.Endpoint}, Peer: {datagram.PeerConnectionId}");
        }
    }
}
