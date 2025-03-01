using Microsoft.Extensions.Configuration;
using NTDLS.Helpers;
using NTDLS.ReliableMessaging;
using NTDLS.SqliteDapperWrapper;
using SecureChat.Library;
using SecureChat.Library.ReliableMessages;
using SecureChat.Server.Models;
using Serilog;

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


        /// <summary>
        /// The remote service is letting us know that they are about to start using the cryptography provider,
        /// so we need to apply the one that we have ready on this end.
        /// </summary>
        public void InitializeBaselineCryptography(RmContext context, InitializeBaselineCryptography notification)
        {
            try
            {
                var session = _chatService.GetSession(context.ConnectionId);
                if (session != null)
                {
                    context.SetCryptographyProvider(session.BaselineCryptographyProvider);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public ExchangePublicKeyQueryReply ExchangePublicKeyQuery(RmContext context, ExchangePublicKeyQuery param)
        {
            try
            {
                var localPPKP = Crypto.GeneratePublicPrivateKeyPair();
                _chatService.RegisterSession(context.ConnectionId, new BaselineCryptographyProvider(param.PublicRsaKey, localPPKP.PrivateRsaKey));
                return new ExchangePublicKeyQueryReply(localPPKP.PublicRsaKey);
            }
            catch (Exception ex)
            {
                return new ExchangePublicKeyQueryReply(ex.GetBaseException());
            }
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

                var session = _chatService.GetSession(context.ConnectionId);
                if (session != null)
                {
                    session.SetAccountId(login.Id);
                    return new LoginQueryReply(login.Username.EnsureNotNull(), login.DisplayName.EnsureNotNull());
                }
                return new LoginQueryReply(new Exception("Session not found."));
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
