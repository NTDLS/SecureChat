using Krypton.Toolkit;
using SecureChat.Client.Controls;

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
                    if (child is MessageBubble bubble && bubble.IsVisible)
                    {
                        bubble.Width = flowLayoutPanelChat.Width;
                    }
                }
                flowLayoutPanelChat.ResumeLayout();

                flowLayoutPanelChat.Invalidate(true);
            };
        }

        private void AddChatBubble(string message)
        {
            var bubble = new MessageBubble(flowLayoutPanelChat, message);
            flowLayoutPanelChat.Controls.Add(bubble);
            flowLayoutPanelChat.VerticalScroll.Value = flowLayoutPanelChat.VerticalScroll.Maximum;
        }

        int number = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            AddChatBubble($"{number++:n0} This is message one. This is message two. This is message three. This is message four. This is message five. This is message six. This is message seven. This is message eight. This is message nine. This is message ten.");
        }
    }
}
