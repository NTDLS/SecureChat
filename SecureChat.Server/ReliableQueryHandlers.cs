using Microsoft.Extensions.Configuration;
using NTDLS.ReliableMessaging;
using NTDLS.SqliteDapperWrapper;
using SecureChat.Library.ReliableMessages;
using SecureChat.Server.Models;

namespace SecureChat.Server
{
    internal class ReliableQueryHandlers
        : IRmMessageHandler
    {
        private readonly ChatService _chatService;
        private readonly IConfiguration _configuration;
        private readonly ManagedDataStorageFactory _dbFactory;

        public ReliableQueryHandlers(IConfiguration configuration, ChatService chatService)
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
                var login = _dbFactory.QueryFirst<LoginModel>(@"SqlQueries\Login.sql",
                    new
                    {
                        Username = param.Username,
                        PasswordHash = param.PasswordHash
                    });
                if (login == null)
                {
                    return new LoginQueryReply(new Exception("Invalid username or password."));
                }

                _chatService.RegisterSession(context.ConnectionId, login.Id);

                return new LoginQueryReply(true)
                {
                    DisplayName = login.DisplayName
                };
            }
            catch (Exception ex)
            {
                return new LoginQueryReply(ex.GetBaseException());
            }
        }

        public GetAcquaintancesQueryReply GetAcquaintancesQuery(RmContext context, GetAcquaintancesQuery param)
        {
            try
            {
                var session = _chatService.GetSession(context.ConnectionId);
                if (session != null)
                {
                    var acquaintances = _dbFactory.Query<AcquaintancesModel>(@"SqlQueries\GetAcquaintances.sql",
                        new
                        {
                            AccountId = session.AccountId,
                        }).ToList();

                    return new GetAcquaintancesQueryReply(acquaintances);
                }
                return new GetAcquaintancesQueryReply(new Exception("Session not found."));
            }
            catch (Exception ex)
            {
                return new GetAcquaintancesQueryReply(ex.GetBaseException());
            }
        }
    }
}
