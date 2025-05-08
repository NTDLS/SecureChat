using NTDLS.DatagramMessaging;
using NTDLS.Helpers;
using NTDLS.ReliableMessaging;
using SecureChat.Client.Forms;
using SecureChat.Library.DatagramMessages;
using SecureChat.Library.Models;
using static SecureChat.Library.ScConstants;

namespace SecureChat.Client
{
    /// <summary>
    /// Used to store state information about the logged in session.
    /// </summary>
    internal class ServerConnection
    {
        public static ServerConnection? Current { get; private set; }

        public static void SetCurrent(ServerConnection localSession)
        {
            Current = localSession;
        }

        public static void TerminateCurrent()
        {
            if (Current != null)
            {
                Exceptions.Ignore(() => Current.IsTerminated = true);
                Exceptions.Ignore(() => Current.DatagramClient.Stop());
                //Task.Run(() => Exceptions.Ignore(() => Current.ReliableClient?.Disconnect())); //Do we need to do this?
                Exceptions.Ignore(() => Current.ReliableClient?.Disconnect());
                Exceptions.Ignore(() => Current.FormHome?.Close());
            }

            Current = null;
        }

        public bool IsTerminated { get; private set; } = false;
        public DmClient DatagramClient { get; private set; }
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

        public ServerConnection(TrayApp tray, FormHome formHome, RmClient reliableClient, Guid accountId, string username, string displayName)
        {
            Tray = tray;
            FormHome = formHome;
            ReliableClient = reliableClient;
            Username = username;
            DisplayName = displayName;
            AccountId = accountId;

            DatagramClient = Settings.Instance.CreateDmClient();

            var keepAliveThread = new Thread(() =>
            {
                while (!IsTerminated)
                {
                    try
                    {
                        DatagramClient.Dispatch(new ConnectionKeepAliveDatagram(reliableClient.ConnectionId.EnsureNotNull()));
                    }
                    catch (Exception ex)
                    {
                        //TODO: Log or report
                    }

                    var breakTime = DateTime.UtcNow.AddSeconds(10);
                    while (!IsTerminated && DateTime.UtcNow < breakTime)
                    {
                        Thread.Sleep(500);
                    }
                }
            });

            keepAliveThread.Start();
        }

        public ActiveChat AddActiveChat(Guid sessionId, Guid peerConnectionId, Guid accountId, string displayName, byte[] sharedSecret)
        {
            var activeChat = new ActiveChat(sessionId, peerConnectionId, accountId, displayName, sharedSecret);
            ActiveChats.Add(sessionId, activeChat);
            return activeChat;
        }

        public ActiveChat? GetActiveChat(Guid sessionId)
        {
            ActiveChats.TryGetValue(sessionId, out var activeChat);
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
