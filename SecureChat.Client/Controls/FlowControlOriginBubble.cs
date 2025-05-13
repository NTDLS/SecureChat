using NTDLS.Helpers;
using static SecureChat.Library.ScConstants;

namespace SecureChat.Client.Controls
{
    public partial class FlowControlOriginBubble : Panel
    {
        private readonly Control _parent;
        private int _lastWidth = -1;
        private readonly Color _bubbleColor = Color.DodgerBlue;
        private readonly ScOrigin _origin;

        private readonly Control _childControl;
        private readonly Label? _labelDisplayName;

        public Control ChildControl => _childControl;

        public bool IsVisible =>
            _parent.ClientRectangle.IntersectsWith(_parent.RectangleToClient(Bounds));

        public FlowControlOriginBubble(Control parent, Control childControl, ScOrigin origin, string? displayName = null)
        {
            _parent = parent;
            _origin = origin;
            _childControl = childControl;

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

            if (displayName != null)
            {
                _labelDisplayName = new Label()
                {
                    AutoSize = true,
                    BackColor = Color.Transparent,
                    ForeColor = Themes.AdjustBrightness(Themes.InvertColor(_bubbleColor), 0.85f),
                    Font = font,
                    Text = displayName,
                    Top = 0,
                };
                Controls.Add(_labelDisplayName);
            }

            _childControl.AutoSize = true;
            _childControl.BackColor = Color.Transparent;
            _childControl.ForeColor = Themes.AdjustBrightness(Themes.InvertColor(_bubbleColor), 0.5f);
            _childControl.Font = font;
            _childControl.Top = (_labelDisplayName?.Top + _labelDisplayName?.Height + 5) ?? 0;

            Controls.Add(_childControl);

            CalculateChildSize();

            Resize += (object? sender, EventArgs e) =>
            {
                CalculateChildSize();
            };
        }

        private void CalculateChildSize()
        {

            if (_lastWidth != _parent.Width)
            {

                int padding = _parent.Width / 3;

                if (_origin == ScOrigin.Local)
                {
                    if (_labelDisplayName != null)
                    {
                        _labelDisplayName.Left = padding + 10;
                    }
                    _childControl.Left = padding + 10;
                }
                else
                {
                    if (_labelDisplayName != null)
                    {
                        _labelDisplayName.Left = 10;
                    }
                    _childControl.Left = 10;
                }

                if (_childControl is Label)
                {
                    //We do some special stuff here to allow the label logic to perform its magic auto-wrapping.
                    this.MaximumSize = new Size(_parent.Width - 30, 0);
                    _childControl.MaximumSize = new Size((_parent.Width - padding) - 40, 0);
                    this.Height = _childControl.Top + _childControl.Height + 5;

                    this.Width = Math.Max(_childControl.Left + _childControl.Width + 5, (_labelDisplayName?.Left + _labelDisplayName?.Width + 5) ?? 0);
                    _lastWidth = _parent.Width;
                }
                else
                {
                    this.Height = _childControl.Top + _childControl.Height + 10;
                    this.Width = Math.Max(_parent.Width - 30, 100);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            CalculateChildSize();

            using var bubbleBrush = new SolidBrush(_bubbleColor);

            int minWidth = Math.Max(_childControl.Width + 10, (_labelDisplayName?.Width + 10) ?? 0);
            var rect = new Rectangle(_childControl.Left - 5, 0, minWidth, this.Height);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.FillRoundedRectangle(bubbleBrush, rect.X, rect.Y, rect.Width, rect.Height, 15);
            base.OnPaint(e);
        }

        public virtual void OnRemove(object? sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnRemove(sender, e)));
                return;
            }

            Exceptions.Ignore(() => _parent.Controls.Remove(this));
        }

        public virtual void OnCopy(object? sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnCopy(sender, e)));
                return;
            }

            Exceptions.Ignore(() => Clipboard.SetText(_childControl.Text));
        }
    }
}