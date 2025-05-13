using NTDLS.Helpers;
using Serilog.Parsing;
using System.Text;
using static SecureChat.Library.ScConstants;

namespace SecureChat.Client.Controls
{
    public partial class FlowControlMessageBubble : UserControl
    {
        private readonly Control _parent;
        private readonly string _message;
        private int _lastWidth = -1;
        private readonly Color _bubbleColor = Color.DodgerBlue;
        private readonly ScAlignment _alignment;
        

        public bool IsVisible =>
            _parent.ClientRectangle.IntersectsWith(_parent.RectangleToClient(Bounds));

        public FlowControlMessageBubble(Control parent, string message, string displayName, Color bubbleColor, ScAlignment alignment = 0)
        {
            InitializeComponent();

            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();

            _alignment = alignment;
            _bubbleColor = bubbleColor;
            _message = message;
            _parent = parent;

            var font = new Font(Settings.Instance.Font, Settings.Instance.FontSize);

            //labelDisplayName.Padding = new Padding(5, 5, 5, 2);
            labelDisplayName.AutoSize = true;
            labelDisplayName.BackColor = Color.Transparent;
            labelDisplayName.ForeColor = Themes.AdjustBrightness(Themes.InvertColor(bubbleColor), 0.85f);
            labelDisplayName.Font = font;
            labelDisplayName.Text = displayName;

            //labelMessage.Padding = new Padding(5, 0, 5, 5);
            labelMessage.AutoSize = true;
            labelMessage.BackColor = Color.Transparent;
            labelMessage.ForeColor = Themes.AdjustBrightness(Themes.InvertColor(bubbleColor), 0.5f);
            labelMessage.Font = font;
            labelMessage.Text = message;

            labelDisplayName.Visible = false;

            CalculateLabelSize();

            Resize += (object? sender, EventArgs e) =>
            {
                CalculateLabelSize();
            };

            MouseClick += LabelMessage_MouseClick;
        }

        private void CalculateLabelSize()
        {
            if (_lastWidth != _parent.Width)
            {
                int padding = _parent.Width / 3;

                if (_alignment == ScAlignment.Right)
                {
                    labelDisplayName.Padding = new Padding(padding, 0, 5, 5);
                    labelMessage.Padding = new Padding(padding, 0, 5, 5);
                }
                else
                {
                    labelDisplayName.Padding = new Padding(5, 0, padding, 5);
                    labelMessage.Padding = new Padding(5, 0, padding, 5);
                }

                this.MaximumSize = new Size(_parent.Width - 30, 0);
                labelMessage.MaximumSize = new Size((_parent.Width - labelMessage.Left) - 40, 0);
                this.Height = labelMessage.Top + labelMessage.Height + 5;
                this.Width = labelMessage.Left + labelMessage.Width + 5;
                _lastWidth = _parent.Width;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            CalculateLabelSize();

            using var bubbleBrush = new SolidBrush(_bubbleColor);

            var rect = new Rectangle(labelMessage.Padding.Left - 5, 0, this.Width - (labelMessage.Padding.Left + labelMessage.Padding.Right) +5 , this.Height - 1);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.FillRoundedRectangle(bubbleBrush, rect.X, rect.Y, rect.Width, rect.Height, 15);
            base.OnPaint(e);
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

            Exceptions.Ignore(() => Clipboard.SetText(labelMessage.Text));
        }
    }
}
