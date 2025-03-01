﻿using NTDLS.ReliableMessaging;
using static SecureChat.Library.Constants;

namespace SecureChat.Client
{
    internal class SessionState
    {
        public static SessionState? Instance;

        public RmClient Client { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public bool ExplicitAway { get; set; }

        public ScOnlineStatus ConnectionState { get; set; } = ScOnlineStatus.Offline;

        public SessionState(RmClient client, string username, string displayName)
        {
            Client = client;
            Username = username;
            DisplayName = displayName;
        }
    }
}
