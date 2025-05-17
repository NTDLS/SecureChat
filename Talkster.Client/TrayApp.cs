using NTDLS.Helpers;
using NTDLS.Persistence;
using NTDLS.ReliableMessaging;
using System.Diagnostics;
using Talkster.Client.Forms;
using Talkster.Client.Helpers;
using Talkster.Client.Models;
using Talkster.Client.Properties;
using Talkster.Library;
using static Talkster.Library.ScConstants;

namespace Talkster.Client
{
    internal class TrayApp : ApplicationContext
    {
        public static bool IsOnlyInstance { get; set; } = true;
        public string? DisplayName { get; private set; }

        private bool _isApplicationClosing = false;
        private readonly NotifyIcon _trayIcon;
        private FormLogin? _formLogin;
        private readonly System.Windows.Forms.Timer? _firstShownTimer = new();
        private readonly System.Windows.Forms.Timer _reconnectTimer = new();

        /// This is used to determine if the disconnect was intentional or not so we can auto-reconnect.
        private bool _intentionalDisconnect = true;
        private bool _IsBusyLoggingIn = false;

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

                _trayIcon.MouseClick += TrayIcon_MouseClick;

                _trayIcon.ContextMenuStrip.Items.Add("About", null, OnAbout);
                _trayIcon.ContextMenuStrip.Items.Add("Log", null, OnLog);
                _trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                _trayIcon.ContextMenuStrip.Items.Add("Settings", null, OnSettings);
                _trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                _trayIcon.ContextMenuStrip.Items.Add("Login", null, OnLogin);
                _trayIcon.ContextMenuStrip.Items.Add("Exit", null, OnExit);

                _ = _trayIcon.ContextMenuStrip.Handle;

                _firstShownTimer.Interval = 250;
                _firstShownTimer.Tick += FirstShownTimer_Tick;
                _firstShownTimer.Enabled = true;

