using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class DatagramStreamReadyNotification
        : IRmNotification
    {
        /// <summary>
        /// The connection id of the remote peer that this message is being sent to.
        /// </summary>
        public Guid PeerConnectionId { get; set; }

        public DatagramStreamReadyNotification(Guid peerConnectionId)
        {
            PeerConnectionId = peerConnectionId;
        }
    }
}
