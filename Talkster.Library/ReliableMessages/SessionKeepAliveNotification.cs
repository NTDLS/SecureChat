using NTDLS.ReliableMessaging;

namespace Talkster.Library.ReliableMessages
{
    public class SessionKeepAliveNotification
        : IRmNotification
    {
        /// <summary>
        /// Identifies this chat session. This is used to identify the chat session when sending messages.
        /// If the session is ended and a new one is started, it will have a different SessionId - even if it is the same contact.
        /// </summary>
        public Guid SessionId { get; set; }

        public SessionKeepAliveNotification(Guid sessionId)
        {
            SessionId = sessionId;
        }
    }
}
