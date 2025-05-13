using Krypton.Toolkit;
using NTDLS.Helpers;
using System.Diagnostics;
using static SecureChat.Library.ScConstants;

namespace SecureChat.Client.Controls
{
    public class FlowControlHyperlink : FlowControlOriginBubble
    {
        public FlowControlHyperlink(FlowLayoutPanel parent, string linkText, ScOrigin origin, string? displayName = null)
            : base(parent, new LinkLabel { Text = linkText }, origin, displayName)
        {
            BackColor = KryptonManager.CurrentGlobalPalette.GetBackColor1(PaletteBackStyle.PanelClient, PaletteState.Normal);

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
                contextMenu.Show((sender as Control) ?? this, e.Location);
            }
        }
    }
}
