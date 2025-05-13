using Krypton.Toolkit;
using SecureChat.Client.Controls;
using System.ComponentModel;
using static SecureChat.Library.ScConstants;

namespace SecureChat.Client.Forms
{
    public partial class FormTest : KryptonForm
    {
        public FormTest()
        {
            InitializeComponent();

            Resize += (s, e) =>
            {
                flowLayoutPanelChat.SuspendLayout();

                foreach (var child in flowLayoutPanelChat.Controls)
                {
                    if (child is FlowControlMessageBubble bubble && bubble.IsVisible)
                    {
                        bubble.Width = flowLayoutPanelChat.Width;
                    }
                }
                flowLayoutPanelChat.ResumeLayout();

                flowLayoutPanelChat.Invalidate(true);
            };
        }

        private void AddChatBubble(string displayName, Color bubbleColor, ScAlignment alignment, string message)
        {
            var bubble = new FlowControlMessageBubble(flowLayoutPanelChat, message, displayName, bubbleColor, alignment);
            flowLayoutPanelChat.Controls.Add(bubble);
            flowLayoutPanelChat.VerticalScroll.Value = flowLayoutPanelChat.VerticalScroll.Maximum;
        }

        int number = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            Color bubbleColor = Color.FromArgb(255, 0, 0, 0);
            string displayName = "Test User";
            ScAlignment alignment;

            if (number % 2 == 0)
            {
                displayName = "Josh";
                bubbleColor = Themes.FromMeColor;
                alignment = ScAlignment.Right;
            }
            else
            {
                displayName = "George";
                bubbleColor = Themes.FromRemoteColor;
                alignment = ScAlignment.Left;
            }

            AddChatBubble(displayName, bubbleColor, alignment, $"{number++:n0} This is message one. This is message two. This is message three. This is message four. This is message five. This is message six. This is message seven. This is message eight. This is message nine. This is message ten.");
        }

        private void FormTest_Load(object sender, EventArgs e)
        {
            button1.Focus();
            this.AcceptButton = button1;
        }
    }
}
