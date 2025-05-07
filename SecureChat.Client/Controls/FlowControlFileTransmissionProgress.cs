using NTDLS.Helpers;
using System.ComponentModel;

namespace SecureChat.Client.Controls
{
    internal partial class FlowControlFileTransmissionProgress : UserControl
    {
        private readonly FlowLayoutPanel _parent;
        private readonly ActiveChat _activeChat;
        private readonly Guid _fileId;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsCancelled { get; private set; }

        public FlowControlFileTransmissionProgress(FlowLayoutPanel parent, ActiveChat activeChat, Guid fileId, string fileName, long fileSize)
        {
            _activeChat = activeChat;
            _fileId = fileId;
            _parent = parent;
            InitializeComponent();

            var fileNameOnly = Path.GetFileName(fileName);

            labelHeaderText.Text = $"{Formatters.FileSize(fileSize)} {fileNameOnly}";
        }

        private void ButtonDecline_Click(object sender, EventArgs e)
        {
            buttonCancel.Enabled = false;
            IsCancelled = true;
            _activeChat.CancelFileTransmission(_fileId);
        }

        public void SetProgressValue(int value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SetProgressValue(value)));
                return;
            }

            if (value < 0)
            {
                value = 0;
            }
            else if (value > 100)
            {
                value = 100;
            }

            progressBarCompletion.Value = value;
        }

        public void Remove()
        {
            Exceptions.Ignore(() =>
            {
                _parent.Invoke(() => _parent.Controls.Remove(this));
            });
        }
    }
}
