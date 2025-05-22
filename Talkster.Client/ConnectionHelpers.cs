using NTDLS.DatagramMessaging;
using NTDLS.Helpers;
using NTDLS.ReliableMessaging;
using NTDLS.SecureKeyExchange;
using System.Reflection;
using Talkster.Client.Helpers;
using Talkster.Client.Models;
using Talkster.Library;
using Talkster.Library.ReliableMessages;

namespace Talkster.Client
{
    internal class ConnectionHelpers
    {
        /// <summary>
        /// Creates a new active chat and starts the end to end key exchange process.
        /// </summary>
        internal static ActiveChat? EstablishEndToEndConnectionFor(Guid accountId)
        {
            //Start the key exchange process then popup the chat window.
            if (ServerConnection.Current?.Connection.Client.IsConnected != true)
            {
                throw new Exception("Connection to the server was lost.");
            }

            var accountProfile = ServerConnection.Current.Connection.Client.Query(new GetAccountProfileQuery(accountId))
                .ContinueWith(o =>
                {
                    if (!o.IsFaulted && o.Result.IsSuccess)
                    {
                        return o.Result;
                    }
                    return null;
                }).Result;
            if (accountProfile?.Account == null)
            {
                throw new Exception("The requested account was not found.");
            }

            ActiveChat? activeChat = null;

            var sessionId = Guid.NewGuid();

            var compoundNegotiator = new CompoundNegotiator();
            var negotiationToken = compoundNegotiator.GenerateNegotiationToken((int)(Math.Ceiling(Settings.Instance.EndToEndKeySize / 128.0)));

            //The first thing we do when we get a connection is start a new key exchange process.
            var queryRequestKeyExchangeReply = ServerConnection.Current.Connection.Client.Query(
                new InitiatePeerToPeerSessionQuery(sessionId, ServerConnection.Current.AccountId, accountId, ServerConnection.Current.DisplayName, negotiationToken))
                .ContinueWith(o =>
                {
                    if (!o.IsFaulted && o.Result.IsSuccess)
                    {
                        return o.Result;
                    }
                    return null;
                }).Result;

            if (queryRequestKeyExchangeReply != null)
            {
                //We received a reply to the secure key exchange, apply it.
                compoundNegotiator.ApplyNegotiationResponseToken(queryRequestKeyExchangeReply.NegotiationToken);

                activeChat = ServerConnection.Current.AddActiveChat(sessionId,
                    queryRequestKeyExchangeReply.PeerConnectionId, accountId, accountProfile.Account.DisplayName, compoundNegotiator.SharedSecret);

                activeChat.Form.Show();
            }
            else
            {
                throw new Exception($"Failed to establish a connection with {accountProfile.Account.DisplayName}.");
            }

            return activeChat;
        }

        /// <summary>
        /// Creates a new datagram client and adds the default handlers to it.
        /// </summary>
        internal static DmClient CreateDmClient()
        {
            var dmClient = new DmClient(Settings.Instance.ServerAddress, Settings.Instance.ServerPort);
            dmClient.AddHandler(new ClientDatagramMessageHandlers());

            dmClient.OnException += (DmContext? context, Exception ex) =>
            {
                Program.Log.Error(ex);
            };

            return dmClient;
        }

        /// <summary>
        /// Creates a new encrypted RmClient, negotiates the cryptography with the server and logs in the user.
        /// The exceptionEvent is only used to communicate RmClient exceptions to the caller and is unsubscribed from the RmClient when the method is done.
        /// </summary>
        internal static LoginResult? CreateLoggedInConnection(string username, string passwordHash,
            RmClient.ExceptionEvent exceptionEvent, ThemedProgressForm? progressForm = null)
        {
            var connection = CreateEncryptedConnection(exceptionEvent, progressForm);

            try
            {
                connection.Client.OnException += exceptionEvent;

                bool explicitAway = false;
                if (Settings.Instance.Users.TryGetValue(username, out var userPersist))
                {
                    //If the user has an explicit away state, send it to the server at
                    //  login so the server can update the user's status appropriately.
                    explicitAway = userPersist.ExplicitAway;
                }

                progressForm?.SetHeaderText("Logging in...");

                var loginReply = connection.Client.Query(new LoginQuery(username, passwordHash, explicitAway)).Result;
                if (!loginReply.IsSuccess)
                {
                    connection.Client.Disconnect();
                    throw new Exception(string.IsNullOrEmpty(loginReply.ErrorMessage) ? "Unknown login error." : loginReply.ErrorMessage);
                }

                var loginResult = new LoginResult(connection, loginReply.AccountId, loginReply.Username,
                    passwordHash, loginReply.DisplayName, loginReply.ProfileJson.EnsureNotNull());

                if (!Settings.Instance.Users.TryGetValue(username, out var userState))
                {
                    Settings.Instance.Users.Add(username, new PersistedUserState());
                }
                else
                {
                    userState.LastLogin = DateTime.UtcNow;
                }

                Settings.Save();

                return loginResult;
            }
            finally
            {
                connection.Client.OnException -= exceptionEvent;
            }
        }

        /// <summary>
        /// Creates a new encrypted RmClient and negotiates the cryptography with the server.
        /// The exceptionEvent is only used to communicate RmClient exceptions to the caller and is unsubscribed from the RmClient when the method is done.
        /// </summary>
        internal static NegotiatedConnection CreateEncryptedConnection(RmClient.ExceptionEvent exceptionEvent, ThemedProgressForm? progressForm = null)
        {
            progressForm?.SetHeaderText("Negotiating cryptography...");

            var rmConfig = new RmConfiguration()
            {
            };

            var rmClient = new RmClient(rmConfig);
            rmClient.SetCompressionProvider(new RmDeflateCompressionProvider());

            rmClient.OnException += (RmContext? context, Exception ex, IRmPayload? payload) =>
            {
                Program.Log.Error(ex);
            };

            rmClient.OnException += exceptionEvent;

            try
            {
                rmClient.Connect(Settings.Instance.ServerAddress, Settings.Instance.ServerPort);

                var keyPair = Crypto.GeneratePublicPrivateKeyPair(Settings.Instance.RsaKeySize);

                var clientVersion = (Assembly.GetEntryAssembly()?.GetName().Version).EnsureNotNull();

                //Send our public key to the server and wait on a reply of their public key.
                var keyExchangeResult = rmClient.Query(new ExchangePublicKeyQuery(rmClient.ConnectionId.EnsureNotNull(), clientVersion,
                    keyPair.PublicRsaKey, Settings.Instance.RsaKeySize, Settings.Instance.AesKeySize)).Result;

                if (!keyExchangeResult.IsSuccess)
                {
                    throw new Exception(string.IsNullOrEmpty(keyExchangeResult.ErrorMessage) ? "Unknown negotiation error." : keyExchangeResult.ErrorMessage);
                }

                if (keyExchangeResult.ServerVersion < ScConstants.MinServerVersion)
                    throw new Exception($"Server version is unsupported, use version {ScConstants.MinServerVersion} or greater.");

                rmClient.Query(new InitializeServerClientCryptographyQuery(), () =>
                {
                    rmClient.SetCryptographyProvider(new ReliableCryptographyProvider(
                        Settings.Instance.RsaKeySize, Settings.Instance.AesKeySize, keyExchangeResult.PublicRsaKey, keyPair.PrivateRsaKey));
                });

                return new NegotiatedConnection(rmClient, keyExchangeResult.ServerVersion);
            }
            finally
            {
                rmClient.OnException -= exceptionEvent;
            }
        }

    }
}
