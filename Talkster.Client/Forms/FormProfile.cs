using Krypton.Toolkit;
using NTDLS.WinFormsHelpers;
using System.Diagnostics;
using Talkster.Library;
using Talkster.Library.Models;
using Talkster.Library.ReliableMessages;

namespace Talkster.Client.Forms
{
    public partial class FormProfile : KryptonForm
    {
        public FormProfile(bool showInTaskbar)
        {
            InitializeComponent();

            BackColor = KryptonManager.CurrentGlobalPalette.GetBackColor1(PaletteBackStyle.PanelClient, PaletteState.Normal);

            if (ServerConnection.Current?.Connection.Client.IsConnected != true)
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

            textBoxDisplayName.Text = ServerConnection.Current.DisplayName;
            textBoxTagline.Text = ServerConnection.Current.Profile.Tagline;
            textBoxBiography.Text = ServerConnection.Current.Profile.Biography;
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ServerConnection.Current?.Connection.Client.IsConnected != true)
                {
                    MessageBox.Show("Connection to the server was lost.", ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.InvokeClose(DialogResult.Cancel);
                    return;
                }

                var displayName = textBoxDisplayName.TextBox.GetAndValidateText("A display is required.");

                var profile = new AccountProfileModel
                {
                    Tagline = textBoxTagline.TextBox.GetAndValidateText(0, 100, "If a tagline is supplied, it must not exceed [max] characters."),
                    Biography = textBoxBiography.TextBox.GetAndValidateText(0, 2500, "If a biography is supplied, it must not exceed [max] characters.")
                };

                ServerConnection.Current.Connection.Client.Query(new UpdateAccountProfileQuery(displayName, profile)).ContinueWith(o =>
                {
                    if (!o.IsFaulted && o.Result.IsSuccess)
                    {
                        ServerConnection.Current.DisplayName = displayName;
                        ServerConnection.Current.Profile = profile;

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
                Program.Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.InvokeClose(DialogResult.Cancel);
        }
    }
}
