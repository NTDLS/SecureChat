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

        public ActiveChat VerifyAndActiveChat(DmContext context, Guid peerToPeerId)
        {
            if (ServerConnection.Current == null)
                throw new Exception("Local connection is not established.");

            if (context.GetCryptographyProvider() == null)
                throw new Exception("Cryptography has not been initialized.");

            var activeChat = ServerConnection.Current.GetActiveChat(peerToPeerId)
                ?? throw new Exception("Chat session was not found.");

            return activeChat;
        }

        public void VoicePacketMessage(DmContext context, VoicePacketMessage datagram)
        {
            if (ServerConnection.Current == null)
                throw new Exception("Local connection is not established.");

            var activeChat = ServerConnection.Current.GetActiveChat(datagram.PeerToPeerId)
                ?? throw new Exception("Chat session was not found.");

            activeChat.PlayAudioPacket(datagram.Bytes);
        }

        /// <summary>
        /// We have received a reply to our UDP hello packet, this means that NAT transversal is complete.
        ///     So lets set the datagram messaging context to use the private key-pair from the reliable
        ///     connection and let the server know via reliable messaging that we have done so,
        ///     so that it can also set the public-private-key context for its UPD datagram messaging.
        /// </summary>
        public void HelloReplyMessage(DmContext context, HelloReplyMessage datagram)
        {
            Console.WriteLine($"Reply received from: {context.Endpoint}, Peer: {datagram.PeerConnectionId} (crypto init'd)");
        }
    }
}
