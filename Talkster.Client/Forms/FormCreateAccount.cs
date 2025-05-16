using Krypton.Toolkit;
using NTDLS.Helpers;
using NTDLS.ReliableMessaging;
using NTDLS.WinFormsHelpers;
using Talkster.Client.Helpers;
using Talkster.Library;
using Talkster.Library.ReliableMessages;
using System.Diagnostics;

namespace Talkster.Client.Forms
{
    public partial class FormCreateAccount : KryptonForm
    {
        private string _username = string.Empty;

        public FormCreateAccount()
        {
            InitializeComponent();

            BackColor = KryptonManager.CurrentGlobalPalette.GetBackColor1(PaletteBackStyle.PanelClient, PaletteState.Normal);

            FormClosing += FormCreateAccount_FormClosing;
            AcceptButton = buttonCreate;
            CancelButton = buttonCancel;
        }

        internal string? CreateAccount()
        {
            if (ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(_username))
            {
                return _username;
            }
            return null;
        }

        private void FormCreateAccount_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (string.IsNullOrEmpty(_username))
            {
                if (MessageBox.Show("Are you sure you want to cancel.",
                    ScConstants.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.InvokeClose(DialogResult.Cancel);
        }

        private void ButtonCreate_Click(object sender, EventArgs e)
        {
            try
            {
                var username = textBoxUsername.TextBox.GetAndValidateText("A username is required.");
                var displayName = textBoxDisplayName.TextBox.GetAndValidateText("A display name is required.");
                var password = textBoxPassword.TextBox.GetAndValidateText("A password is required.");
                var confirmPassword = textBoxPassword.TextBox.GetAndValidateText("A confirm password is required.");

                if (!Crypto.IsPasswordComplex(password, out var errorMessage))
                {
                    throw new Exception(errorMessage);
                }

                if (password != confirmPassword)
                {
                    throw new Exception("Passwords do not match.");
                }

                var passwordHash = Crypto.ComputeSha256Hash(password);
                using var progressForm = new ThemedProgressForm(ScConstants.AppName, "Please wait...");

                progressForm.Execute(() =>
                {
                    try
                    {
                        progressForm.SetHeaderText("Negotiating cryptography...");

                        var connection = ConnectionHelpers.CreateEncryptedConnection(RmExceptionHandler, progressForm);

                        try
                        {
                            progressForm.SetHeaderText("Creating account...");
                            Thread.Sleep(250); //For aesthetics.

                            var isSuccess = connection.Client.Query(new CreateAccountQuery(username, displayName, passwordHash)).ContinueWith(o =>
                            {
                                if (string.IsNullOrEmpty(o.Result.ErrorMessage) == false)
                                {
                                    throw new Exception(o.Result.ErrorMessage);
                                }

                                return !o.IsFaulted && o.Result.IsSuccess;
                            }).Result;

                            _username = isSuccess ? username : string.Empty;

                            this.InvokeClose(isSuccess ? DialogResult.OK : DialogResult.Cancel);
                        }
                        finally
                        {
                            connection.Client.Disconnect();
                        }
                    }
                    catch (Exception ex)
                    {
                        progressForm.MessageBox(ex.GetBaseException().Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                });
            }
            catch (Exception ex)
            {
                Program.Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RmExceptionHandler(RmContext? context, Exception ex, IRmPayload? payload)
        {
            Program.Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
