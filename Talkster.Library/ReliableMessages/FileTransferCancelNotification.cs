using NTDLS.ReliableMessaging;

namespace Talkster.Library.ReliableMessages
{
    public class FileTransferCancelNotification
        : IRmNotification
    {
        public Guid FileId { get; set; }

        /// <summary>
        /// The connection id of the remote peer that this message is being sent to.
        /// </summary>
        public Guid PeerConnectionId { get; set; }

        /// <summary>
        /// Identifies this chat session. This is used to identify the chat session when sending messages.
        /// If the session is ended and a new one is started, it will have a different SessionId - even if it is the same contact.
        /// </summary>
        public Guid SessionId { get; set; }

        public FileTransferCancelNotification(Guid sessionId, Guid peerConnectionId, Guid fileId)
        {
            SessionId = sessionId;
            PeerConnectionId = peerConnectionId;
            FileId = fileId;
        }
    }
}
