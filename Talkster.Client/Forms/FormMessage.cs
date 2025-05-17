using Krypton.Toolkit;
using NTDLS.Helpers;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Talkster.Client.Controls.FlowControls;
using Talkster.Client.Helpers;
using Talkster.Library;
using static System.Windows.Forms.DataFormats;
using static Talkster.Library.ScConstants;

namespace Talkster.Client.Forms
{
    public partial class FormMessage : KryptonForm
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsRecycled { get; private set; } = false;

        private readonly int DefaultHeight = 550;
        private readonly int DefaultWidth = 550;
        private ActiveChat _activeChat;
        private bool _isFormClosing = false;
        private readonly System.Windows.Forms.Timer _timer;

        public FlowLayoutPanel FlowPanel => flowPanel;

        internal FormMessage(ActiveChat activeChat)
        {
            InitializeComponent();

            BackColor = KryptonManager.CurrentGlobalPalette.GetBackColor1(PaletteBackStyle.PanelClient, PaletteState.Normal);

            try
            {
                _activeChat = activeChat;

                Text = $"{activeChat.DisplayName} - {Text}";

                FormClosing += (object? sender, FormClosingEventArgs e) =>
                {
                    if (_isFormClosing || ServerConnection.Current == null || _activeChat.IsTerminated || !ServerConnection.Current.Connection.Client.IsConnected)
                    {
                        _timer?.Stop();
                        _timer?.Dispose();
                        _isFormClosing = true;
                        ServerConnection.Current?.FormHome.RemoveMessageForm(_activeChat.AccountId);
                        return; //Close the dialog.
                    }

                    //Just hide the form because we keep the session alive until the user explicitly terminates it.
                    e.Cancel = true;
                    Hide();
                };

                Shown += (object? sender, EventArgs e) => textBoxMessage.Focus();
                Activated += (object? sender, EventArgs e) => Exceptions.Ignore(() =>
                {
                    if (!_isFormClosing)
                    {
                        Opacity = 1.0;
                    }
                });

                Deactivate += (object? sender, EventArgs e) => Exceptions.Ignore(() =>
                {
                    if (!_isFormClosing)
                    {
                        Opacity = 1.0;
                    }
                });

                Resize += (s, e) =>
                {
                    flowPanel.SuspendLayout();

                    foreach (var child in flowPanel.Controls)
                    {
                        if (child is FlowControlOriginBubble bubble && bubble.IsVisible)
                        {
                            bubble.Width = flowPanel.Width;
                        }
                    }
                    flowPanel.ResumeLayout();

                    flowPanel.Invalidate(true);
                };

                Height = DefaultHeight;
                Width = DefaultWidth;

                var currentScreen = Screen.FromPoint(Cursor.Position);
                int offsetY = 10; // Distance above the taskbar
                int offsetX = 10; // Distance from the right of the screen.
                int x = currentScreen.WorkingArea.Right - DefaultWidth - offsetX;
                int y = currentScreen.WorkingArea.Bottom - DefaultHeight - offsetY;
                Location = new Point(x, y);

                textBoxMessage.AllowDrop = true;
                textBoxMessage.KeyDown += TextBoxMessage_KeyDown;
                textBoxMessage.DragEnter += TextBoxMessage_DragEnter;
                flowPanel.DragEnter += TextBoxMessage_DragEnter;
                textBoxMessage.DragDrop += TextBoxMessage_DragDrop;
                flowPanel.DragDrop += TextBoxMessage_DragDrop;
                textBoxMessage.Focus();
                textBoxMessage.MaxLength = ScConstants.MaxTextMessageSize;
                textBoxMessage.Font = new Font(Settings.Instance.Font, (float)Settings.Instance.FontSize);

                _timer = new System.Windows.Forms.Timer
                {
                    Interval = 5000,
                    Enabled = true
                };
                _timer.Tick += Timer_Tick;
            }
            catch (Exception ex)
            {
                Program.Log.Fatal($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                throw;
            }
        }

        internal void Recycle(ActiveChat activeChat)
        {
            _activeChat = activeChat;
            IsRecycled = true;
        }

        public void Close(bool force)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Close(force)));
                return;
            }

            _isFormClosing = true;
            Close();
        }

        public void ToggleVoiceCallButtons(bool isCallingEnabled)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ToggleVoiceCallButtons(isCallingEnabled)));
                return;
            }
            toolStripButtonVoiceCall.Enabled = isCallingEnabled;
            toolStripButtonVoiceCallEnd.Enabled = !isCallingEnabled;
        }

        private void TextBoxMessage_DragDrop(object? sender, DragEventArgs e)
        {
            try
            {
                var files = (string[]?)e.Data?.GetData(DataFormats.FileDrop);
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        _activeChat.TransmitFileAsync(file);
                    }
                }
            }
            catch (Exception ex)
            {
                _activeChat.AppendErrorLine(ex);
            }
        }

        private void TextBoxMessage_DragEnter(object? sender, DragEventArgs e)
        {
            try
            {
                if (e.Data?.GetDataPresent(DataFormats.FileDrop) != null)
                {
                    var files = (string[]?)e.Data.GetData(DataFormats.FileDrop);
                    if (files?.Length <= ScConstants.DefaultMaxFileDrops)
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
                _activeChat.AppendErrorLine(ex);
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            try
            {
                if (ServerConnection.Current == null || _activeChat.IsTerminated || !ServerConnection.Current.Connection.Client.IsConnected)
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

                if (_activeChat.LastMessageReceived != null)
                {
                    if ((DateTime.Now - (DateTime)_activeChat.LastMessageReceived).TotalSeconds > 60)
                    {
                        _activeChat.AppendSystemMessageLine($"Last message received {_activeChat.LastMessageReceived}.");
                        _activeChat.LastMessageReceived = null;
                    }
                }
            }
            catch (Exception ex)
            {
                _activeChat.AppendErrorLine(ex);
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
                        var dataObject = Clipboard.GetDataObject();

                        if (dataObject?.GetFormats().Contains("HTML Format") == true)
                        {
                            var bytes = GetAnimatedGifFromHTML();
                            if (bytes != null)
                            {
                                _activeChat.TransmitFileAsync($"clipboard.gif", bytes);
                            }
                        }
                        else
                        {
                            var image = Clipboard.GetImage();
                            if (image != null)
                            {
                                var imageBytes = Imaging.ImageToPngBytes(image);
                                _activeChat.TransmitFileAsync($"clipboard.png", imageBytes);
                            }
                        }

                        e.SuppressKeyPress = true;
                    }
                    else if (Clipboard.ContainsFileDropList()) // If clipboard contains file(s)
                    {
                        var fileNames = Clipboard.GetFileDropList().Cast<string>().ToArray();
                        foreach (var fileName in fileNames)
                        {
                            _activeChat.TransmitFileAsync(fileName);
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
                _activeChat.AppendErrorLine(ex);
            }
        }

        /// <summary>
        /// This is used to get the animated gif images from the ("Windows-Key" + ".") GIF picker.
        /// </summary>
        /// <returns></returns>
        private byte[]? GetAnimatedGifFromHTML()
        {
            try
            {
                var dataObject = Clipboard.GetDataObject();

                if (dataObject?.GetData("HTML Format") is string html)
                {
                    if (!string.IsNullOrEmpty(html))
                    {
                        var match = Regex.Match(html, @"<img[^>]+src=[""']([^""']+\.gif)[""']", RegexOptions.IgnoreCase);
                        if (match.Success)
                        {
                            var gifUrl = match.Groups[1].Value;
                            using var client = new HttpClient();
                            return client.GetByteArrayAsync(gifUrl).Result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _activeChat.AppendErrorLine(ex);
            }

            return null;
        }

        private void ButtonSend_Click(object? sender, EventArgs e)
        {
            try
            {
                if (ServerConnection.Current?.Connection.Client.IsConnected != true)
                {
                    _activeChat.AppendSystemMessageLine("Not connected.");
                    return;
                }

                if (_activeChat.IsTerminated)
                {
                    try
                    {
                        ConnectionHelpers.EstablishEndToEndConnectionFor(_activeChat.AccountId);
                    }
                    catch (Exception ex)
                    {
                        _activeChat.AppendErrorLine(ex.GetBaseException().Message);
                        return;
                    }
                }

                string text = textBoxMessage.Text;
                int textMessageSize = text.Length;
                if (textMessageSize == 0 || textMessageSize > ScConstants.MaxTextMessageSize)
                {
                    return;
                }
                textBoxMessage.Clear();

                var control = _activeChat.AppendChatMessage(ServerConnection.Current.DisplayName, text, ScOrigin.Local);
                if (control != null)
                {
                    try
                    {
                        _activeChat.SendTextMessage(control.UID, text);
                    }
                    catch
                    {
                        control.SetStatusError();
                    }
                    control.SetStatusSent();
                }
            }
            catch (Exception ex)
            {
                _activeChat.AppendErrorLine(ex);
            }
        }

        #region Toolbar Menu

        private void ToolStripButtonTerminate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show($"End the conversation with {_activeChat.DisplayName}?",
                    ScConstants.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    _activeChat.Terminate();
                }
            }
            catch (Exception ex)
            {
                _activeChat.AppendErrorLine(ex);
            }
        }

        private void ToolStripButtonAttachFile_Click(object sender, EventArgs e)
        {
            try
            {
                using OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "All Files (*.*)|*.*|Image Files (*.bmp; *.jpg; *.jpeg; *.png; *.gif)|*.bmp;*.jpg;*.jpeg;*.png;*.gif";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _activeChat.TransmitFileAsync(openFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                _activeChat.AppendErrorLine(ex);
            }
        }

        private void ToolStripButtonVoiceCall_Click(object sender, EventArgs e)
        {
            toolStripButtonVoiceCall.Enabled = false;
            toolStripButtonVoiceCallEnd.Enabled = false;

            using var formVoicePreCall = new FormVoicePreCall();
            if (formVoicePreCall.ShowDialog() == DialogResult.OK)
            {
                ToggleVoiceCallButtons(false);

                _activeChat.RequestVoiceCall(formVoicePreCall.InputDeviceIndex, formVoicePreCall.OutputDeviceIndex, formVoicePreCall.Bitrate);
                _activeChat.AppendOutgoingCallRequest(_activeChat.DisplayName);
            }
            else
            {
                ToggleVoiceCallButtons(true);
            }
        }

        private void ToolStripButtonVoiceCallEnd_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show($"End the voice call with {_activeChat.DisplayName}?",
                    ScConstants.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    //Disable the buttons to prevent multiple clicks.
                    //We will re-enable it when the call is successfully ended.
                    toolStripButtonVoiceCall.Enabled = false;
                    toolStripButtonVoiceCallEnd.Enabled = false;

                    _activeChat.RequestTerminateVoiceCall();
                }
            }
            catch (Exception ex)
            {
                _activeChat.AppendErrorLine(ex);
            }
        }

        private void ToolStripButtonExport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Export not implemented.", ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ToolStripButtonProperties_Click(object sender, EventArgs e)
        {
            using var form = new FormMessageProperties(_activeChat);
            form.ShowDialog();
        }
    }

    #endregion
}
