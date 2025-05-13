using System.ComponentModel;

namespace SecureChat.Client.Controls
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
            labelDisplayName.BackColor = Color.Transparent;
            labelDisplayName.ForeColor = Color.White;
            labelDisplayName.Font = new Font("Arial", 10);
            labelDisplayName.Text = "This is from who??";

            labelMessage.Padding = new Padding(5, 0, 5, 5);
            labelMessage.AutoSize = true;
            labelMessage.BackColor = Color.Transparent;
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
                labelMessage.MaximumSize = new Size((_parent.Width - labelMessage.Left) - 30, 0);
                this.Height = labelMessage.Top + labelMessage.Height + 5;
                this.Width = labelMessage.Left + labelMessage.Width + 5;
                _lastWidth = _parent.Width;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            CalculateLabelSize();

            using var bubbleBrush = new SolidBrush(BubbleColor);

            var rect = new Rectangle(0, 0, this.Width, this.Height);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.FillRoundedRectangle(bubbleBrush, rect.X, rect.Y, rect.Width, rect.Height, 15);
            base.OnPaint(e);
        }
    }
}
