using Krypton.Toolkit;
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
    public partial class FormSettings
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
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(FormSettings));
            buttonSave = new KryptonButton();
            buttonCancel = new KryptonButton();
            textBoxFontSample = new KryptonTextBox();
            labelFontAndSize = new KryptonLabel();
            numericUpDownFontSize = new KryptonNumericUpDown();
            comboBoxFont = new KryptonComboBox();
            checkBoxFlashWindowWhenMessageReceived = new KryptonCheckBox();
            checkBoxPlaySoundWhenMessageReceived = new KryptonCheckBox();
            checkBoxPlaySoundWhenContactComesOnline = new KryptonCheckBox();
            textBoxAutoAwayIdleSeconds = new KryptonTextBox();
            checkBoxAlertToastWhenMessageReceived = new KryptonCheckBox();
            labelAutoAwayIdleSeconds = new KryptonLabel();
            checkBoxAlertToastWhenContactComesOnline = new KryptonCheckBox();
            labelServerPort = new KryptonLabel();
            labelServerAddress = new KryptonLabel();
            textBoxServerPort = new KryptonTextBox();
            textBoxServerAddress = new KryptonTextBox();
            checkBoxAutoStartAtWindowsLogin = new KryptonCheckBox();
            textBoxFileTransferChunkSize = new KryptonTextBox();
            textBoxMaxMessages = new KryptonTextBox();
            labelFileTransferChunkSize = new KryptonLabel();
            labelMaxMessages = new KryptonLabel();
            labelRsaKeySize = new KryptonLabel();
            textBoxEndToEndKeySize = new KryptonTextBox();
            textBoxAesKeySize = new KryptonTextBox();
            labelEndToEndKeySize = new KryptonLabel();
            labelAesKeySize = new KryptonLabel();
            textBoxRsaKeySize = new KryptonTextBox();
            kryptonManager1 = new KryptonManager(components);
            kryptonNavigator1 = new Krypton.Navigator.KryptonNavigator();
            kryptonPageMessages = new Krypton.Navigator.KryptonPage();
            kryptonPageUI = new Krypton.Navigator.KryptonPage();
            labelTheme = new KryptonLabel();
            kryptonComboBoxTheme = new KryptonComboBox();
            kryptonPageNotifications = new Krypton.Navigator.KryptonPage();
            kryptonPageServer = new Krypton.Navigator.KryptonPage();
            kryptonPageAdvanced = new Krypton.Navigator.KryptonPage();
            kryptonPageCryptography = new Krypton.Navigator.KryptonPage();
            ((ISupportInitialize)comboBoxFont).BeginInit();
            ((ISupportInitialize)kryptonNavigator1).BeginInit();
            ((ISupportInitialize)kryptonPageMessages).BeginInit();
            kryptonPageMessages.SuspendLayout();
            ((ISupportInitialize)kryptonPageUI).BeginInit();
            kryptonPageUI.SuspendLayout();
            ((ISupportInitialize)kryptonComboBoxTheme).BeginInit();
            ((ISupportInitialize)kryptonPageNotifications).BeginInit();
            kryptonPageNotifications.SuspendLayout();
            ((ISupportInitialize)kryptonPageServer).BeginInit();
            kryptonPageServer.SuspendLayout();
            ((ISupportInitialize)kryptonPageAdvanced).BeginInit();
            kryptonPageAdvanced.SuspendLayout();
            ((ISupportInitialize)kryptonPageCryptography).BeginInit();
            kryptonPageCryptography.SuspendLayout();
            SuspendLayout();
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(288, 317);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(75, 23);
            buttonSave.TabIndex = 3;
            buttonSave.Values.DropDownArrowColor = Color.Empty;
            buttonSave.Values.Text = "Save";
            buttonSave.Click += ButtonSave_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(369, 317);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 4;
            buttonCancel.Values.DropDownArrowColor = Color.Empty;
            buttonCancel.Values.Text = "Cancel";
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // textBoxFontSample
            // 
            textBoxFontSample.Location = new Point(5, 50);
            textBoxFontSample.Multiline = true;
            textBoxFontSample.Name = "textBoxFontSample";
            textBoxFontSample.Size = new Size(417, 205);
            textBoxFontSample.TabIndex = 2;
            // 
            // labelFontAndSize
            // 
            labelFontAndSize.Location = new Point(3, 3);
            labelFontAndSize.Name = "labelFontAndSize";
            labelFontAndSize.Size = new Size(84, 20);
            labelFontAndSize.TabIndex = 3;
            labelFontAndSize.Values.Text = "Font and Size";
            // 
            // numericUpDownFontSize
            // 
            numericUpDownFontSize.AllowDecimals = true;
            numericUpDownFontSize.DecimalPlaces = 2;
            numericUpDownFontSize.Increment = new decimal(new int[] { 25, 0, 0, 131072 });
            numericUpDownFontSize.Location = new Point(360, 22);
            numericUpDownFontSize.Maximum = new decimal(new int[] { 30, 0, 0, 0 });
            numericUpDownFontSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownFontSize.Name = "numericUpDownFontSize";
            numericUpDownFontSize.Size = new Size(62, 22);
            numericUpDownFontSize.TabIndex = 1;
            numericUpDownFontSize.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // comboBoxFont
            // 
            comboBoxFont.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxFont.DropDownWidth = 351;
            comboBoxFont.FormattingEnabled = true;
            comboBoxFont.Location = new Point(5, 22);
            comboBoxFont.Name = "comboBoxFont";
            comboBoxFont.Size = new Size(349, 22);
            comboBoxFont.TabIndex = 0;
            // 
            // checkBoxFlashWindowWhenMessageReceived
            // 
            checkBoxFlashWindowWhenMessageReceived.Location = new Point(9, 103);
            checkBoxFlashWindowWhenMessageReceived.Name = "checkBoxFlashWindowWhenMessageReceived";
            checkBoxFlashWindowWhenMessageReceived.Size = new Size(230, 20);
            checkBoxFlashWindowWhenMessageReceived.TabIndex = 4;
            checkBoxFlashWindowWhenMessageReceived.Values.Text = "Flash window when message received";
            // 
            // checkBoxPlaySoundWhenMessageReceived
            // 
            checkBoxPlaySoundWhenMessageReceived.Location = new Point(9, 78);
            checkBoxPlaySoundWhenMessageReceived.Name = "checkBoxPlaySoundWhenMessageReceived";
            checkBoxPlaySoundWhenMessageReceived.Size = new Size(191, 20);
            checkBoxPlaySoundWhenMessageReceived.TabIndex = 3;
            checkBoxPlaySoundWhenMessageReceived.Values.Text = "Sound when message received";
            // 
            // checkBoxPlaySoundWhenContactComesOnline
            // 
            checkBoxPlaySoundWhenContactComesOnline.Location = new Point(9, 53);
            checkBoxPlaySoundWhenContactComesOnline.Name = "checkBoxPlaySoundWhenContactComesOnline";
            checkBoxPlaySoundWhenContactComesOnline.Size = new Size(211, 20);
            checkBoxPlaySoundWhenContactComesOnline.TabIndex = 2;
            checkBoxPlaySoundWhenContactComesOnline.Values.Text = "Sound when contact comes online";
            // 
            // textBoxAutoAwayIdleSeconds
            // 
            textBoxAutoAwayIdleSeconds.Location = new Point(12, 156);
            textBoxAutoAwayIdleSeconds.Name = "textBoxAutoAwayIdleSeconds";
            textBoxAutoAwayIdleSeconds.Size = new Size(119, 23);
            textBoxAutoAwayIdleSeconds.TabIndex = 5;
            // 
            // checkBoxAlertToastWhenMessageReceived
            // 
            checkBoxAlertToastWhenMessageReceived.Location = new Point(9, 28);
            checkBoxAlertToastWhenMessageReceived.Name = "checkBoxAlertToastWhenMessageReceived";
            checkBoxAlertToastWhenMessageReceived.Size = new Size(216, 20);
            checkBoxAlertToastWhenMessageReceived.TabIndex = 1;
            checkBoxAlertToastWhenMessageReceived.Values.Text = "Visual alert when message received";
            // 
            // labelAutoAwayIdleSeconds
            // 
            labelAutoAwayIdleSeconds.Location = new Point(9, 136);
            labelAutoAwayIdleSeconds.Name = "labelAutoAwayIdleSeconds";
            labelAutoAwayIdleSeconds.Size = new Size(120, 20);
            labelAutoAwayIdleSeconds.TabIndex = 0;
            labelAutoAwayIdleSeconds.Values.Text = "Auto-Away Seconds";
            // 
            // checkBoxAlertToastWhenContactComesOnline
            // 
            checkBoxAlertToastWhenContactComesOnline.Location = new Point(9, 3);
            checkBoxAlertToastWhenContactComesOnline.Name = "checkBoxAlertToastWhenContactComesOnline";
            checkBoxAlertToastWhenContactComesOnline.Size = new Size(236, 20);
            checkBoxAlertToastWhenContactComesOnline.TabIndex = 0;
            checkBoxAlertToastWhenContactComesOnline.Values.Text = "Visual alert when contact comes online";
            // 
            // labelServerPort
            // 
            labelServerPort.Location = new Point(220, 3);
            labelServerPort.Name = "labelServerPort";
            labelServerPort.Size = new Size(71, 20);
            labelServerPort.TabIndex = 3;
            labelServerPort.Values.Text = "Server Port";
            // 
            // labelServerAddress
            // 
            labelServerAddress.Location = new Point(3, 3);
            labelServerAddress.Name = "labelServerAddress";
            labelServerAddress.Size = new Size(92, 20);
            labelServerAddress.TabIndex = 2;
            labelServerAddress.Values.Text = "Server Address";
            // 
            // textBoxServerPort
            // 
            textBoxServerPort.Location = new Point(220, 23);
            textBoxServerPort.Name = "textBoxServerPort";
            textBoxServerPort.Size = new Size(80, 23);
            textBoxServerPort.TabIndex = 1;
            // 
            // textBoxServerAddress
            // 
            textBoxServerAddress.Location = new Point(3, 23);
            textBoxServerAddress.Name = "textBoxServerAddress";
            textBoxServerAddress.Size = new Size(211, 23);
            textBoxServerAddress.TabIndex = 0;
            // 
            // checkBoxAutoStartAtWindowsLogin
            // 
            checkBoxAutoStartAtWindowsLogin.Location = new Point(3, 235);
            checkBoxAutoStartAtWindowsLogin.Name = "checkBoxAutoStartAtWindowsLogin";
            checkBoxAutoStartAtWindowsLogin.Size = new Size(180, 20);
            checkBoxAutoStartAtWindowsLogin.TabIndex = 4;
            checkBoxAutoStartAtWindowsLogin.Values.Text = "Auto-start at windows login?";
            // 
            // textBoxFileTransferChunkSize
            // 
            textBoxFileTransferChunkSize.Location = new Point(3, 71);
            textBoxFileTransferChunkSize.Name = "textBoxFileTransferChunkSize";
            textBoxFileTransferChunkSize.Size = new Size(117, 23);
            textBoxFileTransferChunkSize.TabIndex = 3;
            // 
            // textBoxMaxMessages
            // 
            textBoxMaxMessages.Location = new Point(3, 23);
            textBoxMaxMessages.Name = "textBoxMaxMessages";
            textBoxMaxMessages.Size = new Size(117, 23);
            textBoxMaxMessages.TabIndex = 1;
            // 
            // labelFileTransferChunkSize
            // 
            labelFileTransferChunkSize.Location = new Point(3, 51);
            labelFileTransferChunkSize.Name = "labelFileTransferChunkSize";
            labelFileTransferChunkSize.Size = new Size(92, 20);
            labelFileTransferChunkSize.TabIndex = 2;
            labelFileTransferChunkSize.Values.Text = "File Chunk Size";
            // 
            // labelMaxMessages
            // 
            labelMaxMessages.Location = new Point(3, 3);
            labelMaxMessages.Name = "labelMaxMessages";
            labelMaxMessages.Size = new Size(90, 20);
            labelMaxMessages.TabIndex = 0;
            labelMaxMessages.Values.Text = "Max Messages";
            // 
            // labelRsaKeySize
            // 
            labelRsaKeySize.Location = new Point(3, 3);
            labelRsaKeySize.Name = "labelRsaKeySize";
            labelRsaKeySize.Size = new Size(78, 20);
            labelRsaKeySize.TabIndex = 0;
            labelRsaKeySize.Values.Text = "RSA Key Bits";
            // 
            // textBoxEndToEndKeySize
            // 
            textBoxEndToEndKeySize.Location = new Point(9, 117);
            textBoxEndToEndKeySize.Name = "textBoxEndToEndKeySize";
            textBoxEndToEndKeySize.Size = new Size(122, 23);
            textBoxEndToEndKeySize.TabIndex = 5;
            // 
            // textBoxAesKeySize
            // 
            textBoxAesKeySize.Location = new Point(7, 69);
            textBoxAesKeySize.Name = "textBoxAesKeySize";
            textBoxAesKeySize.Size = new Size(122, 23);
            textBoxAesKeySize.TabIndex = 3;
            // 
            // labelEndToEndKeySize
            // 
            labelEndToEndKeySize.Location = new Point(4, 97);
            labelEndToEndKeySize.Name = "labelEndToEndKeySize";
            labelEndToEndKeySize.Size = new Size(119, 20);
            labelEndToEndKeySize.TabIndex = 4;
            labelEndToEndKeySize.Values.Text = "End-to-End Key Bits";
            // 
            // labelAesKeySize
            // 
            labelAesKeySize.Location = new Point(4, 50);
            labelAesKeySize.Name = "labelAesKeySize";
            labelAesKeySize.Size = new Size(77, 20);
            labelAesKeySize.TabIndex = 2;
            labelAesKeySize.Values.Text = "AES Key Bits";
            // 
            // textBoxRsaKeySize
            // 
            textBoxRsaKeySize.Location = new Point(9, 22);
            textBoxRsaKeySize.Name = "textBoxRsaKeySize";
            textBoxRsaKeySize.Size = new Size(122, 23);
            textBoxRsaKeySize.TabIndex = 1;
            // 
            // kryptonNavigator1
            // 
            kryptonNavigator1.AllowPageDrag = true;
            kryptonNavigator1.AllowPageReorder = false;
            kryptonNavigator1.Button.ButtonDisplayLogic = Krypton.Navigator.ButtonDisplayLogic.Context;
            kryptonNavigator1.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.RemovePageAndDispose;
            kryptonNavigator1.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            kryptonNavigator1.Button.ContextButtonAction = Krypton.Navigator.ContextButtonAction.SelectPage;
            kryptonNavigator1.Button.ContextButtonDisplay = Krypton.Navigator.ButtonDisplay.Logic;
            kryptonNavigator1.Button.ContextMenuMapImage = Krypton.Navigator.MapKryptonPageImage.Small;
            kryptonNavigator1.Button.ContextMenuMapText = Krypton.Navigator.MapKryptonPageText.TextTitle;
            kryptonNavigator1.Button.NextButtonAction = Krypton.Navigator.DirectionButtonAction.ModeAppropriateAction;
            kryptonNavigator1.Button.NextButtonDisplay = Krypton.Navigator.ButtonDisplay.Logic;
            kryptonNavigator1.Button.PreviousButtonAction = Krypton.Navigator.DirectionButtonAction.ModeAppropriateAction;
            kryptonNavigator1.Button.PreviousButtonDisplay = Krypton.Navigator.ButtonDisplay.Logic;
            kryptonNavigator1.ControlKryptonFormFeatures = false;
            kryptonNavigator1.Location = new Point(11, 12);
            kryptonNavigator1.NavigatorMode = Krypton.Navigator.NavigatorMode.BarRibbonTabGroup;
            kryptonNavigator1.Owner = null;
            kryptonNavigator1.PageBackStyle = PaletteBackStyle.PanelClient;
            kryptonNavigator1.Pages.AddRange(new Krypton.Navigator.KryptonPage[] { kryptonPageMessages, kryptonPageUI, kryptonPageNotifications, kryptonPageServer, kryptonPageAdvanced, kryptonPageCryptography });
            kryptonNavigator1.SelectedIndex = 1;
            kryptonNavigator1.Size = new Size(433, 295);
            kryptonNavigator1.TabIndex = 5;
            kryptonNavigator1.Text = "kryptonNavigator1";
            // 
            // kryptonPageMessages
            // 
            kryptonPageMessages.AutoHiddenSlideSize = new Size(200, 200);
            kryptonPageMessages.Controls.Add(textBoxFontSample);
            kryptonPageMessages.Controls.Add(comboBoxFont);
            kryptonPageMessages.Controls.Add(numericUpDownFontSize);
            kryptonPageMessages.Controls.Add(labelFontAndSize);
            kryptonPageMessages.Flags = 65534;
            kryptonPageMessages.LastVisibleSet = true;
            kryptonPageMessages.MinimumSize = new Size(150, 50);
            kryptonPageMessages.Name = "kryptonPageMessages";
            kryptonPageMessages.Size = new Size(431, 266);
            kryptonPageMessages.Text = "Messages";
            kryptonPageMessages.TextDescription = "Messages";
            kryptonPageMessages.TextTitle = "Messages";
            kryptonPageMessages.ToolTipTitle = "Messages";
            kryptonPageMessages.UniqueName = "e020213f7d4e4417a1a5ab280b43a603";
            // 
            // kryptonPageUI
            // 
            kryptonPageUI.AutoHiddenSlideSize = new Size(200, 200);
            kryptonPageUI.Controls.Add(labelTheme);
            kryptonPageUI.Controls.Add(kryptonComboBoxTheme);
            kryptonPageUI.Flags = 65534;
            kryptonPageUI.LastVisibleSet = true;
            kryptonPageUI.MinimumSize = new Size(150, 50);
            kryptonPageUI.Name = "kryptonPageUI";
            kryptonPageUI.Size = new Size(431, 266);
            kryptonPageUI.Text = "UI";
            kryptonPageUI.TextDescription = "UI";
            kryptonPageUI.TextTitle = "UI";
            kryptonPageUI.ToolTipTitle = "UI";
            kryptonPageUI.UniqueName = "7bf4e67f9d584f3ab4761a9eb625f6eb";
            // 
            // labelTheme
            // 
            labelTheme.Location = new Point(5, 3);
            labelTheme.Name = "labelTheme";
            labelTheme.Size = new Size(48, 20);
            labelTheme.TabIndex = 2;
            labelTheme.Values.Text = "Theme";
            // 
            // kryptonComboBoxTheme
            // 
            kryptonComboBoxTheme.DropDownStyle = ComboBoxStyle.DropDownList;
            kryptonComboBoxTheme.DropDownWidth = 351;
            kryptonComboBoxTheme.FormattingEnabled = true;
            kryptonComboBoxTheme.Location = new Point(9, 22);
            kryptonComboBoxTheme.Name = "kryptonComboBoxTheme";
            kryptonComboBoxTheme.Size = new Size(413, 22);
            kryptonComboBoxTheme.TabIndex = 1;
            kryptonComboBoxTheme.SelectedIndexChanged += kryptonComboBoxTheme_SelectedIndexChanged;
            // 
            // kryptonPageNotifications
            // 
            kryptonPageNotifications.AutoHiddenSlideSize = new Size(200, 200);
            kryptonPageNotifications.Controls.Add(checkBoxFlashWindowWhenMessageReceived);
            kryptonPageNotifications.Controls.Add(checkBoxAlertToastWhenContactComesOnline);
            kryptonPageNotifications.Controls.Add(checkBoxPlaySoundWhenMessageReceived);
            kryptonPageNotifications.Controls.Add(checkBoxPlaySoundWhenContactComesOnline);
            kryptonPageNotifications.Controls.Add(checkBoxAlertToastWhenMessageReceived);
            kryptonPageNotifications.Controls.Add(textBoxAutoAwayIdleSeconds);
            kryptonPageNotifications.Controls.Add(labelAutoAwayIdleSeconds);
            kryptonPageNotifications.Flags = 65534;
            kryptonPageNotifications.LastVisibleSet = true;
            kryptonPageNotifications.MinimumSize = new Size(150, 50);
            kryptonPageNotifications.Name = "kryptonPageNotifications";
            kryptonPageNotifications.Size = new Size(442, 260);
            kryptonPageNotifications.Text = "Notifications";
            kryptonPageNotifications.TextDescription = "Notifications";
            kryptonPageNotifications.TextTitle = "Notifications";
            kryptonPageNotifications.ToolTipTitle = "Notifications";
            kryptonPageNotifications.UniqueName = "c7d547960feb40a6ab6e1917d01c78ac";
            // 
            // kryptonPageServer
            // 
            kryptonPageServer.AutoHiddenSlideSize = new Size(200, 200);
            kryptonPageServer.Controls.Add(textBoxServerAddress);
            kryptonPageServer.Controls.Add(textBoxServerPort);
            kryptonPageServer.Controls.Add(labelServerPort);
            kryptonPageServer.Controls.Add(labelServerAddress);
            kryptonPageServer.Flags = 65534;
            kryptonPageServer.LastVisibleSet = true;
            kryptonPageServer.MinimumSize = new Size(150, 50);
            kryptonPageServer.Name = "kryptonPageServer";
            kryptonPageServer.Size = new Size(442, 260);
            kryptonPageServer.Text = "Server";
            kryptonPageServer.TextDescription = "Server";
            kryptonPageServer.TextTitle = "Server";
            kryptonPageServer.ToolTipTitle = "Server";
            kryptonPageServer.UniqueName = "2d61d89bb1d64529b3ad59d7442561b6";
            // 
            // kryptonPageAdvanced
            // 
            kryptonPageAdvanced.AutoHiddenSlideSize = new Size(200, 200);
            kryptonPageAdvanced.Controls.Add(checkBoxAutoStartAtWindowsLogin);
            kryptonPageAdvanced.Controls.Add(textBoxFileTransferChunkSize);
            kryptonPageAdvanced.Controls.Add(textBoxMaxMessages);
            kryptonPageAdvanced.Controls.Add(labelMaxMessages);
            kryptonPageAdvanced.Controls.Add(labelFileTransferChunkSize);
            kryptonPageAdvanced.Flags = 65534;
            kryptonPageAdvanced.LastVisibleSet = true;
            kryptonPageAdvanced.MinimumSize = new Size(150, 50);
            kryptonPageAdvanced.Name = "kryptonPageAdvanced";
            kryptonPageAdvanced.Size = new Size(442, 260);
            kryptonPageAdvanced.Text = "Advanced";
            kryptonPageAdvanced.TextDescription = "Advanced";
            kryptonPageAdvanced.TextTitle = "Advanced";
            kryptonPageAdvanced.ToolTipTitle = "Advanced";
            kryptonPageAdvanced.UniqueName = "a22720113dbb43ada04c650390e2a0b3";
            // 
            // kryptonPageCryptography
            // 
            kryptonPageCryptography.AutoHiddenSlideSize = new Size(200, 200);
            kryptonPageCryptography.Controls.Add(textBoxEndToEndKeySize);
            kryptonPageCryptography.Controls.Add(textBoxRsaKeySize);
            kryptonPageCryptography.Controls.Add(textBoxAesKeySize);
            kryptonPageCryptography.Controls.Add(labelAesKeySize);
            kryptonPageCryptography.Controls.Add(labelEndToEndKeySize);
            kryptonPageCryptography.Controls.Add(labelRsaKeySize);
            kryptonPageCryptography.Flags = 65534;
            kryptonPageCryptography.LastVisibleSet = true;
            kryptonPageCryptography.MinimumSize = new Size(150, 50);
            kryptonPageCryptography.Name = "kryptonPageCryptography";
            kryptonPageCryptography.Size = new Size(442, 260);
            kryptonPageCryptography.Text = "Cryptography";
            kryptonPageCryptography.TextDescription = "Cryptography";
            kryptonPageCryptography.TextTitle = "Cryptography";
            kryptonPageCryptography.ToolTipTitle = "Cryptography";
            kryptonPageCryptography.UniqueName = "7e87f954d3c5433fade882e370a2c59b";
            // 
            // FormSettings
            // 
            ClientSize = new Size(469, 314);
            Controls.Add(kryptonNavigator1);
            Controls.Add(buttonCancel);
            Controls.Add(buttonSave);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FormSettings";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Secure Chat";
            ((ISupportInitialize)comboBoxFont).EndInit();
            ((ISupportInitialize)kryptonNavigator1).EndInit();
            ((ISupportInitialize)kryptonPageMessages).EndInit();
            kryptonPageMessages.ResumeLayout(false);
            kryptonPageMessages.PerformLayout();
            ((ISupportInitialize)kryptonPageUI).EndInit();
            kryptonPageUI.ResumeLayout(false);
            kryptonPageUI.PerformLayout();
            ((ISupportInitialize)kryptonComboBoxTheme).EndInit();
            ((ISupportInitialize)kryptonPageNotifications).EndInit();
            kryptonPageNotifications.ResumeLayout(false);
            kryptonPageNotifications.PerformLayout();
            ((ISupportInitialize)kryptonPageServer).EndInit();
            kryptonPageServer.ResumeLayout(false);
            kryptonPageServer.PerformLayout();
            ((ISupportInitialize)kryptonPageAdvanced).EndInit();
            kryptonPageAdvanced.ResumeLayout(false);
            kryptonPageAdvanced.PerformLayout();
            ((ISupportInitialize)kryptonPageCryptography).EndInit();
            kryptonPageCryptography.ResumeLayout(false);
            kryptonPageCryptography.PerformLayout();
            ResumeLayout(false);
        }
        private KryptonButton buttonSave;
        private KryptonButton buttonCancel;
        private KryptonComboBox comboBoxFont;
        private KryptonNumericUpDown numericUpDownFontSize;
        private KryptonLabel labelFontAndSize;
        private KryptonTextBox textBoxFontSample;
        private KryptonLabel labelServerPort;
        private KryptonLabel labelServerAddress;
        private KryptonTextBox textBoxServerPort;
        private KryptonTextBox textBoxServerAddress;
        private KryptonTextBox textBoxFileTransferChunkSize;
        private KryptonTextBox textBoxMaxMessages;
        private KryptonTextBox textBoxAutoAwayIdleSeconds;
        private KryptonLabel labelFileTransferChunkSize;
        private KryptonLabel labelMaxMessages;
        private KryptonLabel labelAutoAwayIdleSeconds;
        private KryptonCheckBox checkBoxAutoStartAtWindowsLogin;
        private KryptonCheckBox checkBoxFlashWindowWhenMessageReceived;
        private KryptonCheckBox checkBoxPlaySoundWhenMessageReceived;
        private KryptonCheckBox checkBoxPlaySoundWhenContactComesOnline;
        private KryptonCheckBox checkBoxAlertToastWhenMessageReceived;
        private KryptonCheckBox checkBoxAlertToastWhenContactComesOnline;
        private KryptonLabel labelRsaKeySize;
        private KryptonTextBox textBoxEndToEndKeySize;
        private KryptonTextBox textBoxAesKeySize;
        private KryptonLabel labelEndToEndKeySize;
        private KryptonLabel labelAesKeySize;
        private KryptonTextBox textBoxRsaKeySize;
        private Krypton.Toolkit.KryptonManager kryptonManager1;
        private Krypton.Navigator.KryptonNavigator kryptonNavigator1;
        private Krypton.Navigator.KryptonPage kryptonPageMessages;
        private Krypton.Navigator.KryptonPage kryptonPageUI;
        private Krypton.Navigator.KryptonPage kryptonPageNotifications;
        private Krypton.Navigator.KryptonPage kryptonPageServer;
        private Krypton.Navigator.KryptonPage kryptonPageAdvanced;
        private Krypton.Navigator.KryptonPage kryptonPageCryptography;
        private KryptonComboBox kryptonComboBoxTheme;
        private KryptonLabel labelTheme;
    }
}
