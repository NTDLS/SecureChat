using SecureChat.Client.Controls;
using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace chat
{
    public partial class MeBubble : UserControl
    {
        private Control _parent;

        private string _message;
        //private Font _font;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color BubbleColor { get; set; } = Color.DodgerBlue;

        public MeBubble(Control parent, string message)
        {
            InitializeComponent();

            _message = message; 
            _parent = parent;
            //_font = new Font("Arial", 12);

            //AutoSize = true;
            //MaximumSize = new Size(_parent.Width - 40, 0);  // Wrap text at this width
            //Margin = new Padding(20);
            //Padding = new Padding(5);

            //this.BackColor = Color.Transparent;
            //this.ForeColor = Color.Black;
            //this.MaximumSize = new Size(_parent.Width - 20, 0);


            //label1.Dock = DockStyle.Fill;
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.ForeColor = Color.Black;
            label1.Font = new Font("Arial", 12);
            label1.Text = message;

            //CalcSize();

            this.Resize += MeBubble_Resize;

            Invalidate(true);
        }

        private void MeBubble_Resize(object? sender, EventArgs e)
        {
            CalcSize();
        }

        int _lastWidth = 0;

        private void CalcSize()
        {
            if (_lastWidth != _parent.Width)
            {
                this.MaximumSize = new Size(_parent.Width - 10, 0);
                label1.MaximumSize = new Size(_parent.Width - 20, 0);
                this.Height = label1.Height + 10;
                this.Width = label1.Width + 10;
                _lastWidth = _parent.Width;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            CalcSize();

            // Draw the rounded rectangle
            using var bubbleBrush = new SolidBrush(BubbleColor);

            int bubbleWidth = _lastWidth - 20;
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

