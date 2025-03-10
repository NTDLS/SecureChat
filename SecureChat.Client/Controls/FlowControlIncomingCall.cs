﻿namespace SecureChat.Client.Controls
{
    public partial class FlowControlIncomingCall : UserControl
    {
        private readonly FlowLayoutPanel _parent;

        public FlowControlIncomingCall(FlowLayoutPanel parent, string fromName)
        {
            _parent = parent;
            InitializeComponent();

            labelIncomingCallFrom.Text = $"Incoming call from {fromName}...";

            MouseClick += Control_MouseClick;
        }

        private void NuttonAccept_Click(object sender, EventArgs e)
        {
        }

        private void ButtonDecline_Click(object sender, EventArgs e)
        {
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
