namespace SecureChat.Library
{
    public static class ScConstants
    {
        public const string AppName = "Secure Chat";
        public const int EndToEndKeySize = 4096;
        public const int RsaKeySize = 4096;
        public const int AesKeySize = 256;
        public const int OfflineLastSeenSeconds = 600;
        public const int MinPasswordLength = 8;

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
