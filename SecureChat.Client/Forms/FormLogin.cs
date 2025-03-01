using NTDLS.ReliableMessaging;
using NTDLS.WinFormsHelpers;
using SecureChat.Library;
using SecureChat.Library.Messages;

namespace SecureChat.Client.Forms
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();

            textBoxServer.Text = Constants.DefaultServerAddress;
            textBoxPort.Text = $"{Constants.DefaultServerPort:n0}";

#if DEBUG
            textBoxUsername.Text = "_nop";
            textBoxPassword.Text = "password";
#endif
        }

        private void ButtonLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var username = textBoxUsername.GetAndValidateText("A username is required.");
                var passwordHash = Crypto.ComputeSha256Hash(textBoxPassword.Text);
                var address = textBoxServer.GetAndValidateText("A server IP address or host name is required.");
                var port = textBoxPort.GetAndValidateNumeric(1, 65535, "A port number is required between [min] and [max].");

                var progressForm = new ProgressForm(Constants.AppName, "Logging in...");

                progressForm.Execute(() =>
                {
                    try
                    {
                        var client = new RmClient();
                        client.Connect(address, port);

                        var isSuccess = client.Query(new LoginQuery(username, passwordHash)).ContinueWith(o =>
                        {
                            if (string.IsNullOrEmpty(o.Result.ErrorMessage) == false)
                            {
                                throw new Exception(o.Result.ErrorMessage);
                            }
                            return o.Result.IsSuccess;
                        }).Result;

                        client.Disconnect();

                        this.InvokeClose(DialogResult.OK);
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

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.InvokeClose(DialogResult.Cancel);
        }
    }
}
