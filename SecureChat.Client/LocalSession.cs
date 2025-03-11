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
            Task.Run(() => Current?.ReliableClient?.Disconnect());
            Exceptions.Ignore(() => Current?.FormHome?.Close());

            Current = null;
        }

        public RmClient ReliableClient { get; private set; }
        public Guid AccountId { get; private set; }
        public string Username { get; private set; }
        public string DisplayName { get; set; }
        public Dictionary<Guid, ActiveChat> ActiveChats { get; private set; } = new();
        public FormHome FormHome { get; private set; }
        public AccountProfileModel Profile { get; set; } = new();
        public bool ExplicitAway { get; set; }
        public TrayApp Tray { get; private set; }

        public ScOnlineState State { get; set; } = ScOnlineState.Offline;

        public LocalSession(TrayApp tray, FormHome formHome, RmClient reliableClient, Guid accountId, string username, string displayName)
        {
            Tray = tray;
            FormHome = formHome;
            ReliableClient = reliableClient;
            Username = username;
            DisplayName = displayName;
            AccountId = accountId;
        }

        public ActiveChat AddActiveChat(Guid peerToPeerId, Guid connectionId, Guid accountId, string displayName, byte[] sharedSecret)
        {
            var activeChat = new ActiveChat(peerToPeerId, connectionId, accountId, displayName, sharedSecret);
            ActiveChats.Add(peerToPeerId, activeChat);
            return activeChat;
        }

        public ActiveChat? GetActiveChat(Guid peerToPeerId)
        {
            ActiveChats.TryGetValue(peerToPeerId, out var activeChat);
            return activeChat;
        }

        public ActiveChat? GetActiveChatByAccountId(Guid accountId)
        {
            foreach (var activeChat in ActiveChats)
            {
                if (activeChat.Value.AccountId == accountId && activeChat.Value.IsTerminated == false)
                {
                    return activeChat.Value;
                }
            }
            return null;
        }
    }
}
