using NTDLS.Helpers;
using NTDLS.ReliableMessaging;
using SecureChat.Client.Forms;
using SecureChat.Library.Models;
using static SecureChat.Library.ScConstants;

namespace SecureChat.Client
{
    /// <summary>
    /// Used to store state information about the logged in session.
    /// </summary>
    internal class LocalSession
    {
        public static LocalSession? Current { get; private set; }

        public static void Set(LocalSession localSession)
        {
            Current = localSession;
        }

        public static void Clear()
        {
            Task.Run(() => Current?.Client?.Disconnect());
            Exceptions.Ignore(() => Current?.FormHome?.Close());

            Current = null;
        }

        public RmClient Client { get; private set; }
        public Guid AccountId { get; private set; }
        public string Username { get; private set; }
        public string DisplayName { get; set; }
        public List<ActiveChat> ActiveChats { get; private set; } = new();
        public FormHome FormHome { get; private set; }
        public AccountProfileModel Profile { get; set; } = new();
        public bool ExplicitAway { get; set; }
        public TrayApp Tray { get; private set; }

        public ScOnlineState State { get; set; } = ScOnlineState.Offline;

        public LocalSession(TrayApp tray, FormHome formHome, RmClient client, Guid accountId, string username, string displayName)
        {
            Tray = tray;
            FormHome = formHome;
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
