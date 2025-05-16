using NTDLS.Helpers;
using System.Diagnostics;
using static Talkster.Library.ScConstants;

namespace Talkster.Client.Controls.FlowControls
{
    public class FlowControlHyperlink
        : FlowControlOriginBubble, IFlowControl
    {
        public FlowControlHyperlink(FlowLayoutPanel parent, string linkText, ScOrigin origin, Image? initialStatusImage = null, string? displayName = null)
            : base(parent, new LinkLabel { Text = linkText }, origin, initialStatusImage, displayName)
        {

            if (ChildControl is LinkLabel child)
            {
                child.LinkClicked += LabelMessage_LinkClicked;
                child.MouseClick += LabelMessage_MouseClick;
            }
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
                contextMenu.Show(sender as Control ?? this, e.Location);
            }
        }
    }
}
