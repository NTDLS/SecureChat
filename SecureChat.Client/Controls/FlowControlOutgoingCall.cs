namespace SecureChat.Client.Controls
{
    internal partial class FlowControlOutgoingCall : UserControl
    {
        private readonly FlowLayoutPanel _parent;
        private readonly ActiveChat _activeChat;

        public FlowControlOutgoingCall(FlowLayoutPanel parent, ActiveChat activeChat, string toName)
        {
            _activeChat = activeChat;
            _parent = parent;
            InitializeComponent();

            labelOutgoingCallTo.Text = $"Outgoing call to {toName}...";

            MouseClick += Control_MouseClick;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            buttonCancel.Enabled = false;
            _activeChat.CancelVoiceCallRequest();
        }

        private void Control_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add("Copy", null, (a, b) => OnCopy(sender, new EventArgs()));
                contextMenu.Items.Add(new ToolStripSeparator());
                contextMenu.Items.Add("Remove", null, OnRemove);
                contextMenu.Show(sender as Control ?? this, e.Location);
            }
        }

        private void OnRemove(object? sender, EventArgs e)
        {
            try
            {
                _parent.Controls.Remove(this);
            }
            catch
            {
            }
        }

        private void OnCopy(object? sender, EventArgs e)
        {
            try
            {
                if (sender is LinkLabel linkLabel)
                {
                    Clipboard.SetText(linkLabel.Text);
                }
            }
            catch
            {
            }
        }
    }
}
