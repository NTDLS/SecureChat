using NTDLS.ReliableMessaging;
using SecureChat.Library.Messages;

namespace SecureChat.Server
{
    internal class QueryHandlers
        : IRmMessageHandler
    {
        private readonly ChatService _chatService;

        public QueryHandlers(ChatService chatService)
        {
            _chatService = chatService;
        }

        public LoginQueryReply CreateQueueQuery(RmContext context, LoginQuery param)
        {
            try
            {
                return new LoginQueryReply(true);
            }
            catch (Exception ex)
            {
                return new LoginQueryReply(ex.GetBaseException());
            }
        }
    }
}
