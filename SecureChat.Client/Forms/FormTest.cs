using Krypton.Toolkit;
using SecureChat.Client.Controls;
using static SecureChat.Library.ScConstants;

namespace SecureChat.Client.Forms
{
    public partial class FormTest : KryptonForm
    {
        public FormTest()
        {
            InitializeComponent();

            BackColor = KryptonManager.CurrentGlobalPalette.GetBackColor1(PaletteBackStyle.PanelClient, PaletteState.Normal);

            kryptonTextBoxMessage.Text = $"This is message one. This is message two. This is message three. This is message four. This is message five. This is message six. This is message seven. This is message eight. This is message nine. This is message ten.";

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

            kryptonButtonSend.Focus();
            this.AcceptButton = kryptonButtonSend;
        }

        private void AddChatBubble(string displayName, ScOrigin origin, string message)
        {
            var bubble = new FlowControlMessageBubble(flowLayoutPanelChat, message, displayName, origin, true);
            flowLayoutPanelChat.Controls.Add(bubble);
            flowLayoutPanelChat.VerticalScroll.Value = flowLayoutPanelChat.VerticalScroll.Maximum;
        }

        int number = 0;

        private void kryptonButtonSend_Click(object sender, EventArgs e)
        {
            ScOrigin origin = ScOrigin.None;
            string displayName = "Test User";

            if (number++ % 2 == 0)
            {
                displayName = "Josh";
                origin = ScOrigin.Local;
            }
            else
            {
                displayName = "George";
                origin = ScOrigin.Remote;
            }



            AddChatBubble(displayName, origin, kryptonTextBoxMessage.Text);
        }
    }
}
