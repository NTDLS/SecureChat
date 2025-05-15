using NTDLS.DatagramMessaging;
using NTDLS.Helpers;
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
            Exceptions.Ignore(() => Current?.Terminate());
            Current = null;
        }

        public bool IsTerminated { get; private set; } = false;
        public DmClient DatagramClient { get; private set; }
        public NegotiatedConnection Connection { get; private set; }
        public Guid AccountId { get; private set; }
        public string Username { get; private set; }
        public string DisplayName { get; set; }
        public Dictionary<Guid, ActiveChat> ActiveChats { get; private set; } = new();
        public FormHome FormHome { get; private set; }
        public AccountProfileModel Profile { get; set; } = new();
        public bool ExplicitAway { get; set; }
        public TrayApp Tray { get; private set; }
        public ScOnlineState State { get; set; } = ScOnlineState.Offline;

        public ServerConnection(TrayApp tray, FormHome formHome, NegotiatedConnection connection, Guid accountId, string username, string displayName)
        {
            Tray = tray;
            FormHome = formHome;
            Connection = connection;
            Username = username;
            DisplayName = displayName;
            AccountId = accountId;

            DatagramClient = ConnectionHelpers.CreateDmClient();

            new Thread(() =>
            {
                while (!IsTerminated && Connection.Client.IsConnected == true)
                {
                    try
                    {
                        DatagramClient.Dispatch(new ConnectionKeepAliveDatagram(connection.Client.ConnectionId.EnsureNotNull()));
                    }
                    catch (Exception ex)
                    {
                        Program.Log.Error("Error sending connection keep-alive notification.", ex);
                    }

                    var breakTime = DateTime.UtcNow.AddSeconds(10);
                    while (!IsTerminated && Connection.Client.IsConnected == true && DateTime.UtcNow < breakTime)
                    {
                        Thread.Sleep(500);
                    }
                }
            }).Start();
        }

        public void Terminate()
        {
            foreach (var activeChat in ActiveChats)
            {
                activeChat.Value.Terminate();
                Exceptions.Ignore(() => activeChat.Value.Form.Close(true));

            }
            ActiveChats.Clear();

            Exceptions.Ignore(() => IsTerminated = true);
            Exceptions.Ignore(() => DatagramClient.Stop());
            Exceptions.Ignore(() => Connection?.Client.Disconnect(false));
            Exceptions.Ignore(() => FormHome?.Invoke(() => FormHome.Close()));
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
