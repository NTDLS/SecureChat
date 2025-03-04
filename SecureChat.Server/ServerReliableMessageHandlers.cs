using Microsoft.Extensions.Configuration;
using NTDLS.Helpers;
using NTDLS.ReliableMessaging;
using SecureChat.Library;
using SecureChat.Library.ReliableMessages;
using Serilog;
using System.Diagnostics;

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
        private readonly DatabaseRepository _dbRepository;

        public ServerReliableMessageHandlers(IConfiguration configuration, ChatService chatService)
        {
            _configuration = configuration;
            _chatService = chatService;
            _dbRepository = new DatabaseRepository(configuration);
        }

        /// <summary>
        /// Client is sending an update to their display name and/or profile
        /// </summary>
        public UpdateAccountProfileReply UpdateAccountProfile(RmContext context, UpdateAccountProfile param)
        {
            try
            {
                if (context.GetCryptographyProvider() == null)
                    throw new Exception("Cryptography has not been initialized.");

                var session = _chatService.GetSessionByConnectionId(context.ConnectionId)
                    ?? throw new Exception("Session not found.");


                var account = _dbRepository.GetAccountById(session.AccountId.EnsureNotNull());

                if (account.DisplayName != param.DisplayName)
                {
                    _dbRepository.UpdateAccountDisplayName(session.AccountId.EnsureNotNull(), param.DisplayName);
                }

                _dbRepository.UpdateAccountProfile(session.AccountId.EnsureNotNull(), param.Profile);

                return new UpdateAccountProfileReply();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                return new UpdateAccountProfileReply(ex);
            }
        }

        /// <summary>
        /// Client is accepting a contact invite request.
        /// </summary>
        public AcceptContactInviteReply AcceptContactInvite(RmContext context, AcceptContactInvite param)
        {
            try
            {
                if (context.GetCryptographyProvider() == null)
                    throw new Exception("Cryptography has not been initialized.");

                var session = _chatService.GetSessionByConnectionId(context.ConnectionId)
                    ?? throw new Exception("Session not found.");

                _dbRepository.AcceptContactInvite(param.AccountId, session.AccountId.EnsureNotNull());

                return new AcceptContactInviteReply();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                return new AcceptContactInviteReply(ex);
            }
        }

        /// <summary>
        /// Client is sending a request to remove a contact
        /// </summary>
        public RemoveContactQueryReply RemoveContactQuery(RmContext context, RemoveContactQuery param)
        {
            try
            {
                if (context.GetCryptographyProvider() == null)
                    throw new Exception("Cryptography has not been initialized.");

                var session = _chatService.GetSessionByConnectionId(context.ConnectionId)
                    ?? throw new Exception("Session not found.");

                _dbRepository.RemoveContact(session.AccountId.EnsureNotNull(), param.AccountId);

                return new RemoveContactQueryReply();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                return new RemoveContactQueryReply(ex);
            }
        }

        /// <summary>
        /// Client is sending a request to invite another contact.
        /// </summary>
        public InviteContactQueryReply InviteContactQuery(RmContext context, InviteContactQuery param)
        {
            try
            {
                if (context.GetCryptographyProvider() == null)
                    throw new Exception("Cryptography has not been initialized.");

                var session = _chatService.GetSessionByConnectionId(context.ConnectionId)
                    ?? throw new Exception("Session not found.");

                _dbRepository.AddContactInvite(session.AccountId.EnsureNotNull(), param.AccountId);

                return new InviteContactQueryReply();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                return new InviteContactQueryReply(ex);
            }
        }

        /// <summary>
        /// The client is searching for a contact, likely to add them as a contact.
        /// </summary>
        public AccountSearchQueryReply AccountSearchQuery(RmContext context, AccountSearchQuery param)
        {
            try
            {
                if (context.GetCryptographyProvider() == null)
                    throw new Exception("Cryptography has not been initialized.");

                var session = _chatService.GetSessionByConnectionId(context.ConnectionId)
                    ?? throw new Exception("Session not found.");

                var accounts = _dbRepository.AccountSearch(session.AccountId.EnsureNotNull(), param.DisplayName);

                return new AccountSearchQueryReply(accounts);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                return new AccountSearchQueryReply(ex);
            }
        }

        /// <summary>
        /// A client is requesting to create a new account.
        /// </summary>
        public CreateAccountQueryReply CreateAccountQuery(RmContext context, CreateAccountQuery param)
        {
            try
            {
                if (context.GetCryptographyProvider() == null)
                    throw new Exception("Cryptography has not been initialized.");

                _dbRepository.CreateAccount(param.Username, param.DisplayName, param.PasswordHash);

                return new CreateAccountQueryReply();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                return new CreateAccountQueryReply(ex);
            }
        }

        /// <summary>
        /// The client is beginning to transmit a file. Relay it to the appropriate client.
        /// </summary>
        public void FileTransmissionBegin(RmContext context, FileTransmissionBegin param)
        {
            try
            {
                if (context.GetCryptographyProvider() == null)
                    throw new Exception("Cryptography has not been initialized.");

                _chatService.RmServer.Notify(param.ConnectionId, param);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }

        /// <summary>
        /// The client transmitting a file chunk. Relay it to the appropriate client.
        /// </summary>
        public void FileTransmissionChunk(RmContext context, FileTransmissionChunk param)
        {
            try
            {
                if (context.GetCryptographyProvider() == null)
                    throw new Exception("Cryptography has not been initialized.");

                _chatService.RmServer.Notify(param.ConnectionId, param);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }

        /// <summary>
        /// The client has finished transmitting a file. Relay it to the appropriate client.
        /// </summary>
        public FileTransmissionEndReply FileTransmissionEnd(RmContext context, FileTransmissionEnd param)
        {
            try
            {
                if (context.GetCryptographyProvider() == null)
                    throw new Exception("Cryptography has not been initialized.");

                return _chatService.RmServer.Query(param.ConnectionId, param).Result;
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                return new FileTransmissionEndReply(ex);
            }
        }

        /// <summary>
        /// A client is letting the server know that they are terminating the chat.
        /// Relay the message to the other client.
        /// </summary>
        public void TerminateChat(RmContext context, TerminateChat param)
        {
            try
            {
                if (context.GetCryptographyProvider() == null)
                    throw new Exception("Cryptography has not been initialized.");

                _chatService.RmServer.Notify(param.ConnectionId, param);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }

        /// <summary>
        /// A client is updating the server about their state/status.
        /// </summary>
        public void UpdateAccountState(RmContext context, UpdateAccountState param)
        {
            try
            {
                if (context.GetCryptographyProvider() == null)
                    throw new Exception("Cryptography has not been initialized.");

                _dbRepository.UpdateAccountState(param.AccountId, param.State);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
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
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                return new ExchangePeerToPeerQueryReply(ex);
            }
        }

        /// <summary>
        /// A client is telling the server that it would like to establish end-to-end encryption with another client.
        /// </summary>
        public InitiateEndToEndCryptographyReply InitiateEndToEndCryptography(RmContext context, InitiateEndToEndCryptography param)
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
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
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

                var login = _dbRepository.Login(param.Username, param.PasswordHash, param.ExplicitAway)
                    ?? throw new Exception("Invalid username or password.");

                session.SetAccountId(login.Id);

                return new LoginQueryReply(
                    login.Id.EnsureNotNull(),
                    login.Username.EnsureNotNull(),
                    login.DisplayName.EnsureNotNull(),
                    login.ProfileJson ?? string.Empty);
            }
            catch (Exception ex)
            {
                return new LoginQueryReply(ex.GetBaseException());
            }
        }

        /// <summary>
        /// Client is requesting a list of contacts for their account.
        /// </summary>
        public GetContactsQueryReply GetContactsQuery(RmContext context, GetContactsQuery param)
        {
            try
            {
                var session = _chatService.GetSessionByConnectionId(context.ConnectionId)
                    ?? throw new Exception("Session not found.");

                var contacts = _dbRepository.GetContacts(session.AccountId.EnsureNotNull());

                return new GetContactsQueryReply(contacts);
            }
            catch (Exception ex)
            {
                return new GetContactsQueryReply(ex.GetBaseException());
            }
        }
    }
}
