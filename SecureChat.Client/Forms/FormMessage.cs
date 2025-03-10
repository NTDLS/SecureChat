using NTDLS.Helpers;
using SecureChat.Client.Controls;
using SecureChat.Client.Helpers;
using SecureChat.Client.Models;
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
        private readonly string[] allowedFileTypes = { "bmp", "jpg", "jpeg", "png", "gif" };
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
                    if (LocalSession.Current == null || _activeChat.IsTerminated || !LocalSession.Current.Client.IsConnected)
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
                    TransmitFileToRemoteClient(files[0]);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        var fileExtension = Path.GetExtension(files[0]).ToLower();

                        if (!allowedFileTypes.Contains(fileExtension.Substring(1)))
                        {
                            e.Effect = DragDropEffects.None;
                            return;
                        }

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
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            try
            {
                if (LocalSession.Current == null || _activeChat.IsTerminated || !LocalSession.Current.Client.IsConnected)
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
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
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
                            Task.Run(() => _activeChat.TransmitFile("clipboard.png", imageBytes));
                        }
                        e.SuppressKeyPress = true;
                    }
                    else if (Clipboard.ContainsFileDropList()) // If clipboard contains file(s)
                    {
                        string[] files = Clipboard.GetFileDropList().Cast<string>().ToArray();

                        if (files.Length == 1)
                        {
                            string filename = files[0];
                            var fileExtension = Path.GetExtension(filename).ToLower();
                            if (allowedFileTypes.Contains(fileExtension.Substring(1)))
                            {
                                Image image = Image.FromFile(filename);
                                var imageBytes = Imaging.ImageToPngBytes(image);
                                Task.Run(() => _activeChat.TransmitFile("clipboard.png", imageBytes));
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
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AppendSystemMessageLine(string message, Color? color = null)
        {
            AppendFlowControl(new FlowControlSystemText(flowPanel, message, color));
        }

        public void AppendIncomingCallRequest(string fromName, Color? color = null)
        {
            AppendFlowControl(new FlowControlIncomingCall(flowPanel, fromName));
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
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                AppendIncomingCallRequest(fromName, Color.Blue);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void ButtonSend_Click(object? sender, EventArgs e)
        {
            try
            {
                if (LocalSession.Current == null || _activeChat.IsTerminated || !LocalSession.Current.Client.IsConnected)
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
                    AppendReceivedMessageLine(LocalSession.Current.DisplayName, text, false, Color.Blue);
                }
                else
                {
                    AppendSystemMessageLine("Failed to send message.", Color.Red);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Toolbar Menu

        private void CallToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void EndCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void TerminateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _activeChat.Terminate();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    TransmitFileToRemoteClient(openFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TransmitFileToRemoteClient(string filename)
        {
            try
            {
                var fileExtension = Path.GetExtension(filename).ToLower();

                if (!allowedFileTypes.Contains(fileExtension.Substring(1)))
                {
                    AppendSystemMessageLine($"Unsupported file type: {fileExtension}.", Color.Red);
                    return;
                }

                long fileSize = (new FileInfo(filename)).Length;

                if (fileSize > Settings.Instance.MaxFileTransmissionSize)
                {
                    AppendSystemMessageLine($"File is too large {Formatters.FileSize(fileSize)}, max size is {Formatters.FileSize(Settings.Instance.MaxFileTransmissionSize)}.", Color.Red);
                }
                else if (fileSize > 0)
                {
                    var imageBytes = File.ReadAllBytes(filename);

                    Task.Run(() => _activeChat.TransmitFile(filename, imageBytes));
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var formVoicePreCall = new FormVoicePreCall();
            if (formVoicePreCall.ShowDialog() == DialogResult.OK && formVoicePreCall.InputDeviceIndex != null && formVoicePreCall.OutputDeviceIndex != null)
            {
                _activeChat.RequestVoiceCall();
                AppendSystemMessageLine($"Voice call request sent.", Color.Blue);
            }
        }
    }

    #endregion
}
