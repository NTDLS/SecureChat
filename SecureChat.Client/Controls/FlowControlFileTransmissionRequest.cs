using NTDLS.Helpers;

namespace SecureChat.Client.Controls
{
    internal partial class FlowControlFileTransmissionRequest : UserControl
    {
        private readonly FlowLayoutPanel _parent;
        private readonly ActiveChat _activeChat;
        private readonly Guid _fileId;
        private readonly string _fileName;
        private readonly long _fileSize;
        private readonly bool _isImage;

        public string FileName => _fileName;

        public FlowControlFileTransmissionRequest(FlowLayoutPanel parent, ActiveChat activeChat, string fromName,
            Guid fileId, string fileName, long fileSize, bool isImage, Color color)
        {
            _activeChat = activeChat;
            _parent = parent;
            _fileId = fileId;
            _fileName = fileName;
            _fileSize = fileSize;
            _isImage = isImage;

            InitializeComponent();

            labelHeader.Text = $"{fromName} is sending you a {Formatters.FileSize(fileSize)} file.";
            labelFileName.Text = fileName;
        }

        private void ButtonAccept_Click(object sender, EventArgs e)
        {
            var ext = Path.GetExtension(labelFileName.Text).Trim('.');

            using var sfd = new SaveFileDialog();
            sfd.Filter = $"{ext} Files|*.{ext}| All Files (*.*)|*.*";
            sfd.Title = "Save Attachment As";
            sfd.FileName = _fileName;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                buttonAccept.Enabled = false;
                buttonDecline.Enabled = false;

                _activeChat.FileReceiveBuffers.Add(_fileId, new FileReceiveBuffer(_fileId, _fileName, _fileSize, _isImage, sfd.FileName));
                _activeChat.AcceptFileTransmission(_fileId);
            }
        }

        private void ButtonDecline_Click(object sender, EventArgs e)
        {
            buttonAccept.Enabled = false;
            buttonDecline.Enabled = false;
            _activeChat.DeclineFileTransmission(this);
        }
    }
}
