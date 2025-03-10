using NTDLS.WinFormsHelpers;
using SecureChat.Library;
using SecureChat.Library.Models;
using SecureChat.Library.ReliableMessages;
using Serilog;
using System.Diagnostics;

namespace SecureChat.Client.Forms
{
    public partial class FormProfile : Form
    {
        public FormProfile(bool showInTaskbar)
        {
            InitializeComponent();

            if (LocalSession.Current == null || !LocalSession.Current.ReliableClient.IsConnected)
            {
                return;
            }

            if (showInTaskbar)
            {
                ShowInTaskbar = true;
                StartPosition = FormStartPosition.CenterScreen;
                TopMost = true;
            }
            else
            {
                ShowInTaskbar = false;
                StartPosition = FormStartPosition.CenterParent;
                TopMost = false;
            }

            textBoxDisplayName.Text = LocalSession.Current.DisplayName;
            textBoxTagline.Text = LocalSession.Current.Profile.Tagline;
            textBoxBiography.Text = LocalSession.Current.Profile.Biography;
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (LocalSession.Current == null || !LocalSession.Current.ReliableClient.IsConnected)
                {
                    MessageBox.Show("Connection to the server was lost.", ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.InvokeClose(DialogResult.Cancel);
                    return;
                }

                var displayName = textBoxDisplayName.GetAndValidateText("A display is required.");

                var profile = new AccountProfileModel
                {
                    Tagline = textBoxTagline.GetAndValidateText(0, 100, "If a tagline is supplied, it must not exceed [max] characters."),
                    Biography = textBoxBiography.GetAndValidateText(0, 2500, "If a biography is supplied, it must not exceed [max] characters.")
                };

                LocalSession.Current.ReliableClient.Query(new UpdateAccountProfileQuery(displayName, profile)).ContinueWith(o =>
                {
                    if (!o.IsFaulted && o.Result.IsSuccess)
                    {
                        LocalSession.Current.DisplayName = displayName;
                        LocalSession.Current.Profile = profile;

                        this.InvokeClose(DialogResult.OK);
                    }
                    else
                    {
                        throw new Exception("Failed to update profile.");
                    }
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.InvokeClose(DialogResult.Cancel);
        }
    }
}
