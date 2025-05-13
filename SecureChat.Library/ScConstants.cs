namespace SecureChat.Library
{
    public static class ScConstants
    {
        public enum Theme
        {
            Light,
            Dark
        }

        public static readonly string[] ImageFileTypes = { ".bmp", ".jpg", ".jpeg", ".png", ".gif" };

        public static readonly int[] AcceptableRsaKeySizes = new int[] { 1024, 2048, 3072, 4096 };
        public static readonly int[] AcceptableAesKeySizes = new int[] { 128, 192, 256 };

        public const float DefaultFontSize = 10.0f;
        public const int DefaultAesKeySize = 256;
        public const int DefaultAutoAwayIdleSeconds = 600;
        public const int DefaultFileTransferChunkSize = 1024 * 8;
        public const int DefaultMaxMessages = 100;
        public const int DefaultMaxFileDrops = 10;
        public const int DefaultServerPort = 13265;
        public const int DefaultEndToEndKeySize = 4096;
        public const int MinPasswordLength = 8;
        public const int OfflineLastSeenSeconds = 60;
        public const int DefaultRsaKeySize = 4096;
        public const string AppName = "Secure Chat";
        public const string DefaultFont = "Cascadia Mono SemiLight";
        public const string DefaultServerAddress = "securechat.ntdls.com";
        public readonly static Version MinClientVersion = new(1, 0, 11, 0);

        public enum ScAlignment
        {
            Left,
            Right
        }

        public enum ScOrigin
        {
            None,
            Local,
            Remote
        }


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
