using Microsoft.Extensions.Configuration;
using NTDLS.ReliableMessaging;
using NTDLS.SqliteDapperWrapper;
using SecureChat.Library.Messages;

namespace SecureChat.Server
{
    internal class QueryHandlers
        : IRmMessageHandler
    {
        private readonly ChatService _chatService;
        private readonly IConfiguration _configuration;
        private readonly ManagedDataStorageFactory _dbFactory;

        public QueryHandlers(IConfiguration configuration, ChatService chatService)
        {
            _configuration = configuration;
            _chatService = chatService;

            var sqliteConnection = _configuration.GetValue<string>("SQLiteConnection");
            _dbFactory = new ManagedDataStorageFactory($"Data Source={sqliteConnection}");
        }

        public LoginQueryReply LoginQuery(RmContext context, LoginQuery param)
        {
            try
            {
                var userId = _dbFactory.ExecuteScalar<int?>(@"SqlQueries\Login.sql",
                    new
                    {
                        Username = param.Username,
                        PasswordHash = param.PasswordHash
                    });
                if (userId == null)
                {
                    return new LoginQueryReply(new Exception("Invalid username or password."));
                }

                return new LoginQueryReply(true);
            }
            catch (Exception ex)
            {
                return new LoginQueryReply(ex.GetBaseException());
            }
        }
    }
}
