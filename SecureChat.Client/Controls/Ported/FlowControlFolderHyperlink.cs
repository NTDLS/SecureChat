using NTDLS.Helpers;
using System.Diagnostics;
using static SecureChat.Library.ScConstants;

namespace SecureChat.Client.Controls
{
    public class FlowControlFolderHyperlink : FlowControlOriginBubble
    {
        private readonly string _folderPath;

        public FlowControlFolderHyperlink(FlowLayoutPanel parent, string displayText, string folderPath, ScOrigin origin, string? displayName = null)
            : base(parent, new LinkLabel { Text = displayText }, origin, displayName)
        {
            _folderPath = folderPath;

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
                        Process.Start("explorer.exe", _folderPath);
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
