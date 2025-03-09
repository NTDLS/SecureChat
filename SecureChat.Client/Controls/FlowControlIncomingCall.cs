using SecureChat.Client.Helpers;

namespace SecureChat.Client.Controls
{
    public class FlowControlIncomingCall : FlowLayoutPanel
    {
        private readonly FlowLayoutPanel _parent;

        public FlowControlIncomingCall(FlowLayoutPanel parent, string fromName, Color? color = null)
        {
            _parent = parent;
            FlowDirection = FlowDirection.LeftToRight;
            AutoSize = true;
            Margin = new Padding(0);
            Padding = new Padding(0);

            var labelDisplayName = new Label
            {
                Text = fromName,
                AutoSize = true,
                ForeColor = color ?? Color.Black,
                Font = Fonts.Instance.Bold,
                //BackColor = Color.Gray,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            Controls.Add(labelDisplayName);

            /*
            var labelMessage = new LinkLabel
            {
                Text = message,
                AutoSize = true,
                ForeColor = Color.Blue,
                Font = Fonts.Instance.Regular,
                //BackColor = Color.LightGray,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            labelMessage.LinkClicked += LblMessage_LinkClicked;
            labelMessage.MouseClick += LabelMessage_MouseClick;
            Controls.Add(labelMessage);
            */
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
