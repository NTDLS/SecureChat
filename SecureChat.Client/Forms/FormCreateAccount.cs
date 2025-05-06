using NTDLS.Helpers;
using NTDLS.ReliableMessaging;
using NTDLS.WinFormsHelpers;
using SecureChat.Library;
using SecureChat.Library.ReliableMessages;
using Serilog;
using System.Diagnostics;
using System.Reflection;

namespace SecureChat.Client.Forms
{
    public partial class FormCreateAccount : Form
    {
        private string _username = string.Empty;

        public FormCreateAccount()
        {
            InitializeComponent();

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
                    ScConstants.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
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
                var username = textBoxUsername.GetAndValidateText("A username is required.");
                var displayName = textBoxDisplayName.GetAndValidateText("A display name is required.");
                var password = textBoxPassword.GetAndValidateText("A password is required.");
                var confirmPassword = textBoxPassword.GetAndValidateText("A confirm password is required.");

                if (!Crypto.IsPasswordComplex(password, out var errorMessage))
                {
                    throw new Exception(errorMessage);
                }

                if (password != confirmPassword)
                {
                    throw new Exception("Passwords do not match.");
                }

                var passwordHash = Crypto.ComputeSha256Hash(password);
                var progressForm = new ProgressForm(ScConstants.AppName, "Please wait...");

                progressForm.Execute(() =>
                {
                    try
                    {
                        progressForm.SetHeaderText("Negotiating cryptography...");

                        var keyPair = Crypto.GeneratePublicPrivateKeyPair();
                        var client = Settings.Instance.CreateRmClient();
                        client.OnException += Client_OnException;

                        var appVersion = (Assembly.GetEntryAssembly()?.GetName().Version).EnsureNotNull();

                        //Send our public key to the server and wait on a reply of their public key.
                        var remotePublicKey = client.Query(new ExchangePublicKeyQuery(client.ConnectionId.EnsureNotNull(), appVersion, keyPair.PublicRsaKey))
                            .ContinueWith(o =>
                            {
                                if (o.IsFaulted || !o.Result.IsSuccess)
                                {
                                    throw new Exception(string.IsNullOrEmpty(o.Result.ErrorMessage) ? "Unknown negotiation error." : o.Result.ErrorMessage);
                                }

                                return o.Result.PublicRsaKey;
                            }).Result;

                        progressForm.SetHeaderText("Applying cryptography...");

                        client.Notify(new InitializeServerClientCryptographyNotification());
                        client.SetCryptographyProvider(new ReliableCryptographyProvider(remotePublicKey, keyPair.PrivateRsaKey));

                        progressForm.SetHeaderText("Waiting for server...");
                        Thread.Sleep(1000); //Give the server a moment to initialize the cryptography.

                        progressForm.SetHeaderText("Creating account...");
                        Thread.Sleep(250); //For aesthetics.

                        var isSuccess = client.Query(new CreateAccountQuery(username, displayName, passwordHash)).ContinueWith(o =>
                        {
                            if (string.IsNullOrEmpty(o.Result.ErrorMessage) == false)
                            {
                                throw new Exception(o.Result.ErrorMessage);
                            }

                            return !o.IsFaulted && o.Result.IsSuccess;
                        }).Result;

                        client.Disconnect();

                        _username = isSuccess ? username : string.Empty;

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
    }
}
