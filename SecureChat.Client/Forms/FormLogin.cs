using NTDLS.Helpers;
using NTDLS.Persistence;
using NTDLS.ReliableMessaging;
using NTDLS.WinFormsHelpers;
using SecureChat.Client.Models;
using SecureChat.Library;
using SecureChat.Library.ReliableMessages;
using Serilog;

namespace SecureChat.Client.Forms
{
    public partial class FormLogin : Form
    {
        private LoginResult? _loginResult;

        public FormLogin()
        {
            InitializeComponent();

            textBoxServer.Text = Constants.DefaultServerAddress;
            textBoxPort.Text = $"{Constants.DefaultServerPort:n0}";

            AcceptButton = buttonLogin;
            CancelButton = buttonCancel;

#if DEBUG
            textBoxUsername.Text = "_nop";
            textBoxPassword.Text = "password";
#endif
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
                var username = textBoxUsername.GetAndValidateText("A username is required.");
                var passwordHash = Crypto.ComputeSha256Hash(textBoxPassword.Text);
                var serverAddress = textBoxServer.GetAndValidateText("A server IP address or host name is required.");
                var serverPort = textBoxPort.GetAndValidateNumeric(1, 65535, "A port number is required between [min] and [max].");

                var progressForm = new ProgressForm(Constants.AppName, "Logging in...");

                progressForm.Execute(() =>
                {
                    try
                    {
                        var keyPair = Crypto.GeneratePublicPrivateKeyPair();

                        var clientConfiguration = new RmConfiguration()
                        {
                            //We leave asynchronous notifications disabled for the sake or initializing cryptgraphy.
                            AsynchronousNotifications = false
                        };
                        var client = new RmClient(clientConfiguration);
                        client.Connect(serverAddress, serverPort);

                        client.OnException += Client_OnException;

                        //Send our public key to the server and wait on a reply of their public key.
                        var remotePublicKey = client.Query(new ExchangePublicKeyQuery(keyPair.PublicRsaKey)).ContinueWith(o =>
                        {
                            return o.Result.PublicRsaKey;
                        }).Result;

                        client.Notify(new InitializeServerClientCryptography());
                        client.SetCryptographyProvider(new ServerClientCryptographyProvider(remotePublicKey, keyPair.PrivateRsaKey));

                        var isSuccess = client.Query(new LoginQuery(username, passwordHash)).ContinueWith(o =>
                        {
                            if (string.IsNullOrEmpty(o.Result.ErrorMessage) == false)
                            {
                                throw new Exception(o.Result.ErrorMessage);
                            }

                            if (o.Result.IsSuccess)
                            {
                                _loginResult = new LoginResult(client, o.Result.Username.EnsureNotNull(), o.Result.DisplayName.EnsureNotNull());
                            }

                            return o.Result.IsSuccess;
                        }).Result;

                        client.OnException -= Client_OnException;

                        if (!isSuccess)
                        {
                            client.Disconnect();
                        }
                        else
                        {
                            client.Configuration.AsynchronousNotifications = true;

                            var persisted = LocalUserApplicationData.LoadFromDisk(Constants.AppName, new PersistedState());

                            if (persisted.Users.ContainsKey(username) == false)
                            {
                                persisted.Users.Add(username,
                                new PersistedUserState()
                                {
                                    ExplicitAway = false
                                });
                            }

                            LocalUserApplicationData.SaveToDisk(Constants.AppName, persisted);
                        }

                        this.InvokeClose(isSuccess ? DialogResult.OK : DialogResult.Cancel);
                    }
                    catch (Exception ex)
                    {
                        progressForm.MessageBox(ex.GetBaseException().Message, Constants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void Client_OnException(RmContext? context, Exception ex, IRmPayload? payload)
        {
            Log.Error("Reliable messaging exception.", ex);
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.InvokeClose(DialogResult.Cancel);
        }
    }
}
