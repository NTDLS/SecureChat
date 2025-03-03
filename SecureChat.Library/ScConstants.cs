﻿namespace SecureChat.Library
{
    public static class ScConstants
    {
        public const string AppName = "Secure Chat";
        public const string DefaultServerAddress = "127.0.0.1";
        public const int DefaultServerPort = 13265;
        public const int DefaultAutoAwayIdleSeconds = 600;
        public const int EndToEndKeySize = 4096;
        public const int RsaKeySize = 4096;
        public const int AesKeySize = 256;
        public const int DefaultMaxMessages = 100;
        public const int DefaultFileTransmissionChunkSize = 1024 * 8;
        public const int DefaultMaxFileTransmissionSize = 1024 * 1024 * 10;

        public enum ScOnlineState
        {
            Offline,
            Online,
            Away
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
