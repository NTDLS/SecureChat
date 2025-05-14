using Krypton.Toolkit;
using NTDLS.DatagramMessaging;
using NTDLS.Helpers;
using NTDLS.Persistence;
using NTDLS.ReliableMessaging;
using NTDLS.WinFormsHelpers;
using SecureChat.Client.Models;
using SecureChat.Library;
using SecureChat.Library.ReliableMessages;
using Serilog;
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

        public DmClient CreateDmClient()
        {
            var dmClient = new DmClient(ServerAddress, ServerPort);
            dmClient.AddHandler(new ClientDatagramMessageHandlers());

            dmClient.OnException += (DmContext? context, Exception ex) =>
            {
                var baseException = ex.GetBaseException();
                Log.Error(baseException, baseException.Message);
            };

            return dmClient;
        }

        public RmClient CreateEncryptedRmClient(RmClient.ExceptionEvent exceptionEvent, ProgressForm? progressForm = null)
        {
            progressForm?.SetHeaderText("Negotiating cryptography...");

            var rmConfig = new RmConfiguration()
            {
                AsynchronousNotifications = false //Used to ensure the order of chunks is preserved (such as file transfers).
            };

            var rmClient = new RmClient(rmConfig);

            rmClient.Connect(ServerAddress, ServerPort);

            var keyPair = Crypto.GeneratePublicPrivateKeyPair(RsaKeySize);
            rmClient.OnException += exceptionEvent;

            var appVersion = (Assembly.GetEntryAssembly()?.GetName().Version).EnsureNotNull();

            //Send our public key to the server and wait on a reply of their public key.
            var remotePublicKey = rmClient.Query(new ExchangePublicKeyQuery(rmClient.ConnectionId.EnsureNotNull(), appVersion,
                keyPair.PublicRsaKey, RsaKeySize, AesKeySize))
                .ContinueWith(o =>
                {
                    if (o.IsFaulted || !o.Result.IsSuccess)
                    {
                        throw new Exception(string.IsNullOrEmpty(o.Result.ErrorMessage) ? "Unknown negotiation error." : o.Result.ErrorMessage);
                    }

                    return o.Result.PublicRsaKey;
                }).Result;

            progressForm?.SetHeaderText("Applying cryptography...");

            rmClient.Notify(new InitializeServerClientCryptographyNotification());
            rmClient.SetCryptographyProvider(new ReliableCryptographyProvider(
                RsaKeySize, AesKeySize, remotePublicKey, keyPair.PrivateRsaKey));

            progressForm?.SetHeaderText("Waiting for server...");

            Thread.Sleep(1000); //Give the server a moment to initialize the cryptography.

            return rmClient;
        }
    }
}
