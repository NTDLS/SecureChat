using NTDLS.Persistence;
using NTDLS.ReliableMessaging;
using NTDLS.WinFormsHelpers;
using SecureChat.Library;
using SecureChat.Library.ReliableMessages;
using Serilog;
using System.Diagnostics;

namespace SecureChat.Client.Forms
{
    public partial class FormCreateAccount : Form
    {
        private string _username = string.Empty;

        public FormCreateAccount()
        {
            InitializeComponent();

            FormClosing += FormCreateAccount_FormClosing;
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
            if (MessageBox.Show("Are you sure you want to cancel.",
                ScConstants.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.InvokeClose(DialogResult.Cancel);
            }
        }

        private void ButtonCreate_Click(object sender, EventArgs e)
        {
            try
            {
                var settings = LocalUserApplicationData.LoadFromDisk(ScConstants.AppName, new PersistedSettings());

                var username = textBoxUsername.GetAndValidateText("A username is required.");
                var displayName = textBoxDisplayName.GetAndValidateText("A display name is required.");
                var password = textBoxPassword.GetAndValidateText("A password is required.");
                var confirmPassword = textBoxPassword.GetAndValidateText("A confirm password is required.");

                if (password != confirmPassword)
                {
                    throw new Exception("Passwords do not match.");
                }

                var passwordHash = Crypto.ComputeSha256Hash(password);

                var progressForm = new ProgressForm(ScConstants.AppName, "Creating account...");

                progressForm.Execute(() =>
                {
                    try
                    {
                        var keyPair = Crypto.GeneratePublicPrivateKeyPair();
                        var client = new RmClient();
                        client.OnException += Client_OnException;
                        client.Connect(settings.ServerAddress, settings.ServerPort);

                        //Send our public key to the server and wait on a reply of their public key.
                        var remotePublicKey = client.Query(new ExchangePublicKeyQuery(keyPair.PublicRsaKey))
                            .ContinueWith(o => o.Result.PublicRsaKey).Result;

                        client.Notify(new InitializeServerClientCryptography());
                        client.SetCryptographyProvider(new ServerClientCryptographyProvider(remotePublicKey, keyPair.PrivateRsaKey));

                        Thread.Sleep(1000); //Give the server a moment to initialize the cryptography.

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
