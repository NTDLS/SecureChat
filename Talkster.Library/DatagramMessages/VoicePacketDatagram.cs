using NTDLS.DatagramMessaging;

namespace Talkster.Library.DatagramMessages
{
    public class VoicePacketDatagram : IDmDatagram
    {
        /// <summary>
        /// The connection id of the remote peer that this message is being sent to.
        /// </summary>
        public Guid PeerConnectionId { get; set; }

        /// <summary>
        /// Identifies this chat session. This is used to identify the chat session when sending messages.
        /// If the session is ended and a new one is started, it will have a different SessionId - even if it is the same contact.
        /// </summary>
        public Guid SessionId { get; set; }
        public byte[] Bytes { get; set; }

        public VoicePacketDatagram(Guid sessionId, Guid peerConnectionId, byte[] bytes)
        {
            SessionId = sessionId;
            PeerConnectionId = peerConnectionId;
            Bytes = bytes;
        }
    }
}
