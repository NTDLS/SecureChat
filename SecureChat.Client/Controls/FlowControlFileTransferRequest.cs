using NTDLS.Helpers;
using System.ComponentModel;

namespace SecureChat.Client.Controls
{
    internal partial class FlowControlFileTransferRequest : UserControl
    {
        private readonly FlowLayoutPanel _parent;
        private readonly ActiveChat _activeChat;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Guid FileId { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string FileName { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long FileSize { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsImage { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsCancelled { get; private set; }

        public FlowControlFileTransferRequest(FlowLayoutPanel parent, ActiveChat activeChat, string fromName,
            Guid fileId, string fileName, long fileSize, bool isImage, Color color)
        {
            _activeChat = activeChat;
            _parent = parent;
            FileId = fileId;
            FileName = fileName;
            FileSize = fileSize;
            IsImage = isImage;

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
            sfd.FileName = FileName;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                buttonAccept.Enabled = false;
                buttonDecline.Enabled = false;

                Remove();

                //Add the receive control to the chat with the path of the file.
                var control = _activeChat.AppendFileTransferReceiveProgress(FileId, FileName, FileSize, IsImage, sfd.FileName);
                _activeChat.InboundFileTransfers.Add(FileId, control);
                _activeChat.AcceptFileTransfer(FileId);
            }
        }

        private void ButtonDecline_Click(object sender, EventArgs e)
        {
            Cancel();
            Remove();
            _activeChat.DeclineFileTransfer(this);
            _activeChat.AppendSystemMessageLine($"File transfer '{Path.GetFileName(FileName)}' declined.");
        }

        public void Cancel()
        {
            if (InvokeRequired)
            {
                Invoke(() => Cancel());
                return;
            }

            buttonAccept.Enabled = false;
            buttonDecline.Enabled = false;
            IsCancelled = true;
        }
        public void Remove()
        {
            _activeChat.PendingFileTransfers.Remove(FileId);

            Exceptions.Ignore(() =>
            {
                _parent.Invoke(() => _parent.Controls.Remove(this));
            });
        }
    }
}
