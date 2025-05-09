using NTDLS.Helpers;
using SecureChat.Client.Helpers;

namespace SecureChat.Client.Controls
{
    public class FlowControlSystemText : FlowLayoutPanel
    {
        private readonly FlowLayoutPanel _parent;
        private readonly Label _labelMessage;

        public FlowControlSystemText(FlowLayoutPanel parent, string message, Color? color = null)
        {
            _parent = parent;
            FlowDirection = FlowDirection.TopDown;
            AutoSize = true;
            Margin = new Padding(0);

            color ??= Settings.Instance.Theme == Library.ScConstants.Theme.Dark ? Color.LightGray : Color.Maroon;

            _labelMessage = new Label
            {
                Text = message,
                AutoSize = true,
                ForeColor = color.EnsureNotNull(),
                Font = Fonts.Instance.Italic,
                Padding = new Padding(0)
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
                contextMenu.Show((sender as Control) ?? this, e.Location);
            }
        }

        private void OnRemove(object? sender, EventArgs e)
        {
            Exceptions.Ignore(() =>
            {
                _labelMessage.Text = string.Empty;
                _parent.Controls.Remove(this);
            });
        }

        private void OnCopy(object? sender, EventArgs e)
        {
            Exceptions.Ignore(() =>
            {
                Clipboard.SetText(_labelMessage.Text);
            });
        }
    }
}
