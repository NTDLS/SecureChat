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
        private FormHome? _formHome;
        private FormLogin? _formLogin;
        private ScOnlineStatus _currentState = ScOnlineStatus.Offline;

        public TrayApp()
        {
            _trayIcon = new NotifyIcon
            {
                Icon = Icon.ExtractIcon(Application.ExecutablePath, 0, 16),
                Text = Constants.AppName,
                Visible = true,
                ContextMenuStrip = new ContextMenuStrip()
            };

            _trayIcon.MouseDoubleClick += TrayIcon_MouseDoubleClick;

            _trayIcon.ContextMenuStrip.Items.Add("Login", null, OnLogin);
            _trayIcon.ContextMenuStrip.Items.Add("Exit", null, OnExit);

            Login();
        }

        private void TrayIcon_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            if (_rmClient == null)
            {
                Login();
            }
            else
            {
                if (_formHome != null)
                {
                    _formHome.Show();
                    _formHome.BringToFront();
                    _formHome.Activate();
                    _formHome.Focus();
                }
                else
                {
                    using (_formHome = new FormHome())
                    {
                        _formHome.ShowDialog();
                    }
                    _formHome = null;
                }
            }
        }

        private void Login()
        {
            _rmClient = null;

            try
            {
                if (_formLogin != null)
                {
                    _formLogin.Show();
                    _formLogin.BringToFront();
                    _formLogin.Activate();
                    _formLogin.Focus();
                }
                else
                {
                    using (_formLogin = new FormLogin())
                    {
                        var loginResult = _formLogin.DoLogin();
                        if (loginResult != null)
                        {
                            _rmClient = new RmClient();
                            _rmClient.Connect(loginResult.ServerAddress, loginResult.ServerPort);
                            _rmClient.OnDisconnected += RmClient_OnDisconnected;

                            //_trayIcon.BalloonTipTitle = "Connection Failure";
                            //_trayIcon.BalloonTipText = $"Welcome back {loginResult.DisplayName}, you are now logged in!";
                            //_trayIcon.ShowBalloonTip(3000);                    

                            UpdateClientState(ScOnlineStatus.Online);
                        }
                    }
                    _formLogin = null;
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
            UpdateClientState(ScOnlineStatus.Offline);
        }

        static Icon LoadIconFromResources(byte[] iconData)
        {
            using var stream = new MemoryStream(iconData);
            return new Icon(stream);
        }

        void UpdateClientState(ScOnlineStatus state)
        {
            if (state == _currentState)
            {
                return;
            }
            _trayIcon.ContextMenuStrip.EnsureNotNull();

            if (_trayIcon.ContextMenuStrip.InvokeRequired)
            {
                _trayIcon.ContextMenuStrip.Invoke(UpdateClientState, state);
                return;
            }

            try
            {
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

                            _trayIcon.BalloonTipText = $"You have been disconnected.";
                            _trayIcon.ShowBalloonTip(3000);
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }

                _trayIcon.ContextMenuStrip.Items.Add("Exit", null, OnExit);
            }
            finally
            {
                _currentState = state;
            }
        }

        private void OnLogin(object? sender, EventArgs e)
        {
            Login();
        }

        private void OnLogout(object? sender, EventArgs e)
        {
            Task.Run(() => _rmClient?.Disconnect());
            UpdateClientState(ScOnlineStatus.Offline);
            _rmClient = null;
        }

        private void OnExit(object? sender, EventArgs e)
        {
            _formHome?.Close();
            _trayIcon.Visible = false;
            Application.Exit();
        }
    }
}
