using NTDLS.Helpers;
using NTDLS.Persistence;
using NTDLS.ReliableMessaging;
using SecureChat.Client.Forms;
using SecureChat.Client.Helpers;
using SecureChat.Client.Models;
using SecureChat.Client.Properties;
using SecureChat.Library;
using Serilog;
using System.Diagnostics;
using static SecureChat.Library.ScConstants;

namespace SecureChat.Client
{
    internal class TrayApp : ApplicationContext
    {
        public static bool IsOnlyInstance { get; set; } = true;

        public string? DisplayName { get; private set; }
        private bool _applicationClosing = false;
        private readonly NotifyIcon _trayIcon;
        private FormLogin? _formLogin;
        private readonly System.Windows.Forms.Timer? _firstShownTimer = new();

        public TrayApp()
        {
            try
            {
                _trayIcon = new NotifyIcon
                {
                    Text = $"{ScConstants.AppName} (offline)",
                    Icon = Imaging.LoadIconFromResources(Resources.Offline16),
                    Visible = true,
                    ContextMenuStrip = new ContextMenuStrip()
                };

                _trayIcon.MouseDoubleClick += TrayIcon_MouseDoubleClick;

                _trayIcon.ContextMenuStrip.Items.Add("About", null, OnAbout);
                _trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                _trayIcon.ContextMenuStrip.Items.Add("Settings", null, OnSettings);
                _trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                _trayIcon.ContextMenuStrip.Items.Add("Login", null, OnLogin);
                _trayIcon.ContextMenuStrip.Items.Add("Exit", null, OnExit);

                _ = _trayIcon.ContextMenuStrip.Handle;

                _firstShownTimer.Interval = 250;
                _firstShownTimer.Tick += Timer_Tick;
                _firstShownTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                Log.Fatal($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            try
            {
                if (_firstShownTimer != null)
                {
                    _firstShownTimer.Enabled = false;
                    _firstShownTimer.Dispose();

                    Login();
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TrayIcon_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            try
            {
                if (ServerConnection.Current?.Connection == null)
                {
                    Login();
                }
                else
                {
                    if (ServerConnection.Current.FormHome == null)
                    {
                        throw new Exception("Home form should not be null.");
                    }
                    ServerConnection.Current.FormHome.Show();

                    if (ServerConnection.Current.FormHome.WindowState == FormWindowState.Minimized)
                    {
                        ServerConnection.Current.FormHome.WindowState = FormWindowState.Normal;
                    }

                    ServerConnection.Current.FormHome.BringToFront();
                    ServerConnection.Current.FormHome.Activate();
                    ServerConnection.Current.FormHome.Focus();
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
                ServerConnection.TerminateCurrent();

                if (_formLogin != null)
                {
                    _formLogin.Show();
                    _formLogin.BringToFront();
                    _formLogin.Activate();
                    _formLogin.Focus();
                }
                else
                {
                    LoginResult? loginResult = null;

                    var autoLogin = Exceptions.Ignore(() =>
                        LocalUserApplicationData.LoadFromDisk<AutoLogin>(ScConstants.AppName, new PersistentEncryptionProvider()));

                    if (autoLogin != null)
                    {
                        Task.Run(() =>
                        {
                            loginResult = Settings.Instance.CreateLoggedInConnection(autoLogin.Username, autoLogin.PasswordHash, RmExceptionHandler);

                            if (loginResult != null)
                            {
                                if (!Settings.Instance.Users.TryGetValue(autoLogin.Username, out var userState))
                                {
                                    Settings.Instance.Users.Add(autoLogin.Username, new PersistedUserState());
                                }
                                else
                                {
                                    userState.LastLogin = DateTime.UtcNow;
                                }

                                Settings.Save();

                                PropLocalSession(loginResult);
                            }
                        }).ContinueWith(o =>
                        {
                            if (loginResult == null)
                            {
                                using (_formLogin = new FormLogin())
                                {
                                    loginResult = _formLogin.DoLogin();
                                    if (loginResult != null)
                                    {
                                        PropLocalSession(loginResult);
                                    }
                                }
                                _formLogin = null;
                            }
                        });
                    }
                    else
                    {
                        using (_formLogin = new FormLogin())
                        {
                            loginResult = _formLogin.DoLogin();
                            if (loginResult != null)
                            {
                                PropLocalSession(loginResult);
                            }
                        }
                        _formLogin = null;
                    }

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

        public void ShowBalloon(string? title, string text, int duration = 3000)
        {
            _trayIcon.BalloonTipTitle = title ?? string.Empty;
            _trayIcon.BalloonTipText = text;
            _trayIcon.ShowBalloonTip(duration);
        }

        private void PropLocalSession(LoginResult loginResult)
        {
            loginResult.Connection.Client.OnDisconnected += RmClient_OnDisconnected;
            loginResult.Connection.Client.OnException += RmExceptionHandler;
            loginResult.Connection.Client.AddHandler(new ClientReliableMessageHandlers());

            //Yea, I am using the ContextMenuStrips thread for form creation.
            var formHome = _trayIcon.ContextMenuStrip.EnsureNotNull().Invoke(() =>
            {
                var formHome = new FormHome();
                formHome.CreateControl(); //Force the window handle to be created before the form is shown,
                _ = formHome.Handle; // Accessing the Handle property forces handle creation
                return formHome;
            });

            if (Settings.Instance.Users.TryGetValue(loginResult.Username, out var persistedUserState) == false)
            {
                persistedUserState = new();
            }

            var serverConnection = new ServerConnection(this, formHome, loginResult.Connection,
                loginResult.AccountId, loginResult.Username, loginResult.DisplayName)
            {
                Profile = loginResult.Profile,
                State = persistedUserState.ExplicitAway ? ScOnlineState.Away : ScOnlineState.Online,
                ExplicitAway = persistedUserState.ExplicitAway
            };
            DisplayName = loginResult.DisplayName;

            if (persistedUserState.ExplicitAway)
            {
                UpdateClientState(ScOnlineState.Away);
            }
            else
            {
                UpdateClientState(ScOnlineState.Online);
            }

            ServerConnection.SetCurrent(serverConnection);

            _trayIcon.BalloonTipText = $"Welcome back {loginResult.DisplayName}, you are now logged in.";
            _trayIcon.ShowBalloonTip(3000);
        }

        private void RmExceptionHandler(RmContext? context, Exception ex, IRmPayload? payload)
        {
            Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void RmClient_OnDisconnected(RmContext context)
        {
            if (_applicationClosing)
            {
                return;
            }
            try
            {
                ServerConnection.TerminateCurrent();
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
                if (_applicationClosing)
                {
                    return;
                }
                if (state == ServerConnection.Current?.State)
                {
                    return;
                }
                if (ServerConnection.Current != null)
                {
                    ServerConnection.Current.State = state;
                    ServerConnection.Current.FormHome.Repopulate();
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
                            _trayIcon.Text = $"{ScConstants.AppName} - {DisplayName} (online)";
                            _trayIcon.Icon = Imaging.LoadIconFromResources(Resources.Online16);
                            var awayItem = new ToolStripMenuItem("Away", null, OnAway)
                            {
                                Checked = false
                            };
                            _trayIcon.ContextMenuStrip.Items.Add("About", null, OnAbout);
                            _trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                            _trayIcon.ContextMenuStrip.Items.Add(awayItem);
                            _trayIcon.ContextMenuStrip.Items.Add("Profile", null, OnProfile);
                            _trayIcon.ContextMenuStrip.Items.Add("Find People", null, OnFindPeople);
                            _trayIcon.ContextMenuStrip.Items.Add("Settings", null, OnSettings);
                            _trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                            _trayIcon.ContextMenuStrip.Items.Add("Logout", null, OnLogout);
                            _trayIcon.ContextMenuStrip.Items.Add("Exit", null, OnExit);
                        }
                        break;
                    case ScOnlineState.Offline:
                        {
                            _trayIcon.Text = $"{ScConstants.AppName} (offline)";
                            _trayIcon.Icon = Imaging.LoadIconFromResources(Resources.Offline16);
                            _trayIcon.ContextMenuStrip.Items.Add("About", null, OnAbout);
                            _trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                            _trayIcon.ContextMenuStrip.Items.Add("Settings", null, OnSettings);
                            _trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                            _trayIcon.ContextMenuStrip.Items.Add("Login", null, OnLogin);
                            _trayIcon.ContextMenuStrip.Items.Add("Exit", null, OnExit);

                            _trayIcon.BalloonTipText = $"You have been disconnected.";
                            _trayIcon.ShowBalloonTip(3000);
                        }
                        break;
                    case ScOnlineState.Away:
                        {
                            _trayIcon.Text = $"{ScConstants.AppName} - {DisplayName} (away)";
                            _trayIcon.Icon = Imaging.LoadIconFromResources(Resources.Away16);
                            var awayItem = new ToolStripMenuItem("Away", null, OnAway)
                            {
                                Checked = true
                            };
                            _trayIcon.ContextMenuStrip.Items.Add("About", null, OnAbout);
                            _trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                            _trayIcon.ContextMenuStrip.Items.Add(awayItem);
                            _trayIcon.ContextMenuStrip.Items.Add("Profile", null, OnProfile);
                            _trayIcon.ContextMenuStrip.Items.Add("Settings", null, OnSettings);
                            _trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                            _trayIcon.ContextMenuStrip.Items.Add("Logout", null, OnLogout);
                            _trayIcon.ContextMenuStrip.Items.Add("Exit", null, OnExit);
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                //MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnAway(object? sender, EventArgs e)
        {
            try
            {
                if (ServerConnection.Current != null && sender is ToolStripMenuItem menuItem)
                {
                    ServerConnection.Current.ExplicitAway = !ServerConnection.Current.ExplicitAway;

                    menuItem.Checked = ServerConnection.Current.ExplicitAway; //Toggle the explicit away state.

                    UpdateClientState(menuItem.Checked ? ScOnlineState.Away : ScOnlineState.Online);

                    if (Settings.Instance.Users.TryGetValue(ServerConnection.Current.Username, out var persistedUserState) == false)
                    {
                        //Add a default state if its not already present.
                        persistedUserState = new();
                        Settings.Instance.Users.Add(ServerConnection.Current.Username, persistedUserState);
                    }

                    persistedUserState.ExplicitAway = ServerConnection.Current.ExplicitAway;
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

        private void OnProfile(object? sender, EventArgs e)
        {
            try
            {
                using var formProfile = new FormProfile(true);
                if (formProfile.ShowDialog() == DialogResult.OK)
                {
                    ServerConnection.Current?.FormHome.Repopulate();
                }
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
                Task.Run(() => ServerConnection.Current?.Connection?.Client.Disconnect());
                Thread.Sleep(10);
                UpdateClientState(ScOnlineState.Offline);
                ServerConnection.TerminateCurrent();
                Exceptions.Ignore(() => LocalUserApplicationData.DeleteFromDisk(ScConstants.AppName, typeof(AutoLogin)));
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

        private void OnFindPeople(object? sender, EventArgs e)
        {
            try
            {
                using var form = new FormFindPeople();
                form.ShowDialog();
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
            _applicationClosing = true;

            try
            {
                Exceptions.Ignore(() => _formLogin?.Close());

                _trayIcon.Visible = false;
                ServerConnection.TerminateCurrent();
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
