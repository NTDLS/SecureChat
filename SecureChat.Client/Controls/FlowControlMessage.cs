using NTDLS.Helpers;
using static SecureChat.Library.ScConstants;

namespace SecureChat.Client.Controls
{
    public partial class FlowControlMessage : FlowControlOriginBubble
    {
        private readonly Control _parent;
        private readonly ScOrigin _origin;

        public FlowControlMessage(Control parent, string message, string displayName, ScOrigin origin)
            : base(parent, new Label() { Text = message }, origin, displayName)
        {
            _parent = parent;
            _origin = origin;

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

        private void OnRemove(object? sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnRemove(sender, e)));
                return;
            }

            Exceptions.Ignore(() => _parent.Controls.Remove(this));
        }

        private void OnCopy(object? sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnCopy(sender, e)));
                return;
            }

            Exceptions.Ignore(() => Clipboard.SetText(ChildControl.Text));
        }
    }
}
