using SecureChat.Library.ReliableMessages;

namespace SecureChat.Client.Forms
{
    public partial class FormMessage : Form
    {
        private readonly ActiveChat _activeChat;

        private DateTime? _lastMessageReceived;

        internal FormMessage(ActiveChat activeChat)
        {
            InitializeComponent();

            _activeChat = activeChat;
            richTextBoxMessages.AppendText($"Chat started with {activeChat.DisplayName} at {DateTime.Now}.\r\n\r\n");

            FormClosing += FormMessage_FormClosing;

            textBoxMessage.KeyDown += TextBoxMessage_KeyDown;

            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 5000;
            timer.Tick += Timer_Tick;
            timer.Enabled = true;
        }

        private void FormMessage_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (SessionState.Instance != null)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (_lastMessageReceived != null)
            {
                if ((DateTime.Now - (DateTime)_lastMessageReceived).TotalSeconds > 60)
                {
                    lock (richTextBoxMessages)
                    {
                        richTextBoxMessages.SelectionStart = richTextBoxMessages.TextLength;
                        richTextBoxMessages.SelectionLength = 0;

                        richTextBoxMessages.SelectionColor = Color.Gray;
                        richTextBoxMessages.SelectionFont = new Font(richTextBoxMessages.Font, FontStyle.Italic);
                        richTextBoxMessages.AppendText($"Last message received {_lastMessageReceived}.\r\n");
                        richTextBoxMessages.SelectionFont = richTextBoxMessages.Font;
                        richTextBoxMessages.ScrollToCaret();

                        _lastMessageReceived = null;
                    }
                }
            }
        }

        private void TextBoxMessage_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (e.Shift)
                {
                    int selectionStart = textBoxMessage.SelectionStart;
                    textBoxMessage.Text = textBoxMessage.Text.Insert(selectionStart, Environment.NewLine);
                    textBoxMessage.SelectionStart = selectionStart + Environment.NewLine.Length;
                }
                else
                {
                    ButtonSend_Click(sender, new EventArgs());
                }

                e.SuppressKeyPress = true;
            }
        }

        public void AppendMessageLine(string message)
        {
            if (InvokeRequired)
            {
                Invoke(AppendMessageLine, message);
                return;
            }
            lock (richTextBoxMessages)
            {
                richTextBoxMessages.SelectionStart = richTextBoxMessages.TextLength;
                richTextBoxMessages.SelectionLength = 0;
                richTextBoxMessages.AppendText($"{message}\r\n");
                richTextBoxMessages.ScrollToCaret();

            }
        }

        public void AppendReceivedMessage(byte[] cipherText)
        {
            if (InvokeRequired)
            {
                Invoke(AppendReceivedMessage, cipherText);
                return;
            }

            _lastMessageReceived = DateTime.Now;

            lock (richTextBoxMessages)
            {
                richTextBoxMessages.SelectionStart = richTextBoxMessages.TextLength;
                richTextBoxMessages.SelectionLength = 0;

                richTextBoxMessages.SelectionColor = Color.Red;
                richTextBoxMessages.AppendText($"{_activeChat.DisplayName}: ");

                richTextBoxMessages.SelectionColor = richTextBoxMessages.ForeColor;
                richTextBoxMessages.AppendText($"{_activeChat.Decrypt(cipherText)}\r\n");
                richTextBoxMessages.ScrollToCaret();
            }
        }

        private void ButtonSend_Click(object? sender, EventArgs e)
        {
            if (SessionState.Instance == null)
            {
                Close();
                return;
            }

            lock (richTextBoxMessages)
            {
                richTextBoxMessages.SelectionStart = richTextBoxMessages.TextLength;
                richTextBoxMessages.SelectionLength = 0;

                richTextBoxMessages.SelectionColor = Color.Blue;
                richTextBoxMessages.AppendText($"{SessionState.Instance.DisplayName}: ");

                richTextBoxMessages.SelectionColor = richTextBoxMessages.ForeColor;
                richTextBoxMessages.AppendText($"{textBoxMessage.Text}\r\n");
                richTextBoxMessages.ScrollToCaret();
            }

            SessionState.Instance?.Client.Notify(new ExchangePeerToPeerMessage(
                    _activeChat.ConnectionId, SessionState.Instance.AccountId, _activeChat.Encrypt(textBoxMessage.Text)));

            textBoxMessage.Clear();
        }
    }
}
