namespace SecureChat.Client.Forms
{
    public partial class FormBackground : Form
    {
        public FormBackground()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.TopMost = false;
        }
    }
}
