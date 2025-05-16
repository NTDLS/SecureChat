using NTDLS.DatagramMessaging;

namespace Talkster.Library.DatagramMessages
{
    /// <summary>
    /// UDP packet routinely sent from the client to the server.
    /// </summary>
    public class ConnectionKeepAliveDatagram : IDmDatagram
    {
        /// <summary>
        /// The connection id of the remote peer that this message is being sent to.
        /// </summary>
        public Guid PeerConnectionId { get; set; }

        public ConnectionKeepAliveDatagram(Guid peerConnectionId)
        {
            PeerConnectionId = peerConnectionId;
        }
    }
}
