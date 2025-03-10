﻿using SecureChat.Client.Forms;

namespace SecureChat.Client.Controls
{
    internal partial class FlowControlIncomingCall : UserControl
    {
        private readonly FlowLayoutPanel _parent;
        private readonly ActiveChat _activeChat;

        public FlowControlIncomingCall(FlowLayoutPanel parent, ActiveChat activeChat, string fromName)
        {
            _activeChat = activeChat;
            _parent = parent;
            InitializeComponent();

            labelIncomingCallFrom.Text = $"Incoming call from {fromName}...";

            MouseClick += Control_MouseClick;
        }

        private void ButtonAccept_Click(object sender, EventArgs e)
        {
            using var formVoicePreCall = new FormVoicePreCall();
            if (formVoicePreCall.ShowDialog() == DialogResult.OK && formVoicePreCall.InputDeviceIndex != null && formVoicePreCall.OutputDeviceIndex != null)
            {
                buttonAccept.Enabled = false;
                buttonDecline.Enabled = false;

                //TODO: Setup the call...
                _activeChat.AcceptVoiceCallRequest();
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
