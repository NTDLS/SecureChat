using NTDLS.DatagramMessaging;
using SecureChat.Library.DatagramMessages;

namespace SecureChat.Server
{
    internal class DatagramMessageHandlers : IDmMessageHandler
    {
        public static void ProcessFrameNotificationCallback(DmContext context, DmNotificationBytes bytes)
        {
            //context.WriteReplyBytes(bytes.Bytes); //Echo the payload back to the sender.
            //Console.WriteLine($"Received {bytes.Bytes.Length} bytes.");
        }

        public static void InitiateNetworkAddressTranslationMessage(DmContext context, InitiateNetworkAddressTranslationMessage payload)
        {
            //context.WriteReplyMessage(payload); //Echo the payload back to the sender.
            Console.WriteLine($"{payload.ConnectionId}->{payload.PeerToPeerId}");
        }
    }
}
