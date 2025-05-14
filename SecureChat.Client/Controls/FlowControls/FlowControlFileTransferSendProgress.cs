using Krypton.Toolkit;
using NTDLS.Helpers;
using SecureChat.Client.Controls.FlowControls;
using System.ComponentModel;

namespace SecureChat.Client.Controls
{
    internal partial class FlowControlFileTransferSendProgress
        : UserControl, IFlowControl
    {
        private readonly FlowLayoutPanel _parent;
        private readonly ActiveChat _activeChat;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FileOutboundTransfer Transfer { get; private set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsCancelled { get; private set; }

        public FlowControlFileTransferSendProgress(FlowLayoutPanel parent, ActiveChat activeChat, string fileName, long fileSize, Stream stream)
        {
            InitializeComponent();

            BackColor = KryptonManager.CurrentGlobalPalette.GetBackColor1(PaletteBackStyle.PanelClient, PaletteState.Normal);

            Transfer = new FileOutboundTransfer(fileName, fileSize, stream);

            _activeChat = activeChat;
            _parent = parent;

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

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Cancel();
            Remove();
            _activeChat.CancelFileTransfer(Transfer.FileId);
            _activeChat.AppendSystemMessageLine($"File transfer cancelled: {Path.GetFileName(Transfer.FileName)}");
        }

        public void Cancel()
        {
            if (InvokeRequired)
            {
                Invoke(() => Cancel());
                return;
            }

            buttonCancel.Enabled = false;
            IsCancelled = true;
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
            Exceptions.Ignore(() => _parent.Invoke(() => _parent.Controls.Remove(this)));

            //Close the stream (file handle or memory stream).
            Exceptions.Ignore(() => Transfer.Dispose());
        }
    }
}