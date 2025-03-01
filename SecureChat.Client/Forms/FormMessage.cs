using SecureChat.Library.ReliableMessages;

namespace SecureChat.Client.Forms
{
    public partial class FormMessage : Form
    {
        private readonly ActiveChat _activeChat;

        internal FormMessage(ActiveChat activeChat)
        {
            InitializeComponent();

            _activeChat = activeChat;
            richTextBoxMessages.AppendText($"Chat started with {activeChat.AccountId}.\r\n\r\n");
        }

        public void AppendMessageLine(string message)
        {
            if (InvokeRequired)
            {
                Invoke(AppendMessageLine, message);
                return;
            }
            richTextBoxMessages.AppendText($"{message}\r\n");
        }

        private void ButtonSend_Click(object sender, EventArgs e)
        {
            SessionState.Instance?.Client.Notify(new ExchangePeerToPeerMessage(
                _activeChat.ConnectionId, _activeChat.AccountId, SessionState.Instance.DisplayName, textBoxMessage.Text));
        }
    }
}
