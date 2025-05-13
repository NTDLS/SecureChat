using NTDLS.Helpers;
using System.ComponentModel;
using System.Windows.Forms;

namespace SecureChat.Client.Controls
{
    public class ChatBubble : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Message { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color TextColor { get; set; } = Color.Black;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color BubbleColor { get; set; } = Color.DodgerBlue;
        private FlowLayoutPanel _parent;

        public ChatBubble(FlowLayoutPanel parent, string message)
        {
            _parent = parent;

            Message = message;
            DoubleBuffered = true;
            //AutoSize = true;
            //AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Padding = new Padding(10);
            Margin = new Padding(5);
            BackColor = Color.Transparent;
            MinimumSize = new Size(100, 40); // Ensure a minimum size to avoid collapse
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using var bubbleBrush = new SolidBrush(BubbleColor);
            using var textBrush = new SolidBrush(TextColor);
            using var font = new Font("Arial", 12);

            // Calculate text size for dynamic height
            SizeF textSize = e.Graphics.MeasureString(Message, font);
            int bubbleWidth = Math.Max((int)textSize.Width + 20, MinimumSize.Width);
            int bubbleHeight = Math.Max((int)textSize.Height + 20, MinimumSize.Height);

            if(bubbleWidth > _parent.ClientSize.Width)
            {
                int lineCount = bubbleWidth / (_parent.ClientSize.Width - 20);

                bubbleWidth = _parent.ClientSize.Width - 20;

                bubbleHeight = (int)((textSize.Height * lineCount) + 20);
            }


            // Resize the control to fit the content
            this.Size = new Size(bubbleWidth, bubbleHeight);

            // Draw the rounded rectangle
            var rect = new Rectangle(0, 0, bubbleWidth - 1, bubbleHeight - 1);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.FillRoundedRectangle(bubbleBrush, rect.X, rect.Y, rect.Width, rect.Height, 15);

            // Draw the text inside the bubble
            e.Graphics.DrawString(Message, font, textBrush, new PointF(10, 10));
        }


        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Invalidate();  // Force repaint on size change
        }
    }
}
