using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class TextMessageNotification
        : IRmNotification
    {
        /// <summary>
        /// Unique identifier for this message.
        /// </summary>
        public Guid MessageId { get; set; }

        public byte[] CipherText { get; set; }

        /// <summary>
        /// The connection id of the remote peer that this message is being sent to.
        /// </summary>
        public Guid PeerConnectionId { get; set; }

        /// <summary>
        /// Identifies this chat session. This is used to identify the chat session when sending messages.
        /// If the session is ended and a new one is started, it will have a different SessionId - even if it is the same contact.
        /// </summary>
        public Guid SessionId { get; set; }

        public TextMessageNotification(Guid sessionId, Guid peerConnectionId, Guid messageId, byte[] cipherText)
        {
            SessionId = sessionId;
            PeerConnectionId = peerConnectionId;
            MessageId = messageId;
            CipherText = cipherText;
        }
    }
}
