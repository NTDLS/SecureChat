using NTDLS.DatagramMessaging;

namespace SecureChat.Library.DatagramMessages
{
    /// <summary>
    /// UDP packet sent from the server to the client in response to a HelloPacketMessage.
    /// </summary>
    public class HelloReplyMessage : IDmDatagram
    {
        /// <summary>
        /// The connection id of the remote peer that this message is being sent to.
        /// </summary>
        public Guid PeerConnectionId { get; set; }

        public HelloReplyMessage(Guid peerConnectionId)
        {
            PeerConnectionId = peerConnectionId;
        }
    }
}
