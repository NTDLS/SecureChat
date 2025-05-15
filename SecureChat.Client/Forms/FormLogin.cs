using Krypton.Toolkit;
using NTDLS.Persistence;
using NTDLS.ReliableMessaging;
using NTDLS.WinFormsHelpers;
using SecureChat.Client.Helpers;
using SecureChat.Client.Models;
using SecureChat.Library;
using Serilog;
using System.Diagnostics;

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
            textBoxPassword.Text = "Password123!";
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
                using var progressForm = new ThemedProgressForm(ScConstants.AppName, "Please wait...");

                progressForm.Execute(() =>
                {
                    try
                    {
                        _loginResult = Settings.Instance.CreateLoggedInConnection(username, passwordHash, RmExceptionHandler, progressForm);
                        if (_loginResult != null)
                        {
                            if (checkBoxStayLoggedIn.Checked)
                            {
                                var autoLogin = new AutoLogin(username, passwordHash);
                                LocalUserApplicationData.SaveToDisk(ScConstants.AppName, autoLogin, new PersistentEncryptionProvider());
                            }
                        }

                        this.InvokeClose(_loginResult != null ? DialogResult.OK : DialogResult.Cancel);
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

        private void RmExceptionHandler(RmContext? context, Exception ex, IRmPayload? payload)
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

        private void ButtonSettings_Click(object sender, EventArgs e)
        {
            using var form = new FormSettings(false);
            form.ShowDialog();
        }
    }
}
