using NTDLS.ReliableMessaging;

namespace Talkster.Library.ReliableMessages
{
    /// <summary>
    /// Let the remote peer know that the message was received and decrypted successfully.
    /// </summary>
    public class TextMessageAcknowledgmentNotification
        : IRmNotification
    {
        /// <summary>
        /// Unique identifier for this message.
        /// </summary>
        public Guid MessageId { get; set; }

        /// <summary>
        /// The connection id of the remote peer that this message is being sent to.
        /// </summary>
        public Guid PeerConnectionId { get; set; }

        /// <summary>
        /// Identifies this chat session. This is used to identify the chat session when sending messages.
        /// If the session is ended and a new one is started, it will have a different SessionId - even if it is the same contact.
        /// </summary>
        public Guid SessionId { get; set; }

        public TextMessageAcknowledgmentNotification(Guid sessionId, Guid peerConnectionId, Guid messageId)
        {
            SessionId = sessionId;
            PeerConnectionId = peerConnectionId;
            MessageId = messageId;
        }
    }
}
