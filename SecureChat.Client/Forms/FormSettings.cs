﻿using NTDLS.Persistence;
using NTDLS.WinFormsHelpers;
using SecureChat.Library;
using Serilog;
using System.Diagnostics;

namespace SecureChat.Client.Forms
{
    public partial class FormSettings : Form
    {
        public FormSettings(bool showInTaskbar)
        {
            InitializeComponent();

            if (showInTaskbar)
            {
                ShowInTaskbar = true;
                StartPosition = FormStartPosition.CenterScreen;
                TopMost = true;
            }
            else
            {
                ShowInTaskbar = false;
                StartPosition = FormStartPosition.CenterParent;
                TopMost = false;
            }

            AcceptButton = buttonSave;
            CancelButton = buttonCancel;

            textBoxServerAddress.Text = Settings.Instance.ServerAddress;
            textBoxServerPort.Text = $"{Settings.Instance.ServerPort:n0}";
            textBoxAutoAwayIdleSeconds.Text = $"{Settings.Instance.AutoAwayIdleSeconds:n0}";
            textBoxMaxMessages.Text = $"{Settings.Instance.MaxMessages:n0}";
            textBoxFileTransmissionChunkSize.Text = $"{Settings.Instance.FileTransmissionChunkSize:n0}";
            textBoxMaxFileTransmissionSize.Text = $"{Settings.Instance.MaxFileTransmissionSize:n0}";

            textBoxFontSample.Text = "John: Hey, how's is been going?\r\nJane: Pretty good. I've about to head out.\r\nJohn: Wanna grab some lunch?\r\nJane: Thai?\r\nJohn: Are you kidding me? Absolutely!\r\n";

            foreach (var font in FontFamily.Families.OrderBy(o => o.Name))
            {
                comboBoxFont.Items.Add(font.Name);
            }
            comboBoxFont.Text = Settings.Instance.Font;
            numericUpDownFontSize.Value = (decimal)Settings.Instance.FontSize;

            numericUpDownFontSize.ValueChanged += (object? sender, EventArgs e) => UpdateFontSample();
            comboBoxFont.SelectedIndexChanged += (object? sender, EventArgs e) => UpdateFontSample();

            UpdateFontSample();
        }

        private void UpdateFontSample()
        {
            var selectedFontName = comboBoxFont.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedFontName) == false)
            {
                try
                {
                    var fontSize = numericUpDownFontSize.Value;
                    if (fontSize > 0)
                    {
                        textBoxFontSample.Font = new Font(selectedFontName, (float)fontSize);
                    }
                }
                catch { }
            }
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                var settings = LocalUserApplicationData.LoadFromDisk(ScConstants.AppName, new Settings());
                settings.ServerAddress = textBoxServerAddress.GetAndValidateText("Server address must not be empty.");
                settings.Font = comboBoxFont.Text;
                settings.FontSize = (float)numericUpDownFontSize.Value;
                settings.ServerPort = textBoxServerPort.GetAndValidateNumeric(1, 65535, "Server port must be between [min] and [max].");
                settings.AutoAwayIdleSeconds = textBoxAutoAwayIdleSeconds.GetAndValidateNumeric(60, 86400, "Auto-away idle seconds must be between [min] and [max].");
                settings.MaxMessages = textBoxMaxMessages.GetAndValidateNumeric(10, 10000, "Max messages must be between [min] and [max].");
                settings.FileTransmissionChunkSize = textBoxFileTransmissionChunkSize.GetAndValidateNumeric(128, 1024 * 1024, "File transmission chunk size must be between [min] and [max].");
                settings.MaxFileTransmissionSize = textBoxMaxFileTransmissionSize.GetAndValidateNumeric(128, 1024 * 1024 * 1024, "Max file transmission size must be between [min] and [max].");

                Settings.Instance = settings;

                this.InvokeClose(DialogResult.OK);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.InvokeClose(DialogResult.Cancel);
        }
    }
}
