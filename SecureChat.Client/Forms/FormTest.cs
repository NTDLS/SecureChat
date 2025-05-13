using chat;
using Krypton.Toolkit;
using System.Windows.Forms;

namespace SecureChat.Client.Forms
{
    public partial class FormTest : KryptonForm
    {
        public FormTest()
        {
            InitializeComponent();

            this.Resize += (s, e) =>
            {
                flowLayoutPanelChat.SuspendLayout();

                foreach (var child in flowLayoutPanelChat.Controls)
                {
                    if (child is MeBubble bubble)
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
            //flowLayoutPanelChat.SuspendLayout();

            var bubble = new MeBubble(flowLayoutPanelChat, message);
            flowLayoutPanelChat.Controls.Add(bubble);

            var button = new Button();

            flowLayoutPanelChat.Controls.Add(button);
            panelScroll.VerticalScroll.Value = panelScroll.VerticalScroll.Maximum;

            //flowLayoutPanelChat.Controls.Remove(button);
            //flowLayoutPanelChat.ResumeLayout();



            //this.Invoke(() =>
            //panelScroll.PerformLayout();
            //panelScroll.VerticalScroll.Value = panelScroll.VerticalScroll.Maximum;

            //flowLayoutPanelChat.PerformLayout();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddChatBubble("This is message one. This is message two. This is message three. This is message four. This is message five. This is message six. This is message seven. This is message eight. This is message nine. This is message ten.");

        }
    }
}
