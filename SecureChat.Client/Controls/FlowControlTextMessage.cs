using SecureChat.Client.Helpers;

namespace SecureChat.Client.Controls
{
    public class FlowControlTextMessage : FlowLayoutPanel
    {
        private readonly FlowLayoutPanel _parent;
        private readonly Label _labelMessage;

        public FlowControlTextMessage(FlowLayoutPanel parent, string displayName, string message, Color? color)
        {
            _parent = parent;
            FlowDirection = FlowDirection.LeftToRight;
            AutoSize = true;
            Margin = new Padding(0);
            Padding = new Padding(0);

            var labelDisplayName = new Label
            {
                Text = displayName,
                AutoSize = true,
                ForeColor = color ?? Color.Black,
                Font = Fonts.Instance.Bold,
                //BackColor = Color.Gray,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            Controls.Add(labelDisplayName);

            _labelMessage = new Label
            {
                Text = message,
                AutoSize = true,
                ForeColor = Color.Black,
                Font = Fonts.Instance.Regular,
                //BackColor = Color.LightGray,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            _labelMessage.MouseClick += LabelMessage_MouseClick;
            Controls.Add(_labelMessage);
        }

        private void LabelMessage_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add("Copy", null, OnCopy);
                contextMenu.Items.Add(new ToolStripSeparator());
                contextMenu.Items.Add("Remove", null, OnRemove);
                contextMenu.Show(_labelMessage, e.Location);
            }
        }

        private void OnRemove(object? sender, EventArgs e)
        {
            try
            {
                _labelMessage.Text = string.Empty;
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
                Clipboard.SetText(_labelMessage.Text);
            }
            catch
            {
            }
        }
    }
}
