using Krypton.Toolkit;
using NTDLS.Helpers;
using Talkster.Client.Controls.FlowControls;
using Talkster.Client.Forms;

namespace Talkster.Client.Controls
{
    internal partial class FlowControlIncomingCall
        : UserControl, IFlowControl
    {
        private readonly FlowLayoutPanel _parent;
        private readonly ActiveChat _activeChat;

        public FlowControlIncomingCall(FlowLayoutPanel parent, ActiveChat activeChat, string fromName)
        {
            InitializeComponent();

            BackColor = KryptonManager.CurrentGlobalPalette.GetBackColor1(PaletteBackStyle.PanelClient, PaletteState.Normal);

            _activeChat = activeChat;
            _parent = parent;

            labelIncomingCallFrom.Text = $"Incoming call from {fromName}...";

            MouseClick += Control_MouseClick;
        }

        private void ButtonAccept_Click(object sender, EventArgs e)
        {
            using var formVoicePreCall = new FormVoicePreCall();
            if (formVoicePreCall.ShowDialog() == DialogResult.OK)
            {
                buttonAccept.Enabled = false;
                buttonDecline.Enabled = false;

                _activeChat.AcceptVoiceCallRequest(formVoicePreCall.InputDeviceIndex, formVoicePreCall.OutputDeviceIndex, formVoicePreCall.Bitrate);

                //TODO: Setup the call... Does this not do it? I think it does.
                _activeChat.StartAudioPump();
            }
        }

        private void ButtonDecline_Click(object sender, EventArgs e)
        {
            buttonAccept.Enabled = false;
            buttonDecline.Enabled = false;
            _activeChat.DeclineVoiceCallRequest();
        }

        private void Control_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add("Copy", null, (a, b) => OnCopy(sender, new EventArgs()));
                contextMenu.Items.Add(new ToolStripSeparator());
                contextMenu.Items.Add("Remove", null, OnRemove);
                contextMenu.Show((sender as Control) ?? this, e.Location);
            }
        }

        private void OnRemove(object? sender, EventArgs e)
        {
            Remove();
        }

        public void Remove()
        {
            Exceptions.Ignore(() =>
            {
                _parent.Controls.Remove(this);
            });
        }

        private void OnCopy(object? sender, EventArgs e)
        {
            Exceptions.Ignore(() =>
            {
                if (sender is LinkLabel linkLabel)
                {
                    Clipboard.SetText(linkLabel.Text);
                }
            });
        }
    }
}
