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
            tabPageUI = new TabPage();
            groupBoxTheme = new GroupBox();
            radioButtonThemeDark = new RadioButton();
            radioButtonThemeLight = new RadioButton();
            tabPageNotifications = new TabPage();
            checkBoxFlashWindowWhenMessageReceived = new CheckBox();
            checkBoxPlaySoundWhenMessageReceived = new CheckBox();
            checkBoxPlaySoundWhenContactComesOnline = new CheckBox();
            textBoxAutoAwayIdleSeconds = new TextBox();
            checkBoxAlertToastWhenMessageReceived = new CheckBox();
            labelAutoAwayIdleSeconds = new Label();
            checkBoxAlertToastWhenContactComesOnline = new CheckBox();
            tabPageServer = new TabPage();
            labelServerPort = new Label();
            labelServerAddress = new Label();
            textBoxServerPort = new TextBox();
            textBoxServerAddress = new TextBox();
            tabPageAdvanced = new TabPage();
            checkBoxAutoStartAtWindowsLogin = new CheckBox();
            textBoxFileTransferChunkSize = new TextBox();
            textBoxMaxMessages = new TextBox();
            labelFileTransferChunkSize = new Label();
            labelMaxMessages = new Label();
            tabPageCryptogrphy = new TabPage();
            labelRsaKeySize = new Label();
            textBoxEndToEndKeySize = new TextBox();
            textBoxAesKeySize = new TextBox();
            labelEndToEndKeySize = new Label();
            labelAesKeySize = new Label();
            textBoxRsaKeySize = new TextBox();
            tabControlBody.SuspendLayout();
            tabPageMessages.SuspendLayout();
            ((ISupportInitialize)numericUpDownFontSize).BeginInit();
            tabPageUI.SuspendLayout();
            groupBoxTheme.SuspendLayout();
            tabPageNotifications.SuspendLayout();
            tabPageServer.SuspendLayout();
            tabPageAdvanced.SuspendLayout();
            tabPageCryptogrphy.SuspendLayout();
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
            tabControlBody.Controls.Add(tabPageUI);
            tabControlBody.Controls.Add(tabPageNotifications);
            tabControlBody.Controls.Add(tabPageServer);
            tabControlBody.Controls.Add(tabPageAdvanced);
            tabControlBody.Controls.Add(tabPageCryptogrphy);
            tabControlBody.Location = new Point(12, 12);
            tabControlBody.Name = "tabControlBody";
            tabControlBody.SelectedIndex = 0;
            tabControlBody.Size = new Size(448, 287);
            tabControlBody.TabIndex = 0;
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
            // tabPageUI
            // 
            tabPageUI.Controls.Add(groupBoxTheme);
            tabPageUI.Location = new Point(4, 24);
            tabPageUI.Name = "tabPageUI";
            tabPageUI.Padding = new Padding(3);
            tabPageUI.Size = new Size(440, 259);
            tabPageUI.TabIndex = 5;
            tabPageUI.Text = "UI";
            tabPageUI.UseVisualStyleBackColor = true;
            // 
            // groupBoxTheme
            // 
            groupBoxTheme.Controls.Add(radioButtonThemeDark);
            groupBoxTheme.Controls.Add(radioButtonThemeLight);
            groupBoxTheme.Location = new Point(9, 6);
            groupBoxTheme.Name = "groupBoxTheme";
            groupBoxTheme.Size = new Size(101, 79);
            groupBoxTheme.TabIndex = 0;
            groupBoxTheme.TabStop = false;
            groupBoxTheme.Text = "Theme";
            // 
            // radioButtonThemeDark
            // 
            radioButtonThemeDark.AutoSize = true;
            radioButtonThemeDark.Location = new Point(6, 47);
            radioButtonThemeDark.Name = "radioButtonThemeDark";
            radioButtonThemeDark.Size = new Size(49, 19);
            radioButtonThemeDark.TabIndex = 1;
            radioButtonThemeDark.Text = "Dark";
            radioButtonThemeDark.UseVisualStyleBackColor = true;
            // 
            // radioButtonThemeLight
            // 
            radioButtonThemeLight.AutoSize = true;
            radioButtonThemeLight.Checked = true;
            radioButtonThemeLight.Location = new Point(6, 22);
            radioButtonThemeLight.Name = "radioButtonThemeLight";
            radioButtonThemeLight.Size = new Size(52, 19);
            radioButtonThemeLight.TabIndex = 0;
            radioButtonThemeLight.TabStop = true;
            radioButtonThemeLight.Text = "Light";
            radioButtonThemeLight.UseVisualStyleBackColor = true;
            // 
            // tabPageNotifications
            // 
            tabPageNotifications.Controls.Add(checkBoxFlashWindowWhenMessageReceived);
            tabPageNotifications.Controls.Add(checkBoxPlaySoundWhenMessageReceived);
            tabPageNotifications.Controls.Add(checkBoxPlaySoundWhenContactComesOnline);
            tabPageNotifications.Controls.Add(textBoxAutoAwayIdleSeconds);
            tabPageNotifications.Controls.Add(checkBoxAlertToastWhenMessageReceived);
            tabPageNotifications.Controls.Add(labelAutoAwayIdleSeconds);
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
            checkBoxPlaySoundWhenContactComesOnline.Size = new Size(209, 19);
            checkBoxPlaySoundWhenContactComesOnline.TabIndex = 2;
            checkBoxPlaySoundWhenContactComesOnline.Text = "Sound when contact comes online";
            checkBoxPlaySoundWhenContactComesOnline.UseVisualStyleBackColor = true;
            // 
            // textBoxAutoAwayIdleSeconds
            // 
            textBoxAutoAwayIdleSeconds.Location = new Point(6, 157);
            textBoxAutoAwayIdleSeconds.Name = "textBoxAutoAwayIdleSeconds";
            textBoxAutoAwayIdleSeconds.Size = new Size(117, 23);
            textBoxAutoAwayIdleSeconds.TabIndex = 5;
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
            // labelAutoAwayIdleSeconds
            // 
            labelAutoAwayIdleSeconds.AutoSize = true;
            labelAutoAwayIdleSeconds.Location = new Point(6, 139);
            labelAutoAwayIdleSeconds.Name = "labelAutoAwayIdleSeconds";
            labelAutoAwayIdleSeconds.Size = new Size(114, 15);
            labelAutoAwayIdleSeconds.TabIndex = 0;
            labelAutoAwayIdleSeconds.Text = "Auto-Away Seconds";
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
            tabPageAdvanced.Controls.Add(textBoxFileTransferChunkSize);
            tabPageAdvanced.Controls.Add(textBoxMaxMessages);
            tabPageAdvanced.Controls.Add(labelFileTransferChunkSize);
            tabPageAdvanced.Controls.Add(labelMaxMessages);
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
            // textBoxFileTransferChunkSize
            // 
            textBoxFileTransferChunkSize.Location = new Point(8, 70);
            textBoxFileTransferChunkSize.Name = "textBoxFileTransferChunkSize";
            textBoxFileTransferChunkSize.Size = new Size(117, 23);
            textBoxFileTransferChunkSize.TabIndex = 3;
            // 
            // textBoxMaxMessages
            // 
            textBoxMaxMessages.Location = new Point(8, 23);
            textBoxMaxMessages.Name = "textBoxMaxMessages";
            textBoxMaxMessages.Size = new Size(117, 23);
            textBoxMaxMessages.TabIndex = 1;
            // 
            // labelFileTransferChunkSize
            // 
            labelFileTransferChunkSize.AutoSize = true;
            labelFileTransferChunkSize.Location = new Point(8, 52);
            labelFileTransferChunkSize.Name = "labelFileTransferChunkSize";
            labelFileTransferChunkSize.Size = new Size(86, 15);
            labelFileTransferChunkSize.TabIndex = 2;
            labelFileTransferChunkSize.Text = "File Chunk Size";
            // 
            // labelMaxMessages
            // 
            labelMaxMessages.AutoSize = true;
            labelMaxMessages.Location = new Point(8, 5);
            labelMaxMessages.Name = "labelMaxMessages";
            labelMaxMessages.Size = new Size(83, 15);
            labelMaxMessages.TabIndex = 0;
            labelMaxMessages.Text = "Max Messages";
            // 
            // tabPageCryptogrphy
            // 
            tabPageCryptogrphy.Controls.Add(labelRsaKeySize);
            tabPageCryptogrphy.Controls.Add(textBoxEndToEndKeySize);
            tabPageCryptogrphy.Controls.Add(textBoxAesKeySize);
            tabPageCryptogrphy.Controls.Add(labelEndToEndKeySize);
            tabPageCryptogrphy.Controls.Add(labelAesKeySize);
            tabPageCryptogrphy.Controls.Add(textBoxRsaKeySize);
            tabPageCryptogrphy.Location = new Point(4, 24);
            tabPageCryptogrphy.Name = "tabPageCryptogrphy";
            tabPageCryptogrphy.Padding = new Padding(3);
            tabPageCryptogrphy.Size = new Size(440, 259);
            tabPageCryptogrphy.TabIndex = 4;
            tabPageCryptogrphy.Text = "Cryptogrphy";
            tabPageCryptogrphy.UseVisualStyleBackColor = true;
            // 
            // labelRsaKeySize
            // 
            labelRsaKeySize.AutoSize = true;
            labelRsaKeySize.Location = new Point(6, 6);
            labelRsaKeySize.Name = "labelRsaKeySize";
            labelRsaKeySize.Size = new Size(72, 15);
            labelRsaKeySize.TabIndex = 0;
            labelRsaKeySize.Text = "RSA Key Bits";
            // 
            // textBoxEndToEndKeySize
            // 
            textBoxEndToEndKeySize.Location = new Point(6, 118);
            textBoxEndToEndKeySize.Name = "textBoxEndToEndKeySize";
            textBoxEndToEndKeySize.Size = new Size(122, 23);
            textBoxEndToEndKeySize.TabIndex = 5;
            // 
            // textBoxAesKeySize
            // 
            textBoxAesKeySize.Location = new Point(6, 71);
            textBoxAesKeySize.Name = "textBoxAesKeySize";
            textBoxAesKeySize.Size = new Size(122, 23);
            textBoxAesKeySize.TabIndex = 3;
            // 
            // labelEndToEndKeySize
            // 
            labelEndToEndKeySize.AutoSize = true;
            labelEndToEndKeySize.Location = new Point(6, 100);
            labelEndToEndKeySize.Name = "labelEndToEndKeySize";
            labelEndToEndKeySize.Size = new Size(112, 15);
            labelEndToEndKeySize.TabIndex = 4;
            labelEndToEndKeySize.Text = "End-to-End Key Bits";
            // 
            // labelAesKeySize
            // 
            labelAesKeySize.AutoSize = true;
            labelAesKeySize.Location = new Point(6, 53);
            labelAesKeySize.Name = "labelAesKeySize";
            labelAesKeySize.Size = new Size(71, 15);
            labelAesKeySize.TabIndex = 2;
            labelAesKeySize.Text = "AES Key Bits";
            // 
            // textBoxRsaKeySize
            // 
            textBoxRsaKeySize.Location = new Point(6, 24);
            textBoxRsaKeySize.Name = "textBoxRsaKeySize";
            textBoxRsaKeySize.Size = new Size(122, 23);
            textBoxRsaKeySize.TabIndex = 1;
            // 
            // FormSettings
            // 
            ClientSize = new Size(467, 334);
            Controls.Add(tabControlBody);
            Controls.Add(buttonCancel);
            Controls.Add(buttonSave);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FormSettings";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Secure Chat";
            tabControlBody.ResumeLayout(false);
            tabPageMessages.ResumeLayout(false);
            tabPageMessages.PerformLayout();
            ((ISupportInitialize)numericUpDownFontSize).EndInit();
            tabPageUI.ResumeLayout(false);
            groupBoxTheme.ResumeLayout(false);
            groupBoxTheme.PerformLayout();
            tabPageNotifications.ResumeLayout(false);
            tabPageNotifications.PerformLayout();
            tabPageServer.ResumeLayout(false);
            tabPageServer.PerformLayout();
            tabPageAdvanced.ResumeLayout(false);
            tabPageAdvanced.PerformLayout();
            tabPageCryptogrphy.ResumeLayout(false);
            tabPageCryptogrphy.PerformLayout();
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
        private TextBox textBoxFileTransferChunkSize;
        private TextBox textBoxMaxMessages;
        private TextBox textBoxAutoAwayIdleSeconds;
        private Label labelFileTransferChunkSize;
        private Label labelMaxMessages;
        private Label labelAutoAwayIdleSeconds;
        private CheckBox checkBoxAutoStartAtWindowsLogin;
        private TabPage tabPageNotifications;
        private CheckBox checkBoxFlashWindowWhenMessageReceived;
        private CheckBox checkBoxPlaySoundWhenMessageReceived;
        private CheckBox checkBoxPlaySoundWhenContactComesOnline;
        private CheckBox checkBoxAlertToastWhenMessageReceived;
        private CheckBox checkBoxAlertToastWhenContactComesOnline;
        private TabPage tabPageCryptogrphy;
        private Label labelRsaKeySize;
        private TextBox textBoxEndToEndKeySize;
        private TextBox textBoxAesKeySize;
        private Label labelEndToEndKeySize;
        private Label labelAesKeySize;
        private TextBox textBoxRsaKeySize;
        private TabPage tabPageUI;
        private GroupBox groupBoxTheme;
        private RadioButton radioButtonThemeDark;
        private RadioButton radioButtonThemeLight;
    }
}
