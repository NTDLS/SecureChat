using SecureChat.Client.Controls;
using System.ComponentModel;

namespace chat
{
    public partial class MeBubble : UserControl
    {
        private readonly Control _parent;
        private readonly string _message;
        int _lastWidth = -1;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color BubbleColor { get; set; } = Color.DodgerBlue;

        public MeBubble(Control parent, string message)
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();

            InitializeComponent();

            _message = message; 
            _parent = parent;

            label1.Left = 10;
            label1.AutoSize = true;
            label1.BackColor = BubbleColor;
            label1.ForeColor = Color.Black;
            label1.Font = new Font("Arial", 12);
            label1.Text = message;

            CalcSize();

            Resize += (object? sender, EventArgs e) =>
            {
                CalcSize();
            };
        }

        private void CalcSize()
        {
            if (_lastWidth != _parent.Width)
            {
                this.MaximumSize = new Size(_parent.Width - 20, 0);
                label1.MaximumSize = new Size(_parent.Width - 50, 0);
                this.Height = label1.Height + 20;
                this.Width = label1.Width + 20;
                _lastWidth = _parent.Width;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.SuspendLayout();
            CalcSize();

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
            this.ResumeLayout();

        }
    }

}

