using NTDLS.Helpers;
using SecureChat.Client.Controls;
using SecureChat.Client.Helpers;
using SecureChat.Library;
using Serilog;
using System.Diagnostics;

namespace SecureChat.Client.Forms
{
    public partial class FormMessage : Form
    {
        private readonly int DefaultHeight = 550;
        private readonly int DefaultWidth = 550;

        private readonly ActiveChat _activeChat;
        private DateTime? _lastMessageReceived;

        internal FormMessage(ActiveChat activeChat)
        {
            InitializeComponent();

            try
            {
                _activeChat = activeChat;

                Text = $"{activeChat.DisplayName} - {Text}";

                FormClosing += (object? sender, FormClosingEventArgs e) =>
                {
                    if (ServerConnection.Current == null || _activeChat.IsTerminated || !ServerConnection.Current.ReliableClient.IsConnected)
                    {
                        return; //Close the dialog.
                    }

                    e.Cancel = true;
                    Hide();
                };

                Load += (object? sender, EventArgs e) =>
                {
                    Height = DefaultHeight;
                    Width = DefaultWidth;

                    var currentScreen = Screen.FromPoint(Cursor.Position);
                    int offsetY = 10; // Distance above the taskbar
                    int offsetX = 10; // Distance from the right of the screen.
                    int x = currentScreen.WorkingArea.Right - DefaultWidth - offsetX;
                    int y = currentScreen.WorkingArea.Bottom - DefaultHeight - offsetY;
                    Location = new Point(x, y);
                };

                Shown += (object? sender, EventArgs e) => textBoxMessage.Focus();
                Activated += (object? sender, EventArgs e) => Exceptions.Ignore(() => Opacity = 1.0);
                Deactivate += (object? sender, EventArgs e) => Exceptions.Ignore(() => Opacity = 0.95);

                textBoxMessage.AllowDrop = true;
                textBoxMessage.KeyDown += TextBoxMessage_KeyDown;
                textBoxMessage.DragEnter += TextBoxMessage_DragEnter;
                textBoxMessage.DragDrop += TextBoxMessage_DragDrop;
                textBoxMessage.Focus();

                var timer = new System.Windows.Forms.Timer();
                timer.Interval = 5000;
                timer.Tick += Timer_Tick;
                timer.Enabled = true;

                AppendSystemMessageLine($"Chat with {_activeChat.DisplayName} started at {DateTime.Now}.");
            }
            catch (Exception ex)
            {
                Log.Fatal($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                throw;
            }
        }

        private void TextBoxMessage_DragDrop(object? sender, DragEventArgs e)
        {
            try
            {
                var files = (string[]?)e.Data?.GetData(DataFormats.FileDrop);

                if (files?.Length == 1)
                {
                    _activeChat.TransmitFileAsync(files[0]);
                }
            }
            catch (Exception ex)
            {
                AppendErrorLine(ex);
            }
        }

        private void TextBoxMessage_DragEnter(object? sender, DragEventArgs e)
        {
            try
            {
                if (e.Data?.GetDataPresent(DataFormats.FileDrop) != null)
                {
                    var files = (string[]?)e.Data.GetData(DataFormats.FileDrop);
                    if (files?.Length == 1)
                    {
                        e.Effect = DragDropEffects.Copy; // Allow drop
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            catch (Exception ex)
            {
                AppendErrorLine(ex);
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            try
            {
                if (ServerConnection.Current == null || _activeChat.IsTerminated || !ServerConnection.Current.ReliableClient.IsConnected)
                {
                    if (Visible == false)
                    {
                        //If the session has been terminated and the windows is hidden, then just close it.
                        //No need to keep forms hanging out forever.
                        Close();
                    }

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
            catch (Exception ex)
            {
                AppendErrorLine(ex);
            }
        }

        private void TextBoxMessage_KeyDown(object? sender, KeyEventArgs e)
        {
            try
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
                else if (e.Control && e.KeyCode == Keys.V) //Paste
                {
                    if (Clipboard.ContainsImage())
                    {
                        var image = Clipboard.GetImage();
                        if (image != null)
                        {
                            var imageBytes = Imaging.ImageToPngBytes(image);
                            _activeChat.TransmitFileAsync("clipboard.png", imageBytes);
                        }
                        e.SuppressKeyPress = true;
                    }
                    else if (Clipboard.ContainsFileDropList()) // If clipboard contains file(s)
                    {
                        string[] files = Clipboard.GetFileDropList().Cast<string>().ToArray();

                        if (files.Length == 1)
                        {
                            string filename = files[0];
                            if (ScConstants.ImageFileTypes.Contains(Path.GetExtension(filename), StringComparer.InvariantCultureIgnoreCase))
                            {
                                Image image = Image.FromFile(filename);
                                var imageBytes = Imaging.ImageToPngBytes(image);
                                _activeChat.TransmitFileAsync("clipboard.png", imageBytes);
                            }
                        }
                    }
                    else
                    {
                        //textBoxMessage.Paste();
                    }
                }
            }
            catch (Exception ex)
            {
                AppendErrorLine(ex);
            }
        }

        #region Append Flow Controls.

        private void AppendFlowControl(Control control)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(AppendFlowControl, [control]);
                    return;
                }

                lock (flowPanel)
                {
                    flowPanel.Controls.Add(control);
                    while (flowPanel.Controls.Count > Settings.Instance.MaxMessages)
                    {
                        flowPanel.Controls.RemoveAt(0);
                    }
                    flowPanel.ScrollControlIntoView(control);
                }
            }
            catch (Exception ex)
            {
                AppendErrorLine(ex);
            }
        }

        public void AppendImageMessage(string fromName, byte[] imageBytes, bool playNotifications)
        {
            try
            {
                AppendFlowControl(new FlowControlImage(flowPanel, imageBytes));

                Invoke(() =>
                {
                    if (Visible == false)
                    {
                        //We want to show the dialog, but keep it minimized so that it does not jump in front of the user.
                        WindowState = FormWindowState.Minimized;
                        Visible = true;
                    }

                    if (playNotifications)
                    {
                        if (WindowFlasher.FlashWindow(this))
                        {
                            Notifications.MessageReceived(fromName);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                AppendErrorLine(ex);
            }
        }

        public void AppendErrorLine(Exception ex, Color? color = null)
        {
            var baseException = ex.GetBaseException();
            AppendFlowControl(new FlowControlSystemText(flowPanel, baseException.Message, color ?? Color.Red));
            Log.Error(baseException, baseException.Message);
        }

        public void AppendErrorLine(string message, Color? color = null)
        {
            AppendFlowControl(new FlowControlSystemText(flowPanel, message, color ?? Color.Red));
            Log.Error(message);
        }

        public void AppendSystemMessageLine(string message, Color? color = null)
        {
            AppendFlowControl(new FlowControlSystemText(flowPanel, message, color));
        }

        public void AppendIncomingCallRequest(string fromName)
        {
            AppendFlowControl(new FlowControlIncomingCall(flowPanel, _activeChat, fromName));
        }

        public void AppendOutgoingCallRequest(string toName)
        {
            _activeChat.LastOutgoingCallControl = new FlowControlOutgoingCall(flowPanel, _activeChat, toName);
            AppendFlowControl(_activeChat.LastOutgoingCallControl);
        }

        public void AppendReceivedMessageLine(string fromName, string plainText, bool playNotifications, Color? color = null)
        {
            try
            {
                Invoke(() =>
                {
                    if (Visible == false)
                    {
                        //We want to show the dialog, but keep it minimized so that it does not jump in front of the user.
                        WindowState = FormWindowState.Minimized;
                        Visible = true;
                    }

                    if (playNotifications)
                    {
                        if (WindowFlasher.FlashWindow(this))
                        {
                            Notifications.MessageReceived(fromName);
                        }
                    }
                });

                _lastMessageReceived = DateTime.Now;

                if (plainText.StartsWith("http://") || plainText.StartsWith("https://"))
                {
                    AppendFlowControl(new FlowControlHyperlink(flowPanel, fromName, plainText, color));
                }
                else
                {
                    AppendFlowControl(new FlowControlTextMessage(flowPanel, fromName, plainText, color));
                }
            }
            catch (Exception ex)
            {
                AppendErrorLine(ex);
            }
        }

        public void AppendIncomingCall(string fromName, bool playNotifications, Color? color = null)
        {
            try
            {
                Invoke(() =>
                {
                    if (Visible == false)
                    {
                        //We want to show the dialog, but keep it minimized so that it does not jump in front of the user.
                        WindowState = FormWindowState.Minimized;
                        Visible = true;
                    }

                    if (playNotifications)
                    {
                        if (WindowFlasher.FlashWindow(this))
                        {
                            Notifications.IncomingCall(fromName);
                        }
                    }
                });

                _lastMessageReceived = DateTime.Now;

                AppendIncomingCallRequest(fromName);
            }
            catch (Exception ex)
            {
                AppendErrorLine(ex);
            }
        }

        #endregion

        private void ButtonSend_Click(object? sender, EventArgs e)
        {
            try
            {
                if (ServerConnection.Current == null || _activeChat.IsTerminated || !ServerConnection.Current.ReliableClient.IsConnected)
                {
                    AppendSystemMessageLine("Not connected.", Color.Red);
                    return;
                }

                if (_activeChat.IsTerminated)
                {
                    AppendSystemMessageLine("Chat has ended.", Color.Red);
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
                    AppendReceivedMessageLine(ServerConnection.Current.DisplayName, text, false, Color.Blue);
                }
                else
                {
                    AppendSystemMessageLine("Failed to send message.", Color.Red);
                }
            }
            catch (Exception ex)
            {
                AppendErrorLine(ex);
            }
        }

        #region Toolbar Menu



        private void ToolStripButtonTerminate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show($"End the chat session with {_activeChat.DisplayName}?",
                    ScConstants.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    _activeChat.Terminate();
                }
            }
            catch (Exception ex)
            {
                AppendErrorLine(ex);
            }
        }

        private void ToolStripButtonAttachFile_Click(object sender, EventArgs e)
        {
            try
            {
                using OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _activeChat.TransmitFileAsync(openFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                AppendErrorLine(ex);
            }
        }

        private void ToolStripButtonVoiceCall_Click(object sender, EventArgs e)
        {
            using var formVoicePreCall = new FormVoicePreCall();
            if (formVoicePreCall.ShowDialog() == DialogResult.OK)
            {
                _activeChat.RequestVoiceCall(formVoicePreCall.InputDeviceIndex, formVoicePreCall.OutputDeviceIndex, formVoicePreCall.Bitrate);
                AppendOutgoingCallRequest(_activeChat.DisplayName);
            }
        }

        private void ToolStripButtonExport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Export not implemented.", ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ToolStripButtonProperties_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Properties not implemented.", ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    #endregion
}
