using SecureChat.Client.Helpers;
using System.Diagnostics;

namespace SecureChat.Client.Controls
{
    public class FlowControlHyperlink : FlowLayoutPanel
    {
        public FlowControlHyperlink(string displayName, string message, Color? color)
        {
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

            Controls.Add(labelMessage);
        }

        private void LblMessage_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (e.Link != null)
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = e.Link.ToString(),
                        UseShellExecute = true
                    });
                }
            }
            catch
            {
            }
        }
    }
}
