using NTDLS.Helpers;
using NTDLS.ReliableMessaging;
using SecureChat.Client.Forms;
using SecureChat.Client.Properties;
using SecureChat.Library;
using Serilog;
using System.Diagnostics;
using static SecureChat.Library.ScConstants;

namespace SecureChat.Client
{
    class TrayApp : ApplicationContext
    {
        private readonly NotifyIcon _trayIcon;
        private FormLogin? _formLogin;

        public TrayApp()
        {
            try
            {
                _trayIcon = new NotifyIcon
                {
                    Icon = Icon.ExtractIcon(Application.ExecutablePath, 0, 16),
                    Text = ScConstants.AppName,
                    Visible = true,
                    ContextMenuStrip = new ContextMenuStrip()
                };

                _trayIcon.MouseDoubleClick += TrayIcon_MouseDoubleClick;

                _trayIcon.ContextMenuStrip.Items.Add("Login", null, OnLogin);
                _trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                _trayIcon.ContextMenuStrip.Items.Add("About", null, OnAbout);
                _trayIcon.ContextMenuStrip.Items.Add("Settings", null, OnSettings);
                _trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                _trayIcon.ContextMenuStrip.Items.Add("Exit", null, OnExit);

                Login();
            }
            catch (Exception ex)
            {
                Log.Fatal($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void TrayIcon_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            try
            {
                if (LocalSession.Current?.Client == null)
                {
                    Login();
                }
                else
                {
                    if (LocalSession.Current.FormHome != null)
                    {
                        LocalSession.Current.FormHome.Show();

                        if (LocalSession.Current.FormHome.WindowState == FormWindowState.Minimized)
                        {
                            LocalSession.Current.FormHome.WindowState = FormWindowState.Normal;
                        }

                        LocalSession.Current.FormHome.BringToFront();
                        LocalSession.Current.FormHome.Activate();
                        LocalSession.Current.FormHome.Focus();
                    }
                    else
                    {
                        //using (SessionState.Instance.FormHome = new FormHome())
                        //{
                        //SessionState.Instance.FormHome.ShowDialog();
                        //}
                        //SessionState.Instance.FormHome = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Login()
        {
            try
            {
                LocalSession.Current = null;

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
                            loginResult.Client.OnDisconnected += RmClient_OnDisconnected;
                            loginResult.Client.OnException += Client_OnException;
                            loginResult.Client.AddHandler(new ClientReliableMessageHandlers());

                            var formHome = new FormHome();
                            formHome.CreateControl(); //Force the window handle to be created before the form is shown,
                            var handle = formHome.Handle; // Accessing the Handle property forces handle creation

                            if (Settings.Instance.Users.TryGetValue(loginResult.Username, out var persistedUserState) == false)
                            {
                                persistedUserState = new();
                            }

                            if (persistedUserState.ExplicitAway)
                            {
                                UpdateClientState(ScOnlineState.Away);
                            }
                            else
                            {
                                UpdateClientState(ScOnlineState.Online);
                            }

                            LocalSession.Current = new LocalSession(_trayIcon,
                                formHome,
                                loginResult.Client,
                                loginResult.AccountId,
                                loginResult.Username,
                                loginResult.DisplayName)
                            {
                                Status = loginResult.Status,
                                ExplicitAway = persistedUserState.ExplicitAway
                            };

                            _trayIcon.BalloonTipText = $"Welcome back {loginResult.DisplayName}, you are now logged in.";
                            _trayIcon.ShowBalloonTip(3000);
                        }
                    }
                    _formLogin = null;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                _trayIcon.BalloonTipTitle = "Login Failure";
                _trayIcon.BalloonTipText = $"An error has occurred during the connection process.";
                _trayIcon.ShowBalloonTip(3000);

            }
        }

        private void Client_OnException(RmContext? context, Exception ex, IRmPayload? payload)
        {
            Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void RmClient_OnDisconnected(RmContext context)
        {
            try
            {
                UpdateClientState(ScOnlineState.Offline);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void UpdateClientState(ScOnlineState state)
        {
            try
            {
                if (state == LocalSession.Current?.ConnectionState)
                {
                    return;
                }
                if (LocalSession.Current != null)
                {
                    LocalSession.Current.ConnectionState = state;
                }

                _trayIcon.ContextMenuStrip.EnsureNotNull();

                if (_trayIcon.ContextMenuStrip.InvokeRequired)
                {
                    _trayIcon.ContextMenuStrip.Invoke(UpdateClientState, state);
                    return;
                }

                _trayIcon.ContextMenuStrip.Items.Clear();

                switch (state)
                {
                    case ScOnlineState.Online:
                        {
                            _trayIcon.Icon = Imaging.LoadIconFromResources(Resources.Online16);
                            var awayItem = new ToolStripMenuItem("Away", null, OnAway)
                            {
                                Checked = false
                            };
                            _trayIcon.ContextMenuStrip.Items.Add(awayItem);
                            _trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                            _trayIcon.ContextMenuStrip.Items.Add("Logout", null, OnLogout);
                        }
                        break;
                    case ScOnlineState.Offline:
                        {
                            _trayIcon.Icon = Imaging.LoadIconFromResources(Resources.Offline16);
                            _trayIcon.ContextMenuStrip.Items.Add("Login", null, OnLogin);

                            _trayIcon.BalloonTipText = $"You have been disconnected.";
                            _trayIcon.ShowBalloonTip(3000);
                        }
                        break;
                    case ScOnlineState.Away:
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

                _trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                _trayIcon.ContextMenuStrip.Items.Add("About", null, OnAbout);
                _trayIcon.ContextMenuStrip.Items.Add("Settings", null, OnSettings);
                _trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                _trayIcon.ContextMenuStrip.Items.Add("Exit", null, OnExit);

            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnAway(object? sender, EventArgs e)
        {
            try
            {
                if (LocalSession.Current != null && sender is ToolStripMenuItem menuItem)
                {
                    LocalSession.Current.ExplicitAway = !LocalSession.Current.ExplicitAway;

                    menuItem.Checked = LocalSession.Current.ExplicitAway; //Toggle the explicit away state.

                    UpdateClientState(menuItem.Checked ? ScOnlineState.Away : ScOnlineState.Online);

                    if (Settings.Instance.Users.TryGetValue(LocalSession.Current.Username, out var persistedUserState) == false)
                    {
                        //Add a default state if its not already present.
                        persistedUserState = new();
                        Settings.Instance.Users.Add(LocalSession.Current.Username, persistedUserState);
                    }

                    persistedUserState.ExplicitAway = LocalSession.Current.ExplicitAway;
                    Settings.Save();
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnLogin(object? sender, EventArgs e)
        {
            try
            {
                Login();

            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnLogout(object? sender, EventArgs e)
        {
            try
            {
                Task.Run(() => LocalSession.Current?.Client?.Disconnect());
                Thread.Sleep(10);
                UpdateClientState(ScOnlineState.Offline);
                LocalSession.Current = null;
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnAbout(object? sender, EventArgs e)
        {
            try
            {
                using var formAboutOnExit = new FormAbout(true);
                formAboutOnExit.ShowDialog();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnSettings(object? sender, EventArgs e)
        {
            try
            {
                using var formSettings = new FormSettings(true);
                formSettings.ShowDialog();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnExit(object? sender, EventArgs e)
        {
            try
            {
                Task.Run(() => LocalSession.Current?.Client?.Disconnect());

                LocalSession.Current?.FormHome?.Close();
                _trayIcon.Visible = false;
                LocalSession.Current = null;
                Application.Exit();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
