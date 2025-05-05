using NTDLS.DatagramMessaging;

namespace SecureChat.Library.DatagramMessages
{
    /// <summary>
    /// UDP packet routinely sent from the client to the server.
    /// </summary>
    public class HelloPacketMessage : IDmDatagram
    {
        /// <summary>
        /// The connection id of the remote peer that this message is being sent to.
        /// </summary>
        public Guid PeerConnectionId { get; set; }

        public HelloPacketMessage(Guid peerConnectionId)
        {
            PeerConnectionId = peerConnectionId;
        }
    }
}
