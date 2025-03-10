using NTDLS.DatagramMessaging;

namespace SecureChat.Server
{
    internal class DatagramMessageHandlers : IDmMessageHandler
    {
        public static void ProcessFrameNotificationCallback(DmContext context, DmNotificationBytes bytes)
        {
            //context.WriteReplyBytes(bytes.Bytes); //Echo the payload back to the sender.
            //Console.WriteLine($"Received {bytes.Bytes.Length} bytes.");
        }

        /*
        public static void ProcessFrameNotificationCallback(DmContext context, MyFirstUDPPacket payload)
        {
            //context.WriteReplyMessage(payload); //Echo the payload back to the sender.
            Console.WriteLine($"{payload.Message}->{payload.UID}->{payload.TimeStamp}");
        }
        */
    }
}
