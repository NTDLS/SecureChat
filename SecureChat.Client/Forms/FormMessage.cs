using SecureChat.Client.Controls;

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

            FormClosing += FormMessage_FormClosing;
            Load += FormMessage_Load;

            Activated += FormMessage_Activated;
            Deactivate += FormMessage_Deactivate;

            textBoxMessage.KeyDown += TextBoxMessage_KeyDown;

            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 5000;
            timer.Tick += Timer_Tick;
            timer.Enabled = true;

            AppendSystemMessageLine($"Chat with {_activeChat.DisplayName} started at {DateTime.Now}.");
        }

        private void FormMessage_Deactivate(object? sender, EventArgs e)
        {
            Opacity = 0.8;
        }

        private void FormMessage_Activated(object? sender, EventArgs e)
        {
            Opacity = 1;
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
                    AppendSystemMessageLine($"Last message received {_lastMessageReceived}.");
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

        #region Append Flow Controls.

        private void AppendFlowControl(FlowLayoutPanel control)
        {
            if (InvokeRequired)
            {
                Invoke(AppendFlowControl, [control]);
                return;
            }
            lock (flowPanel)
            {
                flowPanel.Controls.Add(control);
                while (flowPanel.Controls.Count > 100)
                {
                    flowPanel.Controls.RemoveAt(0);
                }
                flowPanel.ScrollControlIntoView(control);
            }
        }

        public void AppendSystemMessageLine(string message, Color? color = null)
        {
            AppendFlowControl(new FlowControlSystemText(message, color));
        }

        public void AppendMessageLine(string message, Color? color = null)
        {
            if (InvokeRequired)
            {
                Invoke(AppendMessageLine, [message, color]);
                return;
            }
            lock (flowPanel)
            {
                AppendFlowControl(new FlowControlSystemText(message, color));
            }
        }

        public void AppendReceivedMessageLine(string fromName, string plainText, Color? color = null)
        {
            _lastMessageReceived = DateTime.Now;
            AppendFlowControl(new FlowControlTextMessage(fromName, plainText, color));
        }

        #endregion

        private void ButtonSend_Click(object? sender, EventArgs e)
        {
            if (LocalSession.Current == null)
            {
                AppendMessageLine("Not connected.", Color.Red);
                return;
            }

            if (_activeChat.IsTerminated)
            {
                AppendMessageLine("Chat has ended.", Color.Red);
                return;
            }

            string text = textBoxMessage.Text;
            if (text.Trim().Length == 0)
            {
                return;
            }
            textBoxMessage.Clear();

            if (_activeChat.SendMessage(text))
            {
                AppendReceivedMessageLine(LocalSession.Current.DisplayName, text, Color.Blue);
            }
            else
            {
                AppendMessageLine("Failed to send message.", Color.Red);
            }
        }

        #region Toolbar Menu

        private void TerminateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _activeChat.Terminate();
        }

        private void ImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                byte[] imageBytes = File.ReadAllBytes(openFileDialog.FileName);

                AppendFlowControl(new FlowControlImage(imageBytes));
            }
        }

        #endregion
    }
}
