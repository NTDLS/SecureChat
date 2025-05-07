using System.Drawing;

namespace SecureChat.Library
{
    public static class ScConstants
    {
        public static readonly string[] ImageFileTypes = { ".bmp", ".jpg", ".jpeg", ".png", ".gif" };

        public const float DefaultFontSize = 10.0f;
        public const int AesKeySize = 256;
        public const int DefaultAutoAwayIdleSeconds = 600;
        public const int DefaultFileTransmissionChunkSize = 1024 * 8;
        public const int DefaultMaxFileTransmissionSize = 1024 * 1024 * 10;
        public const int DefaultMaxMessages = 100;
        public const int DefaultMaxFileDrops = 10;
        public const int DefaultServerPort = 13265;
        public const int EndToEndKeySize = 4096;
        public const int MinPasswordLength = 8;
        public const int OfflineLastSeenSeconds = 60;
        public const int RsaKeySize = 4096;
        public const string AppName = "Secure Chat";
        public const string DefaultFont = "Cascadia Mono SemiLight";
        public const string DefaultServerAddress = "securechat.ntdls.com";
        public readonly static Version MinClientVersion = new Version(1, 0, 2, 0);

        /// <summary>
        /// Color used for display name when message are from the local client.
        /// </summary>
        public static readonly Color FromMeColor = Color.Blue;

        /// <summary>
        /// Color used for display name when message are from remote clients.
        /// </summary>
        public static readonly Color FromRemoteColor = Color.DarkRed;

        public enum ScOnlineState
        {
            Offline,
            Online,
            Away,
            Pending
        }

        /// <summary>
        /// Used for message and error logging.
        /// </summary>
        public enum ScErrorLevel
        {
            /// <summary>
            /// Use for detailed diagnostic information.
            /// </summary>
            Verbose,
            /// <summary>
            /// Use for debugging information.
            /// </summary>
            Debug,
            /// <summary>
            /// Use for general informational messages.
            /// </summary>
            Information,
            /// <summary>
            /// Use for potentially harmful situations.
            /// </summary>
            Warning,
            /// <summary>
            ///Use for errors that prevent the execution of a specific part of the application.    
            /// </summary>
            Error,
            /// <summary>
            /// Use for critical errors that cause the application to crash or terminate.
            /// </summary>
            Fatal
        }
    }
}
