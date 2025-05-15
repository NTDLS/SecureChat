using Krypton.Toolkit;
using NTDLS.DatagramMessaging;
using NTDLS.Helpers;
using NTDLS.Persistence;
using NTDLS.ReliableMessaging;
using SecureChat.Client.Helpers;
using SecureChat.Client.Models;
using SecureChat.Library;
using SecureChat.Library.ReliableMessages;
using System.Reflection;

namespace SecureChat.Client
{
    /// <summary>
    /// Settings that are saved to disk.
    /// </summary>
    internal class Settings
    {
        private static Settings? _instance;
        internal static Settings Instance
        {
            get
            {
                _instance ??= LocalUserApplicationData.LoadFromDisk(ScConstants.AppName, new Settings());
                return _instance;
            }
            set
            {
                _instance = value;
                LocalUserApplicationData.SaveToDisk(ScConstants.AppName, _instance);
            }
        }

        public static void Save()
        {
            LocalUserApplicationData.SaveToDisk(ScConstants.AppName, Instance);
        }

        public PaletteMode Theme { get; set; } = Themes.IsWindowsDarkMode() ? PaletteMode.SparkleBlueDarkMode : PaletteMode.Office2010White;

        public string ServerAddress { get; set; } = ScConstants.DefaultServerAddress;
        public string Font { get; set; } = ScConstants.DefaultFont;
        public float FontSize { get; set; } = ScConstants.DefaultFontSize;
        public int ServerPort { get; set; } = ScConstants.DefaultServerPort;
        public int AutoAwayIdleSeconds { get; set; } = ScConstants.DefaultAutoAwayIdleSeconds;
        public int MaxMessages { get; set; } = ScConstants.DefaultMaxMessages;
        public int FileTransferChunkSize { get; set; } = ScConstants.DefaultFileTransferChunkSize;

        public int RsaKeySize { get; set; } = ScConstants.DefaultRsaKeySize;
        public int AesKeySize { get; set; } = ScConstants.DefaultAesKeySize;
        public int EndToEndKeySize { get; set; } = ScConstants.DefaultEndToEndKeySize;

        public bool AlertToastWhenContactComesOnline { get; set; } = true;
        public bool AlertToastWhenMessageReceived { get; set; } = true;
        public bool PlaySoundWhenContactComesOnline { get; set; } = true;
        public bool PlaySoundWhenMessageReceived { get; set; } = true;
        public bool FlashWindowWhenMessageReceived { get; set; } = true;

        /// <summary>
        /// Per user state that is saved to disk.
        /// </summary>
        public Dictionary<string, PersistedUserState> Users = new(StringComparer.CurrentCultureIgnoreCase);

        /// <summary>
        /// Creates a new datagram client and adds the default handlers to it.
        /// </summary>
        public DmClient CreateDmClient()
        {
            var dmClient = new DmClient(ServerAddress, ServerPort);
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
        public LoginResult? CreateLoggedInConnection(string username, string passwordHash,
            RmClient.ExceptionEvent exceptionEvent, ThemedProgressForm? progressForm = null)
        {
            var connection = CreateEncryptedConnection(exceptionEvent, progressForm);

            try
            {
                connection.Client.OnException += exceptionEvent;

                bool explicitAway = false;
                if (Users.TryGetValue(username, out var userPersist))
                {
                    //If the user has an explicit away state, send it to the server at
                    //  login so the server can update the user's status appropriately.
                    explicitAway = userPersist.ExplicitAway;
                }

                progressForm?.SetHeaderText("Logging in...");
                Thread.Sleep(250); //For aesthetics.

                var loginResult = connection.Client.Query(new LoginQuery(username, passwordHash, explicitAway)).ContinueWith(o =>
                {
                    if (string.IsNullOrEmpty(o.Result.ErrorMessage) == false)
                    {
                        throw new Exception(o.Result.ErrorMessage);
                    }

                    if (!o.IsFaulted && o.Result.IsSuccess)
                    {
                        return new LoginResult(connection,
                            o.Result.AccountId.EnsureNotNull(),
                            o.Result.Username.EnsureNotNull(),
                            o.Result.DisplayName.EnsureNotNull(),
                            o.Result.ProfileJson.EnsureNotNull());
                    }

                    return null;
                }).Result;

                if (loginResult == null)
                {
                    connection.Client.Disconnect();
                }
                else
                {
                    if (!Users.TryGetValue(username, out var userState))
                    {
                        Users.Add(username, new PersistedUserState());
                    }
                    else
                    {
                        userState.LastLogin = DateTime.UtcNow;
                    }

                    Save();
                }

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
        public NegotiatedConnection CreateEncryptedConnection(RmClient.ExceptionEvent exceptionEvent, ThemedProgressForm? progressForm = null)
        {
            progressForm?.SetHeaderText("Negotiating cryptography...");

            var rmConfig = new RmConfiguration()
            {
                AsynchronousNotifications = true //Used to ensure the order of chunks is preserved (such as file transfers).
            };

            var rmClient = new RmClient(rmConfig);

            rmClient.OnException += (RmContext? context, Exception ex, IRmPayload? payload) =>
            {
                Program.Log.Error(ex);
            };

            rmClient.OnException += exceptionEvent;

            try
            {
                rmClient.Connect(ServerAddress, ServerPort);

                var keyPair = Crypto.GeneratePublicPrivateKeyPair(RsaKeySize);

                var clientVersion = (Assembly.GetEntryAssembly()?.GetName().Version).EnsureNotNull();

                //Send our public key to the server and wait on a reply of their public key.
                var keyExchangeResult = rmClient.Query(new ExchangePublicKeyQuery(rmClient.ConnectionId.EnsureNotNull(), clientVersion,
                    keyPair.PublicRsaKey, RsaKeySize, AesKeySize))
                    .ContinueWith(o =>
                    {
                        if (o.IsFaulted || !o.Result.IsSuccess)
                        {
                            throw new Exception(string.IsNullOrEmpty(o.Result.ErrorMessage) ? "Unknown negotiation error." : o.Result.ErrorMessage);
                        }

                        return o.Result;
                    }).Result;

                if (keyExchangeResult.ServerVersion < ScConstants.MinServerVersion)
                    throw new Exception($"Server version is unsupported, use version {ScConstants.MinServerVersion} or greater.");

                progressForm?.SetHeaderText("Applying cryptography...");

                rmClient.Notify(new InitializeServerClientCryptographyNotification());
                rmClient.SetCryptographyProvider(new ReliableCryptographyProvider(
                    RsaKeySize, AesKeySize, keyExchangeResult.PublicRsaKey, keyPair.PrivateRsaKey));

                progressForm?.SetHeaderText("Waiting for server...");

                Thread.Sleep(1000); //Give the server a moment to initialize the cryptography.

                return new NegotiatedConnection(rmClient, keyExchangeResult.ServerVersion);
            }
            finally
            {
                rmClient.OnException -= exceptionEvent;
            }
        }
    }
}
