using Krypton.Toolkit;
using NTDLS.Helpers;
using NTDLS.Persistence;
using NTDLS.ReliableMessaging;
using NTDLS.WinFormsHelpers;
using SecureChat.Client.Models;
using SecureChat.Library;
using SecureChat.Library.ReliableMessages;
using Serilog;
using System.Diagnostics;
using System.Reflection;

namespace SecureChat.Client.Forms
{
    public partial class FormLogin : KryptonForm
    {
        private LoginResult? _loginResult;

        public FormLogin()
        {
            InitializeComponent();

            BackColor = KryptonManager.CurrentGlobalPalette.GetBackColor1(PaletteBackStyle.PanelClient, PaletteState.Normal);

            AcceptButton = buttonLogin;
            CancelButton = buttonCancel;

#if DEBUG
            if (TrayApp.IsOnlyInstance)
            {
                textBoxUsername.Text = "Adrian";
            }
            else
            {
                textBoxUsername.Text = "George";
            }
            textBoxPassword.Text = "MyPassword123!";
#else
            var lastUser = Settings.Instance.Users.OrderByDescending(o => o.Value.LastLogin).FirstOrDefault();
            if(lastUser.Value != null)
            {
                textBoxUsername.Text = lastUser.Key;
            }
#endif

            Shown += (object? sender, EventArgs e) =>
            {
                if (textBoxUsername.Text.Length > 0)
                {
                    textBoxPassword.Focus();
                }
            };
        }

        /// <summary>
        /// Prompts the user for login credentials and returns NULL on cancel or a connected reliable messaging client on success.
        /// </summary>
        internal LoginResult? DoLogin()
        {
            if (ShowDialog() == DialogResult.OK && _loginResult != null)
            {
                return _loginResult;
            }
            return null;
        }

        private void ButtonLogin_Click(object sender, EventArgs e)
        {
            _loginResult = null;

            try
            {
                var username = textBoxUsername.TextBox.GetAndValidateText("A username is required.");
                var passwordHash = Crypto.ComputeSha256Hash(textBoxPassword.Text);
                var progressForm = new ProgressForm(ScConstants.AppName, "Please wait...", (Form f) => Themes.ApplyDarkTheme(f));

                progressForm.Execute(() =>
                {
                    try
                    {
                        var rmClient = Settings.Instance.CreateEncryptedRmClient(Client_OnException, progressForm);

                        bool explicitAway = false;
                        if (Settings.Instance.Users.TryGetValue(username, out var userPersist))
                        {
                            //If the user has an explicit away state, send it to the server at
                            //  login so the server can update the user's status appropriately.
                            explicitAway = userPersist.ExplicitAway;
                        }

                        progressForm.SetHeaderText("Logging in...");
                        Thread.Sleep(250); //For aesthetics.

                        var isSuccess = rmClient.Query(new LoginQuery(username, passwordHash, explicitAway)).ContinueWith(o =>
                        {
                            if (string.IsNullOrEmpty(o.Result.ErrorMessage) == false)
                            {
                                throw new Exception(o.Result.ErrorMessage);
                            }

                            if (!o.IsFaulted && o.Result.IsSuccess)
                            {
                                _loginResult = new LoginResult(rmClient,
                                    o.Result.AccountId.EnsureNotNull(),
                                    o.Result.Username.EnsureNotNull(),
                                    o.Result.DisplayName.EnsureNotNull(),
                                    o.Result.ProfileJson.EnsureNotNull()
                                    );
                                return true;
                            }

                            return false;
                        }).Result;

                        rmClient.OnException -= Client_OnException;

                        if (!isSuccess)
                        {
                            rmClient.Disconnect();
                        }
                        else
                        {
                            if (!Settings.Instance.Users.TryGetValue(username, out var userState))
                            {
                                Settings.Instance.Users.Add(username, new PersistedUserState());
                            }
                            else
                            {
                                userState.LastLogin = DateTime.UtcNow;
                            }

                            Settings.Save();

                            if (checkBoxStayLoggedIn.Checked)
                            {
                                var autoLogin = new AutoLogin(username, passwordHash);
                                LocalUserApplicationData.SaveToDisk(ScConstants.AppName, autoLogin, new PersistentEncryptionProvider());
                            }
                        }

                        this.InvokeClose(isSuccess ? DialogResult.OK : DialogResult.Cancel);
                    }
                    catch (Exception ex)
                    {
                        progressForm.MessageBox(ex.GetBaseException().Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Client_OnException(RmContext? context, Exception ex, IRmPayload? payload)
        {
            Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.InvokeClose(DialogResult.Cancel);
        }

        private void LinkLabelCreateAccount_LinkClicked(object sender, EventArgs e)
        {
            using var form = new FormCreateAccount();
            var username = form.CreateAccount();
            if (username != null)
            {
                textBoxUsername.Text = username;
                textBoxPassword.Focus();
            }
        }
    }
}
