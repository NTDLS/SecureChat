using static SecureChat.Library.ScConstants;

namespace SecureChat.Client.Controls
{
    public partial class FlowControlMessage : FlowControlOriginBubble
    {
        public FlowControlMessage(Control parent, string message, ScOrigin origin, string? displayName = null)
            : base(parent, new Label() { Text = message }, origin, displayName)
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();

            ChildControl.MouseClick += LabelMessage_MouseClick;
            MouseClick += LabelMessage_MouseClick;
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
    }
}