                _reconnectTimer.Interval = 10000;
                _reconnectTimer.Tick += ReconnectTimer_Tick;
            }
            catch (Exception ex)
            {
                Program.Log.Fatal($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        #region Invokers.

        public bool InvokeRequired
            => _trayIcon.ContextMenuStrip.EnsureNotNull().InvokeRequired;

        public void Invoke(Action method)
            => _trayIcon.ContextMenuStrip.EnsureNotNull().Invoke(method);

        public object Invoke(Delegate method)
             => _trayIcon.ContextMenuStrip.EnsureNotNull().Invoke(method, null);

        public object Invoke(Delegate method, params object?[]? args)
            => _trayIcon.ContextMenuStrip.EnsureNotNull().Invoke(method, args);

        public T Invoke<T>(Func<T> method)
            => _trayIcon.ContextMenuStrip.EnsureNotNull().Invoke(method);

        #endregion

        private void ReconnectTimer_Tick(object? sender, EventArgs e)
        {
            try
            {
                _reconnectTimer.Stop();

                if (_intentionalDisconnect || ServerConnection.Current?.Connection != null)
                {
                    return;
                }

                Login(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FirstShownTimer_Tick(object? sender, EventArgs e)
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
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TrayIcon_MouseClick(object? sender, MouseEventArgs e)
        {
            if (_IsBusyLoggingIn)
            {
                return;
            }
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            try
            {
                if (ServerConnection.Current?.Connection == null)
                {
                    Login();
                }
                else
                {
                    ServerConnection.Current?.ShowHomeForm();
                }
            }
            catch (Exception ex)
            {
                Program.Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Login(bool isReconnectAttempt = false)
        {
            if (_IsBusyLoggingIn)
            {
                return;
            }

            _IsBusyLoggingIn = true;
            UpdateClientState(ScOnlineState.Connecting);

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
                            loginResult = ConnectionHelpers.CreateLoggedInConnection(autoLogin.Username, autoLogin.PasswordHash, RmExceptionHandler);

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
                            _IsBusyLoggingIn = false;

                            if (loginResult == null)
                            {
                                UpdateClientState(ScOnlineState.Offline);

                                if (isReconnectAttempt)
                                {
                                    //Failed to auto-reconnect, lets try again.
                                    Invoke(() => _reconnectTimer.Start());
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
                Program.Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                if (Settings.Instance.AlertToastErrorMessages)
                {
                    Notifications.ToastError(ScConstants.AppName, $"An error has occurred during the connection process.", 4000);
                }
            }
        }

        private void PropLocalSession(LoginResult loginResult)
        {
            _intentionalDisconnect = false;

            loginResult.Connection.Client.OnDisconnected += RmClient_OnDisconnected;
            loginResult.Connection.Client.OnException += RmExceptionHandler;
            loginResult.Connection.Client.AddHandler(new ClientReliableMessageHandlers());

            //Yea, I am using the ContextMenuStrips thread for form creation.
            var formHome = Invoke(() =>
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

            if (Settings.Instance.AlertToastWhenMyOnlineStatusChanges)
            {
                Notifications.ToastPlain(ScConstants.AppName, $"Welcome back {loginResult.DisplayName}, you are now logged in.",
                    () => ServerConnection.Current?.ShowHomeForm());
            }
        }

        private void RmExceptionHandler(RmContext? context, Exception ex, IRmPayload? payload)
        {
            Program.Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            //MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void RmClient_OnDisconnected(RmContext context)
        {
            if (_isApplicationClosing)
            {
                return;
            }
            try
            {
                ServerConnection.TerminateCurrent();
                UpdateClientState(ScOnlineState.Offline);

                if (Settings.Instance.AlertToastWhenMyOnlineStatusChanges)
                {
                    Notifications.ToastPlain(ScConstants.AppName, $"You have been disconnected.");
                }

                if (!_intentionalDisconnect)
                {
                    Invoke(() => _reconnectTimer.Start());
                }

            }
            catch (Exception ex)
            {
                Program.Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void UpdateClientState(ScOnlineState state)
        {
            try
            {
                if (_isApplicationClosing)
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

                if (InvokeRequired)
                {
                    Invoke(UpdateClientState, state);
                    return;
                }

                _trayIcon.ContextMenuStrip.Items.Clear();

                switch (state)
                {
                    case ScOnlineState.Connecting:
                        {
                            _trayIcon.Text = $"{ScConstants.AppName} - connecting...";
                            _trayIcon.Icon = Imaging.LoadIconFromResources(Resources.Offline16);
                            _trayIcon.ContextMenuStrip.Items.Add("About", null, OnAbout);
                            _trayIcon.ContextMenuStrip.Items.Add("Log", null, OnLog);
                            _trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                            _trayIcon.ContextMenuStrip.Items.Add("Settings", null, OnSettings);
                            _trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                            _trayIcon.ContextMenuStrip.Items.Add("Login", null, OnLogin).Enabled = false;
                            _trayIcon.ContextMenuStrip.Items.Add("Exit", null, OnExit);
                        }
                        break;
                    case ScOnlineState.Online:
                        {
                            _trayIcon.Text = $"{ScConstants.AppName} - {DisplayName}";
                            _trayIcon.Icon = Imaging.LoadIconFromResources(Resources.Online16);
                            var awayItem = new ToolStripMenuItem("Away", null, OnAway)
                            {
                                Checked = false
                            };
                            _trayIcon.ContextMenuStrip.Items.Add("About", null, OnAbout);
                            _trayIcon.ContextMenuStrip.Items.Add("Log", null, OnLog);
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
                            _trayIcon.ContextMenuStrip.Items.Add("Log", null, OnLog);
                            _trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                            _trayIcon.ContextMenuStrip.Items.Add("Settings", null, OnSettings);
                            _trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                            _trayIcon.ContextMenuStrip.Items.Add("Login", null, OnLogin);
                            _trayIcon.ContextMenuStrip.Items.Add("Exit", null, OnExit);
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
                            _trayIcon.ContextMenuStrip.Items.Add("Log", null, OnLog);
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
                Program.Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
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
                Program.Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnLogin(object? sender, EventArgs e)
        {
            try
            {
                //The user is trying to manually login, so we need to stop the reconnect timer.
                _intentionalDisconnect = true;
                Login();
            }
            catch (Exception ex)
            {
                Program.Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
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
                Program.Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnLogout(object? sender, EventArgs e)
        {
            try
            {
                _intentionalDisconnect = true;

                Task.Run(() => ServerConnection.Current?.Connection?.Client.Disconnect());
                Thread.Sleep(10);
                UpdateClientState(ScOnlineState.Offline);
                ServerConnection.TerminateCurrent();
                Exceptions.Ignore(() => LocalUserApplicationData.DeleteFromDisk(ScConstants.AppName, typeof(AutoLogin)));
            }
            catch (Exception ex)
            {
                Program.Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnLog(object? sender, EventArgs e)
        {
            try
            {
                using var form = new FormLog();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                Program.Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
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
                Program.Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
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
                Program.Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
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
                Program.Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnExit(object? sender, EventArgs e)
        {
            _isApplicationClosing = true;
            _intentionalDisconnect = true;

            try
            {
                Exceptions.Ignore(() => _formLogin?.Close());

                _trayIcon.Visible = false;
                ServerConnection.TerminateCurrent();
                Application.Exit();
            }
            catch (Exception ex)
            {
                Program.Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
