using NTDLS.Helpers;
using static SecureChat.Library.ScConstants;

namespace SecureChat.Client.Controls
{
    public partial class FlowControlMessageBubble : Panel
    {
        private readonly Control _parent;
        private int _lastWidth = -1;
        private readonly Color _bubbleColor = Color.DodgerBlue;
        private readonly ScOrigin _origin;

        private readonly Label _labelMessage;
        private readonly Label? _labelDisplayName;

        public bool IsVisible =>
            _parent.ClientRectangle.IntersectsWith(_parent.RectangleToClient(Bounds));

        public FlowControlMessageBubble(Control parent, string message, string displayName, ScOrigin origin, bool showDisplayName)
        {
            _parent = parent;
            _origin = origin;

            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();

            if (origin == ScOrigin.Local)
            {
                _bubbleColor = Themes.FromLocalColor;
            }
            else
            {
                _bubbleColor = Themes.FromRemoteColor;
            }

            using var font = new Font(Settings.Instance.Font, Settings.Instance.FontSize);

            if (showDisplayName)
            {
                _labelDisplayName = new Label()
                {
                    AutoSize = true,
                    BackColor = Color.Transparent,
                    ForeColor = Themes.AdjustBrightness(Themes.InvertColor(_bubbleColor), 0.85f),
                    Font = font,
                    Text = displayName,
                    Left = 0,
                    Top = 0,
                };
                _labelDisplayName.MouseClick += LabelMessage_MouseClick;
                Controls.Add(_labelDisplayName);
            }

            _labelMessage = new Label
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                ForeColor = Themes.AdjustBrightness(Themes.InvertColor(_bubbleColor), 0.5f),
                Font = font,
                Text = message,
                Left = 0,
                Top = (_labelDisplayName?.Top + _labelDisplayName?.Height + 5) ?? 0,

            };
            Controls.Add(_labelMessage);

            CalculateLabelSize();

            Resize += (object? sender, EventArgs e) =>
            {
                CalculateLabelSize();
            };

            _labelMessage.MouseClick += LabelMessage_MouseClick;
            MouseClick += LabelMessage_MouseClick;
        }

        private void CalculateLabelSize()
        {
            if (_lastWidth != _parent.Width)
            {
                int padding = _parent.Width / 3;

                if (_origin == ScOrigin.Local)
                {
                    if (_labelDisplayName != null)
                    {
                        _labelDisplayName.Padding = new Padding(padding, 0, 5, 5);
                    }
                    _labelMessage.Padding = new Padding(padding, 0, 5, 5);
                }
                else
                {
                    if (_labelDisplayName != null)
                    {
                        _labelDisplayName.Padding = new Padding(5, 0, padding, 5);
                    }
                    _labelMessage.Padding = new Padding(5, 0, padding, 5);
                }

                this.MaximumSize = new Size(_parent.Width - 30, 0);
                _labelMessage.MaximumSize = new Size((_parent.Width - _labelMessage.Left) - 40, 0);
                this.Height = _labelMessage.Top + _labelMessage.Height + 5;
                this.Width = _labelMessage.Left + _labelMessage.Width + 5;
                _lastWidth = _parent.Width;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            CalculateLabelSize();

            using var bubbleBrush = new SolidBrush(_bubbleColor);

            //var rect = new Rectangle(labelMessage.Padding.Left - 5, 0, this.Width - (labelMessage.Padding.Left + labelMessage.Padding.Right) + 5, this.Height - 1);
            //e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //e.Graphics.FillRoundedRectangle(bubbleBrush, rect.X, rect.Y, rect.Width, rect.Height, 15);
            //base.OnPaint(e);
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

            Exceptions.Ignore(() => Clipboard.SetText(_labelMessage.Text));
        }
    }
}
