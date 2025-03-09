using NTDLS.WinFormsHelpers;

namespace SecureChat.Client.Forms
{
    public partial class FormIncomingCall : Form
    {
        public FormIncomingCall()
        {
            InitializeComponent();
        }

        private void ButtonAccept_Click(object sender, EventArgs e)
        {
            this.InvokeClose(DialogResult.OK);
        }

        private void ButtonDecline_Click(object sender, EventArgs e)
        {
            this.InvokeClose(DialogResult.Cancel);
        }
    }
}
