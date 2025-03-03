using NTDLS.ReliableMessaging;
using SecureChat.Client.Forms;
using static SecureChat.Library.ScConstants;

namespace SecureChat.Client
{
    /// <summary>
    /// Used to store state information about the logged in session.
    /// </summary>
    internal class LocalSession
    {
        public static LocalSession? Current;

        public RmClient Client { get; set; }
        public Guid AccountId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public List<ActiveChat> ActiveChats { get; set; } = new();
        public NotifyIcon TrayIcon { get; set; }
        public FormHome FormHome { get; set; }

        /// <summary>
        /// User defined status text.
        /// </summary>
        public string Status { get; set; } = string.Empty;
        public bool ExplicitAway { get; set; }

        public ScOnlineState State { get; set; } = ScOnlineState.Offline;

        public LocalSession(NotifyIcon trayIcon, FormHome formHome, RmClient client, Guid accountId, string username, string displayName)
        {
            FormHome = formHome;
            TrayIcon = trayIcon;
            Client = client;
            Username = username;
            DisplayName = displayName;
            AccountId = accountId;
        }

        public ActiveChat AddActiveChat(Guid connectionId, Guid accountId, string displayName, byte[] sharedSecret)
        {
            var activeChat = new ActiveChat(connectionId, accountId, displayName, sharedSecret);
            ActiveChats.Add(activeChat);
            return activeChat;
        }

        public ActiveChat? GetActiveChatByAccountId(Guid accountId)
        {
            foreach (var activeChat in ActiveChats)
            {
                if (activeChat.AccountId == accountId)
                {
                    return activeChat;
                }
            }
            return null;
        }
    }
}
