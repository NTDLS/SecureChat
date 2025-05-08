using NTDLS.Helpers;
using System.ComponentModel;

namespace SecureChat.Client.Controls
{
    internal partial class FlowControlFileTransmissionProgress
        : UserControl, IFileTransmissionControl
    {
        private readonly FlowLayoutPanel _parent;
        private readonly ActiveChat _activeChat;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FileOutboundTransfer Transfer { get; private set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsCancelled { get; private set; }

        public FlowControlFileTransmissionProgress(FlowLayoutPanel parent, ActiveChat activeChat, string fileName, long fileSize, Stream stream)
        {
            Transfer = new FileOutboundTransfer(fileName, fileSize, stream);

            _activeChat = activeChat;
            _parent = parent;
            InitializeComponent();

            var fileNameOnly = Path.GetFileName(Transfer.FileName);

            labelHeaderText.Text = $"{Formatters.FileSize(Transfer.FileSize)} {fileNameOnly}";
        }

        private void ButtonDecline_Click(object sender, EventArgs e)
        {
            buttonCancel.Enabled = false;
            IsCancelled = true;
            _activeChat.CancelFileTransmission(Transfer.FileId);
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

            //Close the stream (file handle or memory stream).
            Exceptions.Ignore(() => Transfer.Dispose());
        }
    }
}