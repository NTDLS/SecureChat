using NTDLS.Helpers;
using System.ComponentModel;

namespace SecureChat.Client.Controls
{
    internal partial class FlowControlFileTransmissionSendProgress
        : UserControl
    {
        private readonly FlowLayoutPanel _parent;
        private readonly ActiveChat _activeChat;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FileOutboundTransfer Transfer { get; private set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsCancelled { get; private set; }

        public FlowControlFileTransmissionSendProgress(FlowLayoutPanel parent, ActiveChat activeChat, string fileName, long fileSize, Stream stream)
        {
            Transfer = new FileOutboundTransfer(fileName, fileSize, stream);

            _activeChat = activeChat;
            _parent = parent;
            InitializeComponent();

            var fileNameOnly = Path.GetFileName(Transfer.FileName);

            if (Transfer.IsImage)
            {
                //We will just start transferring images.
                labelWaitingStatus.Visible = false;
                progressBarCompletion.Visible = true;
            }
            else
            {
                //If this is not an image, then we are waiting on the remote client to accept it.
                labelWaitingStatus.Visible = true;
                progressBarCompletion.Visible = false;
            }

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

            if (progressBarCompletion.Visible == false)
            {
                //We are no longer waiting on the remote client to accept the file.
                labelWaitingStatus.Visible = false;
                progressBarCompletion.Visible = true;
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