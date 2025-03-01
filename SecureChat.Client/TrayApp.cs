using NTDLS.Helpers;
using NTDLS.ReliableMessaging;
using SecureChat.Client.Forms;
using SecureChat.Library;
using static SecureChat.Library.Constants;

namespace SecureChat.Client
{
    class TrayApp : ApplicationContext
    {
        private readonly NotifyIcon _trayIcon;
        private RmClient? _rmClient;

        public TrayApp()
        {
            _trayIcon = new NotifyIcon
            {
                Icon = Icon.ExtractIcon(Application.ExecutablePath, 0, 16),
                Text = Constants.AppName,
                Visible = true,
                ContextMenuStrip = new ContextMenuStrip()
            };

            _trayIcon.ContextMenuStrip.Items.Add("Exit", null, OnExit);

            Login();
        }

        private void Login()
        {
            _rmClient = null;

            try
            {
                using var formLogin = new FormLogin();
                var loginResult = formLogin.DoLogin();
                if (loginResult != null)
                {
                    _rmClient = new RmClient();
                    _rmClient.Connect(loginResult.ServerAddress, loginResult.ServerPort);
                    _rmClient.OnDisconnected += RmClient_OnDisconnected
                        ;

                    //_trayIcon.BalloonTipTitle = "Connection Failure";
                    //_trayIcon.BalloonTipText = $"Welcome back {loginResult.DisplayName}, you are now logged in!";
                    //_trayIcon.ShowBalloonTip(3000);                    

                    SetTrayIcon(ScOnlineStatus.Online);
                }
            }
            catch (Exception ex)
            {
                _trayIcon.BalloonTipTitle = "Connection Failure";
                _trayIcon.BalloonTipText = $"An error has occurred during the connection process.";
                _trayIcon.ShowBalloonTip(3000);

            }
        }

        private void RmClient_OnDisconnected(RmContext context)
        {
            _trayIcon.BalloonTipText = $"You have been disconnected.";
            _trayIcon.ShowBalloonTip(3000);
            SetTrayIcon(ScOnlineStatus.Offline);
        }

        static Icon LoadIconFromResources(byte[] iconData)
        {
            using var stream = new MemoryStream(iconData);
            return new Icon(stream);
        }

        void SetTrayIcon(ScOnlineStatus state)
        {
            _trayIcon.ContextMenuStrip.EnsureNotNull();
            _trayIcon.ContextMenuStrip.Items.Clear();

            //_trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());

            switch (state)
            {
                case ScOnlineStatus.Online:
                    {
                        _trayIcon.Icon = LoadIconFromResources(Properties.Resources.Online16);
                        _trayIcon.ContextMenuStrip.Items.Add("Logout", null, OnLogout);
                    }
                    break;
                case ScOnlineStatus.Offline:
                    {
                        _trayIcon.Icon = LoadIconFromResources(Properties.Resources.Offline16);
                        _trayIcon.ContextMenuStrip.Items.Add("Login", null, OnLogin);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            _trayIcon.ContextMenuStrip.Items.Add("Exit", null, OnExit);
        }

        private void OnLogin(object? sender, EventArgs e)
        {
            Login();
        }

        private void OnLogout(object? sender, EventArgs e)
        {
            _rmClient?.Disconnect();
            _rmClient = null;
        }

        private void OnExit(object? sender, EventArgs e)
        {
            _trayIcon.Visible = false;
            Application.Exit();
        }
    }
}
