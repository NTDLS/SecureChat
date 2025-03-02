﻿namespace SecureChat.Client.Forms
{
    public partial class FormMessage : Form
    {
        private readonly ActiveChat _activeChat;
        private bool _allowClose = false;

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
            if (_allowClose)
            {
                return; //Close the dialog.
            }
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

        public void AppendReceivedMessageFrom(Color color, string fromName, string plainText)
        {
            if (InvokeRequired)
            {
                Invoke(AppendReceivedMessageFrom, [color, fromName, plainText]);
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
            if (SessionState.Instance == null)
            {
                Close();
                return;
            }

            string text = textBoxMessage.Text;
            textBoxMessage.Clear();

            if (_activeChat.SendMessage(text))
            {
                AppendReceivedMessageFrom(Color.Blue, SessionState.Instance.DisplayName, text);
            }
            else
            {
                AppendMessageLine(Color.Red, "Failed to send message.");
            }
        }

        #region Toolbar Menu

        private void TerminateToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        #endregion
    }
}
