using NTDLS.Helpers;
using NTDLS.Persistence;
using NTDLS.ReliableMessaging;
using SecureChat.Client.Forms;
using SecureChat.Client.Properties;
using SecureChat.Library;
using static SecureChat.Library.Constants;

namespace SecureChat.Client
{
    class TrayApp : ApplicationContext
    {
        private readonly NotifyIcon _trayIcon;
        private FormHome? _formHome;
        private FormLogin? _formLogin;

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
            if (SessionState.Instance?.Client == null)
            {
                Login();
            }
            else
            {
                if (_formHome != null)
                {
                    _formHome.Show();

                    if (_formHome.WindowState == FormWindowState.Minimized)
                    {
                        _formHome.WindowState = FormWindowState.Normal;
                    }

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
            SessionState.Instance = null;

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
                            var persistedState = LocalUserApplicationData.LoadFromDisk(Constants.AppName, new PersistedState());
                            SessionState.Instance = new SessionState(loginResult.Client, loginResult.Username, loginResult.DisplayName);
                            loginResult.Client.OnDisconnected += RmClient_OnDisconnected;

                            if (persistedState.Users.TryGetValue(loginResult.Username, out var persistedUserState) == false)
                            {
                                persistedUserState = new();
                            }

                            if (persistedUserState.ExplicitAway)
                            {
                                UpdateClientState(ScOnlineStatus.Away);
                            }
                            else
                            {
                                UpdateClientState(ScOnlineStatus.Online);
                            }

                            _trayIcon.BalloonTipText = $"Welcome back {loginResult.DisplayName}, you are now logged in.";
                            _trayIcon.ShowBalloonTip(3000);
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

        void UpdateClientState(ScOnlineStatus state)
        {
            if (state == SessionState.Instance?.ConnectionState)
            {
                return;
            }
            if (SessionState.Instance != null)
            {
                SessionState.Instance.ConnectionState = state;
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

                switch (state)
                {
                    case ScOnlineStatus.Online:
                        {
                            _trayIcon.Icon = Imaging.LoadIconFromResources(Resources.Online16);
                            var awayItem = new ToolStripMenuItem("Away", null, OnAway)
                            {
                                Checked = false
                            };
                            _trayIcon.ContextMenuStrip.Items.Add(awayItem); _trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                            _trayIcon.ContextMenuStrip.Items.Add("Logout", null, OnLogout);
                        }
                        break;
                    case ScOnlineStatus.Offline:
                        {
                            _trayIcon.Icon = Imaging.LoadIconFromResources(Resources.Offline16);
                            _trayIcon.ContextMenuStrip.Items.Add("Login", null, OnLogin);

                            _trayIcon.BalloonTipText = $"You have been disconnected.";
                            _trayIcon.ShowBalloonTip(3000);
                        }
                        break;
                    case ScOnlineStatus.Away:
                        {
                            _trayIcon.Icon = Imaging.LoadIconFromResources(Resources.Away16);
                            var awayItem = new ToolStripMenuItem("Away", null, OnAway)
                            {
                                Checked = true
                            };
                            _trayIcon.ContextMenuStrip.Items.Add(awayItem);
                            _trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                            _trayIcon.ContextMenuStrip.Items.Add("Logout", null, OnLogout);
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }

                _trayIcon.ContextMenuStrip.Items.Add("Exit", null, OnExit);
            }
            catch (Exception ex)
            {
            }
        }

        private void OnAway(object? sender, EventArgs e)
        {
            if (SessionState.Instance != null && sender is ToolStripMenuItem menuItem)
            {
                SessionState.Instance.ExplicitAway = !SessionState.Instance.ExplicitAway;

                menuItem.Checked = SessionState.Instance.ExplicitAway; //Toggle the explicit away state.

                var persistedState = LocalUserApplicationData.LoadFromDisk(Constants.AppName, new PersistedState());

                if (persistedState.Users.TryGetValue(SessionState.Instance.Username, out var persistedUserState) == false)
                {
                    //Add a default state if its not already present.
                    persistedUserState = new();
                    persistedState.Users.Add(SessionState.Instance.Username, persistedUserState);
                }

                persistedUserState.ExplicitAway = SessionState.Instance.ExplicitAway;
                LocalUserApplicationData.SaveToDisk(Constants.AppName, persistedState);
            }
        }

        private void OnLogin(object? sender, EventArgs e)
        {
            Login();
        }

        private void OnLogout(object? sender, EventArgs e)
        {
            Task.Run(() => SessionState.Instance?.Client?.Disconnect());
            Thread.Sleep(10);
            UpdateClientState(ScOnlineStatus.Offline);
            SessionState.Instance = null;
        }

        private void OnExit(object? sender, EventArgs e)
        {
            Task.Run(() => SessionState.Instance?.Client?.Disconnect());

            _formHome?.Close();
            _trayIcon.Visible = false;
            SessionState.Instance = null;
            Application.Exit();
        }
    }
}
