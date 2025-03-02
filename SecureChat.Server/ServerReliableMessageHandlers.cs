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
    /// <summary>
    /// Reliable query and notification handler for client-server communication.
    /// </summary>
    internal class ServerReliableMessageHandlers
        : IRmMessageHandler
    {
        private readonly ChatService _chatService;
        private readonly IConfiguration _configuration;
        private readonly ManagedDataStorageFactory _dbFactory;

        public ServerReliableMessageHandlers(IConfiguration configuration, ChatService chatService)
        {
            _configuration = configuration;
            _chatService = chatService;

            var sqliteConnection = _configuration.GetValue<string>("SQLiteConnection");
            _dbFactory = new ManagedDataStorageFactory($"Data Source={sqliteConnection}");
        }

        /// <summary>
        /// A client is updating the server about their state/status.
        /// </summary>
        public void UpdateAccountStatus(RmContext context, UpdateAccountStatus param)
        {
            try
            {
                if (context.GetCryptographyProvider() == null)
                    throw new Exception("Cryptography has not been initialized.");

                _dbFactory.Execute(@"SqlQueries\UpdateAccountStatus.sql",
                    new
                    {
                        AccountId = param.AccountId,
                        State = param.State.ToString(),
                        Status = param.Status ?? string.Empty,
                        LastSeen = DateTime.UtcNow
                    });
            }
            catch (Exception ex)
            {
                Log.Error("Failed to route peer-to-peer message.", ex);
            }
        }

        /// <summary>
        /// A client is sending a message to another client.
        /// </summary>
        public ExchangePeerToPeerQueryReply ExchangePeerToPeerQuery(RmContext context, ExchangePeerToPeerQuery param)
        {
            try
            {
                if (context.GetCryptographyProvider() == null)
                    throw new Exception("Cryptography has not been initialized.");

                return _chatService.RmServer.Query(param.ConnectionId, param).Result;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to route peer-to-peer message.", ex);
                return new ExchangePeerToPeerQueryReply(ex);
            }
        }

        /// <summary>
        /// A client is telling the server that it would like to establish end-to-end encryption with another client.
        /// </summary>
        public InitiateEndToEndCryptographyReply ExchangePublicKeyQuery(RmContext context, InitiateEndToEndCryptography param)
        {
            try
            {
                //Find the session for the requested account (if they are logged in).
                var requestedSession = _chatService.GetSessionByAccountId(param.TargetAccountId)
                    ?? throw new Exception("Remote session not found.");

                //Send the ConnectionId to the other peer.
                param.PeerConnectionId = context.ConnectionId;

                //Relay the query to the requested session and reply to the requester with the other clients reply.
                //This can be found in: ClientReliableMessageHandlers
                var reply = _chatService.RmServer.Query(requestedSession.ConnectionId, param).Result;

                //Reply with the ConnectionId to the requested peer.
                reply.PeerConnectionId = requestedSession.ConnectionId;

                return reply;
            }
            catch (Exception ex)
            {
                return new InitiateEndToEndCryptographyReply(ex.GetBaseException());
            }
        }

        /// <summary>
        /// The remote service is letting us know that they are about to start using the
        /// cryptography provider, so we need to apply the one that we have ready on this end.
        /// </summary>
        public void InitializeServerClientCryptography(RmContext context, InitializeServerClientCryptography notification)
        {
            try
            {
                if (context.GetCryptographyProvider() != null)
                    throw new Exception("Cryptography has already been initialized.");

                var session = _chatService.GetSessionByConnectionId(context.ConnectionId);
                if (session != null)
                {
                    context.SetCryptographyProvider(session.ServerClientCryptographyProvider);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed to apply client-server cryptography.", ex);
            }
        }

        /// <summary>
        /// Client is supplying the server with their public key that should be used for all client-server communication.
        /// Save it, generate our own public-private-key-pair and reply with the public key.
        /// </summary>
        public ExchangePublicKeyQueryReply ExchangePublicKeyQuery(RmContext context, ExchangePublicKeyQuery param)
        {
            try
            {
                if (context.GetCryptographyProvider() != null)
                    throw new Exception("Cryptography has already been initialized.");

                var localPublicPrivateKeyPair = Crypto.GeneratePublicPrivateKeyPair();
                _chatService.RegisterSession(context.ConnectionId, new ServerClientCryptographyProvider(param.PublicRsaKey, localPublicPrivateKeyPair.PrivateRsaKey));
                return new ExchangePublicKeyQueryReply(localPublicPrivateKeyPair.PublicRsaKey);
            }
            catch (Exception ex)
            {
                return new ExchangePublicKeyQueryReply(ex.GetBaseException());
            }
        }

        /// <summary>
        /// Client is supplying the server with login credentials, test them and reply.
        /// </summary>
        public LoginQueryReply LoginQuery(RmContext context, LoginQuery param)
        {
            try
            {
                if (context.GetCryptographyProvider() == null)
                    throw new Exception("Cryptography has not been initialized.");

                var session = _chatService.GetSessionByConnectionId(context.ConnectionId)
                    ?? throw new Exception("Session not found.");

                if (session.AccountId != null)
                {
                    throw new Exception("Session is already logged in.");
                }

                var login = _dbFactory.QueryFirst<LoginModel>(@"SqlQueries\Login.sql",
                    new
                    {
                        Username = param.Username,
                        PasswordHash = param.PasswordHash
                    }) ?? throw new Exception("Invalid username or password.");

                session.SetAccountId(login.Id);

                return new LoginQueryReply(
                    login.Id.EnsureNotNull(),
                    login.Username.EnsureNotNull(),
                    login.DisplayName.EnsureNotNull(),
                    login.Status ?? string.Empty);
            }
            catch (Exception ex)
            {
                return new LoginQueryReply(ex.GetBaseException());
            }
        }

        /// <summary>
        /// Client is requesting a list of acquaintances for their account.
        /// </summary>
        public GetAcquaintancesQueryReply GetAcquaintancesQuery(RmContext context, GetAcquaintancesQuery param)
        {
            try
            {
                var session = _chatService.GetSessionByConnectionId(context.ConnectionId)
                    ?? throw new Exception("Session not found.");

                var acquaintances = _dbFactory.Query<AcquaintanceModel>(@"SqlQueries\GetAcquaintances.sql",
                    new
                    {
                        AccountId = session.AccountId,
                    }).ToList();

                return new GetAcquaintancesQueryReply(acquaintances);
            }
            catch (Exception ex)
            {
                return new GetAcquaintancesQueryReply(ex.GetBaseException());
            }
        }
    }
}
