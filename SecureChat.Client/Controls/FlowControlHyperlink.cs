using NTDLS.Helpers;
using SecureChat.Client.Helpers;
using System.Diagnostics;

namespace SecureChat.Client.Controls
{
    public class FlowControlHyperlink : FlowLayoutPanel
    {
        private readonly FlowLayoutPanel _parent;

        public FlowControlHyperlink(FlowLayoutPanel parent, string displayName, string message, Color? color)
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
            labelDisplayName.MouseClick += LabelMessage_MouseClick;
            Controls.Add(labelDisplayName);

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
            labelMessage.LinkClicked += LabelMessage_LinkClicked;
            labelMessage.MouseClick += LabelMessage_MouseClick;
            Controls.Add(labelMessage);
        }

        private void LabelMessage_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Exceptions.Ignore(() =>
                {
                    if (sender is LinkLabel linkLabel)
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = linkLabel.Text,
                            UseShellExecute = true
                        });
                    }
                });
            }
        }

        private void LabelMessage_MouseClick(object? sender, MouseEventArgs e)
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
