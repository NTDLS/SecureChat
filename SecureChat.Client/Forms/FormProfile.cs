namespace SecureChat.Client.Forms
{
    public partial class FormProfile : Form
    {
        public FormProfile(bool showInTaskbar)
        {
            InitializeComponent();

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
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
        }
    }
}
