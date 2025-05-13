using SecureChat.Client.Controls;
using System.ComponentModel;

namespace chat
{
    public partial class MessageBubble : UserControl
    {
        private readonly Control _parent;
        private readonly string _message;
        int _lastWidth = -1;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color BubbleColor { get; set; } = Color.DodgerBlue;

        public bool IsVisible =>
            _parent.ClientRectangle.IntersectsWith(_parent.RectangleToClient(Bounds));

        public MessageBubble(Control parent, string message)
        {
            InitializeComponent();

            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();

            _message = message;
            _parent = parent;

            labelDisplayName.Padding = new Padding(5, 5, 5, 2);
            labelDisplayName.AutoSize = true;

            labelMessage.Padding = new Padding(5, 0, 5, 5);
            labelMessage.AutoSize = true;
            labelMessage.BackColor = BubbleColor;
            labelMessage.ForeColor = Color.Black;
            labelMessage.Font = new Font("Arial", 12);
            labelMessage.Text = message;

            CalculateLabelSize();

            Resize += (object? sender, EventArgs e) =>
            {
                CalculateLabelSize();
            };
        }

        private void CalculateLabelSize()
        {
            if (_lastWidth != _parent.Width)
            {
                this.MaximumSize = new Size(_parent.Width - 20, 0);
                labelMessage.MaximumSize = new Size(_parent.Width - 50, 0);
                this.Height = labelMessage.Top + labelMessage.Height + 20;
                this.Width = labelMessage.Left + labelMessage.Width + 20;
                _lastWidth = _parent.Width;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            CalculateLabelSize();

            // Draw the rounded rectangle
            using var bubbleBrush = new SolidBrush(BubbleColor);

            int bubbleWidth = _lastWidth - 30;
            int bubbleHeight = this.Height;

            // Resize the control to fit the content
            this.Size = new Size(bubbleWidth, bubbleHeight);

            // Draw the rounded rectangle
            var rect = new Rectangle(0, 0, bubbleWidth - 1, bubbleHeight - 1);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.FillRoundedRectangle(bubbleBrush, rect.X, rect.Y, rect.Width, rect.Height, 15);
            base.OnPaint(e);
        }
    }
}

