using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecureChat.Client.Forms
{
    public partial class FormSettings : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(FormSettings));
            buttonSave = new Button();
            buttonCancel = new Button();
            tabControlBody = new TabControl();
            tabPageMessages = new TabPage();
            textBoxFontSample = new TextBox();
            labelFontAndSize = new Label();
            numericUpDownFontSize = new NumericUpDown();
            comboBoxFont = new ComboBox();
            tabPageServer = new TabPage();
            labelServerPort = new Label();
            labelServerAddress = new Label();
            textBoxServerPort = new TextBox();
            textBoxServerAddress = new TextBox();
            tabPageAdvanced = new TabPage();
            checkBoxAutoStartAtWindowsLogin = new CheckBox();
            textBoxMaxFileTransmissionSize = new TextBox();
            textBoxFileTransmissionChunkSize = new TextBox();
            textBoxMaxMessages = new TextBox();
            textBoxAutoAwayIdleSeconds = new TextBox();
            labelMaxFileTransmissionSize = new Label();
            labelFileTransmissionChunkSize = new Label();
            labelMaxMessages = new Label();
            labelAutoAwayIdleSeconds = new Label();
            tabPageNotifications = new TabPage();
            checkBoxFlashWindowWhenMessageReceived = new CheckBox();
            checkBoxPlaySoundWhenMessageReceived = new CheckBox();
            checkBoxPlaySoundWhenContactComesOnline = new CheckBox();
            checkBoxAlertToastWhenMessageReceived = new CheckBox();
            checkBoxAlertToastWhenContactComesOnline = new CheckBox();
            tabControlBody.SuspendLayout();
            tabPageMessages.SuspendLayout();
            ((ISupportInitialize)numericUpDownFontSize).BeginInit();
            tabPageServer.SuspendLayout();
            tabPageAdvanced.SuspendLayout();
            tabPageNotifications.SuspendLayout();
            SuspendLayout();
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(299, 305);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(75, 23);
            buttonSave.TabIndex = 3;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += ButtonSave_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(381, 305);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 4;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // tabControlBody
            // 
            tabControlBody.Controls.Add(tabPageMessages);
            tabControlBody.Controls.Add(tabPageServer);
            tabControlBody.Controls.Add(tabPageAdvanced);
            tabControlBody.Controls.Add(tabPageNotifications);
            tabControlBody.Location = new Point(12, 12);
            tabControlBody.Name = "tabControlBody";
            tabControlBody.SelectedIndex = 0;
            tabControlBody.Size = new Size(448, 287);
            tabControlBody.TabIndex = 5;
            // 
            // tabPageMessages
            // 
            tabPageMessages.Controls.Add(textBoxFontSample);
            tabPageMessages.Controls.Add(labelFontAndSize);
            tabPageMessages.Controls.Add(numericUpDownFontSize);
            tabPageMessages.Controls.Add(comboBoxFont);
            tabPageMessages.Location = new Point(4, 24);
            tabPageMessages.Name = "tabPageMessages";
            tabPageMessages.Size = new Size(440, 259);
            tabPageMessages.TabIndex = 0;
            tabPageMessages.Text = "Messages";
            tabPageMessages.UseVisualStyleBackColor = true;
            // 
            // textBoxFontSample
            // 
            textBoxFontSample.Location = new Point(11, 52);
            textBoxFontSample.Multiline = true;
            textBoxFontSample.Name = "textBoxFontSample";
            textBoxFontSample.Size = new Size(419, 197);
            textBoxFontSample.TabIndex = 2;
            // 
            // labelFontAndSize
            // 
            labelFontAndSize.AutoSize = true;
            labelFontAndSize.Location = new Point(11, 5);
            labelFontAndSize.Name = "labelFontAndSize";
            labelFontAndSize.Size = new Size(77, 15);
            labelFontAndSize.TabIndex = 3;
            labelFontAndSize.Text = "Font and Size";
            // 
            // numericUpDownFontSize
            // 
            numericUpDownFontSize.DecimalPlaces = 2;
            numericUpDownFontSize.Increment = new decimal(new int[] { 25, 0, 0, 131072 });
            numericUpDownFontSize.Location = new Point(368, 23);
            numericUpDownFontSize.Maximum = new decimal(new int[] { 30, 0, 0, 0 });
            numericUpDownFontSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownFontSize.Name = "numericUpDownFontSize";
            numericUpDownFontSize.Size = new Size(62, 23);
            numericUpDownFontSize.TabIndex = 1;
            numericUpDownFontSize.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // comboBoxFont
            // 
            comboBoxFont.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxFont.FormattingEnabled = true;
            comboBoxFont.Location = new Point(11, 23);
            comboBoxFont.Name = "comboBoxFont";
            comboBoxFont.Size = new Size(351, 23);
            comboBoxFont.TabIndex = 0;
            // 
            // tabPageServer
            // 
            tabPageServer.Controls.Add(labelServerPort);
            tabPageServer.Controls.Add(labelServerAddress);
            tabPageServer.Controls.Add(textBoxServerPort);
            tabPageServer.Controls.Add(textBoxServerAddress);
            tabPageServer.Location = new Point(4, 24);
            tabPageServer.Name = "tabPageServer";
            tabPageServer.Size = new Size(440, 259);
            tabPageServer.TabIndex = 2;
            tabPageServer.Text = "Server";
            tabPageServer.UseVisualStyleBackColor = true;
            // 
            // labelServerPort
            // 
            labelServerPort.AutoSize = true;
            labelServerPort.Location = new Point(225, 5);
            labelServerPort.Name = "labelServerPort";
            labelServerPort.Size = new Size(64, 15);
            labelServerPort.TabIndex = 3;
            labelServerPort.Text = "Server Port";
            // 
            // labelServerAddress
            // 
            labelServerAddress.AutoSize = true;
            labelServerAddress.Location = new Point(8, 5);
            labelServerAddress.Name = "labelServerAddress";
            labelServerAddress.Size = new Size(84, 15);
            labelServerAddress.TabIndex = 2;
            labelServerAddress.Text = "Server Address";
            // 
            // textBoxServerPort
            // 
            textBoxServerPort.Location = new Point(225, 23);
            textBoxServerPort.Name = "textBoxServerPort";
            textBoxServerPort.Size = new Size(80, 23);
            textBoxServerPort.TabIndex = 1;
            // 
            // textBoxServerAddress
            // 
            textBoxServerAddress.Location = new Point(8, 23);
            textBoxServerAddress.Name = "textBoxServerAddress";
            textBoxServerAddress.Size = new Size(211, 23);
            textBoxServerAddress.TabIndex = 0;
            // 
            // tabPageAdvanced
            // 
            tabPageAdvanced.Controls.Add(checkBoxAutoStartAtWindowsLogin);
            tabPageAdvanced.Controls.Add(textBoxMaxFileTransmissionSize);
            tabPageAdvanced.Controls.Add(textBoxFileTransmissionChunkSize);
            tabPageAdvanced.Controls.Add(textBoxMaxMessages);
            tabPageAdvanced.Controls.Add(textBoxAutoAwayIdleSeconds);
            tabPageAdvanced.Controls.Add(labelMaxFileTransmissionSize);
            tabPageAdvanced.Controls.Add(labelFileTransmissionChunkSize);
            tabPageAdvanced.Controls.Add(labelMaxMessages);
            tabPageAdvanced.Controls.Add(labelAutoAwayIdleSeconds);
            tabPageAdvanced.Location = new Point(4, 24);
            tabPageAdvanced.Name = "tabPageAdvanced";
            tabPageAdvanced.Size = new Size(440, 259);
            tabPageAdvanced.TabIndex = 1;
            tabPageAdvanced.Text = "Advanced";
            tabPageAdvanced.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoStartAtWindowsLogin
            // 
            checkBoxAutoStartAtWindowsLogin.AutoSize = true;
            checkBoxAutoStartAtWindowsLogin.Location = new Point(8, 237);
            checkBoxAutoStartAtWindowsLogin.Name = "checkBoxAutoStartAtWindowsLogin";
            checkBoxAutoStartAtWindowsLogin.Size = new Size(178, 19);
            checkBoxAutoStartAtWindowsLogin.TabIndex = 4;
            checkBoxAutoStartAtWindowsLogin.Text = "Auto-start at windows login?";
            checkBoxAutoStartAtWindowsLogin.UseVisualStyleBackColor = true;
            // 
            // textBoxMaxFileTransmissionSize
            // 
            textBoxMaxFileTransmissionSize.Location = new Point(8, 170);
            textBoxMaxFileTransmissionSize.Name = "textBoxMaxFileTransmissionSize";
            textBoxMaxFileTransmissionSize.Size = new Size(117, 23);
            textBoxMaxFileTransmissionSize.TabIndex = 3;
            // 
            // textBoxFileTransmissionChunkSize
            // 
            textBoxFileTransmissionChunkSize.Location = new Point(8, 121);
            textBoxFileTransmissionChunkSize.Name = "textBoxFileTransmissionChunkSize";
            textBoxFileTransmissionChunkSize.Size = new Size(117, 23);
            textBoxFileTransmissionChunkSize.TabIndex = 2;
            // 
            // textBoxMaxMessages
            // 
            textBoxMaxMessages.Location = new Point(8, 72);
            textBoxMaxMessages.Name = "textBoxMaxMessages";
            textBoxMaxMessages.Size = new Size(117, 23);
            textBoxMaxMessages.TabIndex = 1;
            // 
            // textBoxAutoAwayIdleSeconds
            // 
            textBoxAutoAwayIdleSeconds.Location = new Point(8, 23);
            textBoxAutoAwayIdleSeconds.Name = "textBoxAutoAwayIdleSeconds";
            textBoxAutoAwayIdleSeconds.Size = new Size(117, 23);
            textBoxAutoAwayIdleSeconds.TabIndex = 0;
            // 
            // labelMaxFileTransmissionSize
            // 
            labelMaxFileTransmissionSize.AutoSize = true;
            labelMaxFileTransmissionSize.Location = new Point(8, 152);
            labelMaxFileTransmissionSize.Name = "labelMaxFileTransmissionSize";
            labelMaxFileTransmissionSize.Size = new Size(73, 15);
            labelMaxFileTransmissionSize.TabIndex = 3;
            labelMaxFileTransmissionSize.Text = "Max File Size";
            // 
            // labelFileTransmissionChunkSize
            // 
            labelFileTransmissionChunkSize.AutoSize = true;
            labelFileTransmissionChunkSize.Location = new Point(8, 103);
            labelFileTransmissionChunkSize.Name = "labelFileTransmissionChunkSize";
            labelFileTransmissionChunkSize.Size = new Size(86, 15);
            labelFileTransmissionChunkSize.TabIndex = 2;
            labelFileTransmissionChunkSize.Text = "File Chunk Size";
            // 
            // labelMaxMessages
            // 
            labelMaxMessages.AutoSize = true;
            labelMaxMessages.Location = new Point(8, 54);
            labelMaxMessages.Name = "labelMaxMessages";
            labelMaxMessages.Size = new Size(83, 15);
            labelMaxMessages.TabIndex = 1;
            labelMaxMessages.Text = "Max Messages";
            // 
            // labelAutoAwayIdleSeconds
            // 
            labelAutoAwayIdleSeconds.AutoSize = true;
            labelAutoAwayIdleSeconds.Location = new Point(8, 5);
            labelAutoAwayIdleSeconds.Name = "labelAutoAwayIdleSeconds";
            labelAutoAwayIdleSeconds.Size = new Size(114, 15);
            labelAutoAwayIdleSeconds.TabIndex = 0;
            labelAutoAwayIdleSeconds.Text = "Auto-Away Seconds";
            // 
            // tabPageNotifications
            // 
            tabPageNotifications.Controls.Add(checkBoxFlashWindowWhenMessageReceived);
            tabPageNotifications.Controls.Add(checkBoxPlaySoundWhenMessageReceived);
            tabPageNotifications.Controls.Add(checkBoxPlaySoundWhenContactComesOnline);
            tabPageNotifications.Controls.Add(checkBoxAlertToastWhenMessageReceived);
            tabPageNotifications.Controls.Add(checkBoxAlertToastWhenContactComesOnline);
            tabPageNotifications.Location = new Point(4, 24);
            tabPageNotifications.Name = "tabPageNotifications";
            tabPageNotifications.Padding = new Padding(3);
            tabPageNotifications.Size = new Size(440, 259);
            tabPageNotifications.TabIndex = 3;
            tabPageNotifications.Text = "Notifications";
            tabPageNotifications.UseVisualStyleBackColor = true;
            // 
            // checkBoxFlashWindowWhenMessageReceived
            // 
            checkBoxFlashWindowWhenMessageReceived.AutoSize = true;
            checkBoxFlashWindowWhenMessageReceived.Location = new Point(6, 106);
            checkBoxFlashWindowWhenMessageReceived.Name = "checkBoxFlashWindowWhenMessageReceived";
            checkBoxFlashWindowWhenMessageReceived.Size = new Size(226, 19);
            checkBoxFlashWindowWhenMessageReceived.TabIndex = 4;
            checkBoxFlashWindowWhenMessageReceived.Text = "Flash window when message received";
            checkBoxFlashWindowWhenMessageReceived.UseVisualStyleBackColor = true;
            // 
            // checkBoxPlaySoundWhenMessageReceived
            // 
            checkBoxPlaySoundWhenMessageReceived.AutoSize = true;
            checkBoxPlaySoundWhenMessageReceived.Location = new Point(6, 81);
            checkBoxPlaySoundWhenMessageReceived.Name = "checkBoxPlaySoundWhenMessageReceived";
            checkBoxPlaySoundWhenMessageReceived.Size = new Size(188, 19);
            checkBoxPlaySoundWhenMessageReceived.TabIndex = 3;
            checkBoxPlaySoundWhenMessageReceived.Text = "Sound when message received";
            checkBoxPlaySoundWhenMessageReceived.UseVisualStyleBackColor = true;
            // 
            // checkBoxPlaySoundWhenContactComesOnline
            // 
            checkBoxPlaySoundWhenContactComesOnline.AutoSize = true;
            checkBoxPlaySoundWhenContactComesOnline.Location = new Point(6, 56);
            checkBoxPlaySoundWhenContactComesOnline.Name = "checkBoxPlaySoundWhenContactComesOnline";
            checkBoxPlaySoundWhenContactComesOnline.Size = new Size(215, 19);
            checkBoxPlaySoundWhenContactComesOnline.TabIndex = 2;
            checkBoxPlaySoundWhenContactComesOnline.Text = "Sound when contact comesOonline";
            checkBoxPlaySoundWhenContactComesOnline.UseVisualStyleBackColor = true;
            // 
            // checkBoxAlertToastWhenMessageReceived
            // 
            checkBoxAlertToastWhenMessageReceived.AutoSize = true;
            checkBoxAlertToastWhenMessageReceived.Location = new Point(6, 31);
            checkBoxAlertToastWhenMessageReceived.Name = "checkBoxAlertToastWhenMessageReceived";
            checkBoxAlertToastWhenMessageReceived.Size = new Size(211, 19);
            checkBoxAlertToastWhenMessageReceived.TabIndex = 1;
            checkBoxAlertToastWhenMessageReceived.Text = "Visual alert when message received";
            checkBoxAlertToastWhenMessageReceived.UseVisualStyleBackColor = true;
            // 
            // checkBoxAlertToastWhenContactComesOnline
            // 
            checkBoxAlertToastWhenContactComesOnline.AutoSize = true;
            checkBoxAlertToastWhenContactComesOnline.Location = new Point(6, 6);
            checkBoxAlertToastWhenContactComesOnline.Name = "checkBoxAlertToastWhenContactComesOnline";
            checkBoxAlertToastWhenContactComesOnline.Size = new Size(232, 19);
            checkBoxAlertToastWhenContactComesOnline.TabIndex = 0;
            checkBoxAlertToastWhenContactComesOnline.Text = "Visual alert when contact comes online";
            checkBoxAlertToastWhenContactComesOnline.UseVisualStyleBackColor = true;
            // 
            // FormSettings
            // 
            ClientSize = new Size(467, 340);
            Controls.Add(tabControlBody);
            Controls.Add(buttonCancel);
            Controls.Add(buttonSave);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormSettings";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Secure Chat";
            tabControlBody.ResumeLayout(false);
            tabPageMessages.ResumeLayout(false);
            tabPageMessages.PerformLayout();
            ((ISupportInitialize)numericUpDownFontSize).EndInit();
            tabPageServer.ResumeLayout(false);
            tabPageServer.PerformLayout();
            tabPageAdvanced.ResumeLayout(false);
            tabPageAdvanced.PerformLayout();
            tabPageNotifications.ResumeLayout(false);
            tabPageNotifications.PerformLayout();
            ResumeLayout(false);
        }
        private Button buttonSave;
        private Button buttonCancel;
        private TabControl tabControlBody;
        private TabPage tabPageMessages;
        private ComboBox comboBoxFont;
        private TabPage tabPageAdvanced;
        private NumericUpDown numericUpDownFontSize;
        private Label labelFontAndSize;
        private TabPage tabPageServer;
        private TextBox textBoxFontSample;
        private Label labelServerPort;
        private Label labelServerAddress;
        private TextBox textBoxServerPort;
        private TextBox textBoxServerAddress;
        private TextBox textBoxMaxFileTransmissionSize;
        private TextBox textBoxFileTransmissionChunkSize;
        private TextBox textBoxMaxMessages;
        private TextBox textBoxAutoAwayIdleSeconds;
        private Label labelMaxFileTransmissionSize;
        private Label labelFileTransmissionChunkSize;
        private Label labelMaxMessages;
        private Label labelAutoAwayIdleSeconds;
        private CheckBox checkBoxAutoStartAtWindowsLogin;
        private TabPage tabPageNotifications;
        private CheckBox checkBoxFlashWindowWhenMessageReceived;
        private CheckBox checkBoxPlaySoundWhenMessageReceived;
        private CheckBox checkBoxPlaySoundWhenContactComesOnline;
        private CheckBox checkBoxAlertToastWhenMessageReceived;
        private CheckBox checkBoxAlertToastWhenContactComesOnline;
    }
}
