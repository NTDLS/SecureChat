using System.Text;
using System.Windows.Forms;

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
            Load += FormMessage_Load;

            textBoxMessage.KeyDown += TextBoxMessage_KeyDown;

            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 5000;
            timer.Tick += Timer_Tick;
            timer.Enabled = true;
        }

        private void FormMessage_Load(object? sender, EventArgs e)
        {
            var currentScreen = Screen.FromPoint(Cursor.Position);
            int offsetY = 10; // Distance above the taskbar
            int offsetX = 10; // Distance from the right of the screen.
            int x = currentScreen.WorkingArea.Right - this.Width - offsetX;
            int y = currentScreen.WorkingArea.Bottom - this.Height - offsetY;
            Location = new Point(x, y);
        }

        private void FormMessage_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (LocalSession.Current == null || _activeChat.IsTerminated)
            {
                return; //Close the dialog.
            }

            e.Cancel = true;
            Hide();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (LocalSession.Current == null)
            {
                return;
            }
            if (_activeChat.IsTerminated)
            {
                return;
            }

            if (_lastMessageReceived != null)
            {
                if ((DateTime.Now - (DateTime)_lastMessageReceived).TotalSeconds > 60)
                {
                    AppendSystemMessageLine(Color.Gray, $"Last message received {_lastMessageReceived}.");
                    _lastMessageReceived = null;
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

        public void AppendSystemMessageLine(Color color, string message)
        {
            if (InvokeRequired)
            {
                Invoke(AppendSystemMessageLine, [color, message]);
                return;
            }

            lock (richTextBoxMessages)
            {
                richTextBoxMessages.SelectionStart = richTextBoxMessages.TextLength;
                richTextBoxMessages.SelectionLength = 0;

                richTextBoxMessages.SelectionColor = Color.Gray;
                richTextBoxMessages.SelectionFont = new Font(richTextBoxMessages.Font, FontStyle.Italic);
                richTextBoxMessages.AppendText($"{message}\r\n");
                richTextBoxMessages.SelectionFont = richTextBoxMessages.Font;
                richTextBoxMessages.ScrollToCaret();

                _lastMessageReceived = null;
            }
        }

        public void AppendMessageLine(Color color, string message)
        {
            if (InvokeRequired)
            {
                Invoke(AppendMessageLine, [color, message]);
                return;
            }
            lock (richTextBoxMessages)
            {
                lock (richTextBoxMessages)
                {
                    richTextBoxMessages.SelectionStart = richTextBoxMessages.TextLength;
                    richTextBoxMessages.SelectionLength = 0;

                    richTextBoxMessages.SelectionColor = color;
                    richTextBoxMessages.AppendText($"{message}\r\n");
                }
            }
        }

        public void AppendReceivedMessageLine(Color color, string fromName, string plainText)
        {
            if (InvokeRequired)
            {
                Invoke(AppendReceivedMessageLine, [color, fromName, plainText]);
                return;
            }

            _lastMessageReceived = DateTime.Now;

            lock (richTextBoxMessages)
            {
                richTextBoxMessages.SelectionStart = richTextBoxMessages.TextLength;
                richTextBoxMessages.SelectionLength = 0;

                richTextBoxMessages.SelectionColor = color;
                richTextBoxMessages.AppendText($"{fromName}: ");

                richTextBoxMessages.SelectionColor = richTextBoxMessages.ForeColor;
                richTextBoxMessages.AppendText($"{plainText}\r\n");
                richTextBoxMessages.ScrollToCaret();
            }
        }

        private void ButtonSend_Click(object? sender, EventArgs e)
        {
            if (LocalSession.Current == null)
            {
                AppendMessageLine(Color.Red, "Not connected.");
                return;
            }

            if (_activeChat.IsTerminated)
            {
                AppendMessageLine(Color.Red, "Chat has ended.");
                return;
            }

            string text = textBoxMessage.Text;
            textBoxMessage.Clear();

            if (_activeChat.SendMessage(text))
            {
                AppendReceivedMessageLine(Color.Blue, LocalSession.Current.DisplayName, text);
            }
            else
            {
                AppendMessageLine(Color.Red, "Failed to send message.");
            }
        }

        #region Toolbar Menu

        private void TerminateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _activeChat.Terminate();
        }

        #endregion

        private void ImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //InsertImageIntoRichTextBox(openFileDialog.FileName);
                }
            }
        }
    }
}
