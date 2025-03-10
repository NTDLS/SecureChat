﻿using NTDLS.DatagramMessaging;
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
            Task.Run(() => Current?.RmClient?.Disconnect());
            Exceptions.Ignore(() => Current?.FormHome?.Close());

            Current = null;
        }

        public DatagramMessenger DmClient { get; private set; }
        public RmClient RmClient { get; private set; }
        public Guid AccountId { get; private set; }
        public string Username { get; private set; }
        public string DisplayName { get; set; }
        public Dictionary<Guid, ActiveChat> ActiveChats { get; private set; } = new();
        public FormHome FormHome { get; private set; }
        public AccountProfileModel Profile { get; set; } = new();
        public bool ExplicitAway { get; set; }
        public TrayApp Tray { get; private set; }

        public ScOnlineState State { get; set; } = ScOnlineState.Offline;

        public LocalSession(TrayApp tray, FormHome formHome, RmClient client, Guid accountId, string username, string displayName)
        {
            Tray = tray;
            FormHome = formHome;
            RmClient = client;
            DmClient = new DatagramMessenger();
            Username = username;
            DisplayName = displayName;
            AccountId = accountId;
        }

        /// <summary>
        /// Writes a datagram packet to the server
        /// </summary>
        public void DatagramDispatch(IDmNotification payload)
        {
            DmClient.Dispatch(
#if DEBUG
            "127.0.0.1",
#else
            Settings.Instance.ServerAddress,
#endif
            Settings.Instance.ServerPort, payload);
        }

        /// <summary>
        /// Writes a datagram byte array to the server
        /// </summary>
        public void DatagramDispatch(byte[] bytes)
        {
            DmClient.Dispatch(
#if DEBUG
            "127.0.0.1",
#else
            Settings.Instance.ServerAddress,
#endif
            Settings.Instance.ServerPort, bytes);
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
