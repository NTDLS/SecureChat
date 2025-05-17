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

namespace Talkster.Client.Forms
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
            textBoxAutoAwayIdleMinutes = new KryptonTextBox();
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
            kryptonNavigator = new Krypton.Navigator.KryptonNavigator();
            kryptonPageNotifications = new Krypton.Navigator.KryptonPage();
            textBoxToastTimeoutSeconds = new KryptonTextBox();
            labelToastTimeoutSeconds = new KryptonLabel();
            checkBoxAlertToastErrorMessages = new KryptonCheckBox();
            checkBoxAlertToastWhenMyOnlineStatusChanges = new KryptonCheckBox();
            kryptonPageMessages = new Krypton.Navigator.KryptonPage();
            kryptonPageUI = new Krypton.Navigator.KryptonPage();
            kryptonComboBoxTheme = new KryptonComboBox();
            labelTheme = new KryptonLabel();
            kryptonPageServer = new Krypton.Navigator.KryptonPage();
            kryptonPageAdvanced = new Krypton.Navigator.KryptonPage();
            kryptonPageCryptography = new Krypton.Navigator.KryptonPage();
            splitContainer1 = new SplitContainer();
            ((ISupportInitialize)comboBoxFont).BeginInit();
            ((ISupportInitialize)kryptonNavigator).BeginInit();
            ((ISupportInitialize)kryptonPageNotifications).BeginInit();
            kryptonPageNotifications.SuspendLayout();
            ((ISupportInitialize)kryptonPageMessages).BeginInit();
            kryptonPageMessages.SuspendLayout();
            ((ISupportInitialize)kryptonPageUI).BeginInit();
            kryptonPageUI.SuspendLayout();
            ((ISupportInitialize)kryptonComboBoxTheme).BeginInit();
            ((ISupportInitialize)kryptonPageServer).BeginInit();
            kryptonPageServer.SuspendLayout();
            ((ISupportInitialize)kryptonPageAdvanced).BeginInit();
            kryptonPageAdvanced.SuspendLayout();
            ((ISupportInitialize)kryptonPageCryptography).BeginInit();
            kryptonPageCryptography.SuspendLayout();
            ((ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(281, 8);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(75, 23);
            buttonSave.TabIndex = 3;
            buttonSave.Values.DropDownArrowColor = Color.Empty;
            buttonSave.Values.Text = "Save";
            buttonSave.Click += ButtonSave_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(362, 8);
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
            textBoxFontSample.Size = new Size(424, 205);
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
            numericUpDownFontSize.Location = new Point(367, 22);
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
            comboBoxFont.Size = new Size(356, 22);
            comboBoxFont.TabIndex = 0;
            // 
            // checkBoxFlashWindowWhenMessageReceived
            // 
            checkBoxFlashWindowWhenMessageReceived.Location = new Point(11, 57);
            checkBoxFlashWindowWhenMessageReceived.Name = "checkBoxFlashWindowWhenMessageReceived";
            checkBoxFlashWindowWhenMessageReceived.Size = new Size(241, 20);
            checkBoxFlashWindowWhenMessageReceived.TabIndex = 0;
            checkBoxFlashWindowWhenMessageReceived.Values.Text = "Flash window when message is received";
            // 
            // checkBoxPlaySoundWhenMessageReceived
            // 
            checkBoxPlaySoundWhenMessageReceived.Location = new Point(11, 31);
            checkBoxPlaySoundWhenMessageReceived.Name = "checkBoxPlaySoundWhenMessageReceived";
            checkBoxPlaySoundWhenMessageReceived.Size = new Size(226, 20);
            checkBoxPlaySoundWhenMessageReceived.TabIndex = 2;
            checkBoxPlaySoundWhenMessageReceived.Values.Text = "Audible alert when message received";
            // 
            // checkBoxPlaySoundWhenContactComesOnline
            // 
            checkBoxPlaySoundWhenContactComesOnline.Location = new Point(11, 5);
            checkBoxPlaySoundWhenContactComesOnline.Name = "checkBoxPlaySoundWhenContactComesOnline";
            checkBoxPlaySoundWhenContactComesOnline.Size = new Size(245, 20);
            checkBoxPlaySoundWhenContactComesOnline.TabIndex = 1;
            checkBoxPlaySoundWhenContactComesOnline.Values.Text = "Audible alert when contact comes online";
            // 
            // textBoxAutoAwayIdleMinutes
            // 
            textBoxAutoAwayIdleMinutes.Location = new Point(284, 72);
            textBoxAutoAwayIdleMinutes.Name = "textBoxAutoAwayIdleMinutes";
            textBoxAutoAwayIdleMinutes.Size = new Size(100, 23);
            textBoxAutoAwayIdleMinutes.TabIndex = 8;
            // 
            // checkBoxAlertToastWhenMessageReceived
            // 
            checkBoxAlertToastWhenMessageReceived.Location = new Point(11, 135);
            checkBoxAlertToastWhenMessageReceived.Name = "checkBoxAlertToastWhenMessageReceived";
            checkBoxAlertToastWhenMessageReceived.Size = new Size(228, 20);
            checkBoxAlertToastWhenMessageReceived.TabIndex = 5;
            checkBoxAlertToastWhenMessageReceived.Values.Text = "Visual alert when message is received";
            // 
            // labelAutoAwayIdleSeconds
            // 
            labelAutoAwayIdleSeconds.Location = new Point(280, 54);
            labelAutoAwayIdleSeconds.Name = "labelAutoAwayIdleSeconds";
            labelAutoAwayIdleSeconds.Size = new Size(152, 20);
            labelAutoAwayIdleSeconds.TabIndex = 7;
            labelAutoAwayIdleSeconds.Values.Text = "Auto-away after (minutes)";
            // 
            // checkBoxAlertToastWhenContactComesOnline
            // 
            checkBoxAlertToastWhenContactComesOnline.Location = new Point(11, 109);
            checkBoxAlertToastWhenContactComesOnline.Name = "checkBoxAlertToastWhenContactComesOnline";
            checkBoxAlertToastWhenContactComesOnline.Size = new Size(236, 20);
            checkBoxAlertToastWhenContactComesOnline.TabIndex = 4;
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
            textBoxServerPort.Location = new Point(224, 25);
            textBoxServerPort.Name = "textBoxServerPort";
            textBoxServerPort.Size = new Size(80, 23);
            textBoxServerPort.TabIndex = 1;
            // 
            // textBoxServerAddress
            // 
            textBoxServerAddress.Location = new Point(7, 25);
            textBoxServerAddress.Name = "textBoxServerAddress";
            textBoxServerAddress.Size = new Size(211, 23);
            textBoxServerAddress.TabIndex = 0;
            // 
            // checkBoxAutoStartAtWindowsLogin
            // 
            checkBoxAutoStartAtWindowsLogin.Location = new Point(7, 54);
            checkBoxAutoStartAtWindowsLogin.Name = "checkBoxAutoStartAtWindowsLogin";
            checkBoxAutoStartAtWindowsLogin.Size = new Size(180, 20);
            checkBoxAutoStartAtWindowsLogin.TabIndex = 2;
            checkBoxAutoStartAtWindowsLogin.Values.Text = "Auto-start at windows login?";
            // 
            // textBoxFileTransferChunkSize
            // 
            textBoxFileTransferChunkSize.Location = new Point(7, 71);
            textBoxFileTransferChunkSize.Name = "textBoxFileTransferChunkSize";
            textBoxFileTransferChunkSize.Size = new Size(117, 23);
            textBoxFileTransferChunkSize.TabIndex = 3;
            // 
            // textBoxMaxMessages
            // 
            textBoxMaxMessages.Location = new Point(7, 23);
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
            textBoxEndToEndKeySize.Location = new Point(7, 120);
            textBoxEndToEndKeySize.Name = "textBoxEndToEndKeySize";
            textBoxEndToEndKeySize.Size = new Size(122, 23);
            textBoxEndToEndKeySize.TabIndex = 5;
            // 
            // textBoxAesKeySize
            // 
            textBoxAesKeySize.Location = new Point(7, 71);
            textBoxAesKeySize.Name = "textBoxAesKeySize";
            textBoxAesKeySize.Size = new Size(122, 23);
            textBoxAesKeySize.TabIndex = 3;
            // 
            // labelEndToEndKeySize
            // 
            labelEndToEndKeySize.Location = new Point(3, 100);
            labelEndToEndKeySize.Name = "labelEndToEndKeySize";
            labelEndToEndKeySize.Size = new Size(119, 20);
            labelEndToEndKeySize.TabIndex = 4;
            labelEndToEndKeySize.Values.Text = "End-to-End Key Bits";
            // 
            // labelAesKeySize
            // 
            labelAesKeySize.Location = new Point(3, 51);
            labelAesKeySize.Name = "labelAesKeySize";
            labelAesKeySize.Size = new Size(77, 20);
            labelAesKeySize.TabIndex = 2;
            labelAesKeySize.Values.Text = "AES Key Bits";
            // 
            // textBoxRsaKeySize
            // 
            textBoxRsaKeySize.Location = new Point(7, 23);
            textBoxRsaKeySize.Name = "textBoxRsaKeySize";
            textBoxRsaKeySize.Size = new Size(122, 23);
            textBoxRsaKeySize.TabIndex = 1;
            // 
            // kryptonNavigator
            // 
            kryptonNavigator.AllowPageDrag = true;
            kryptonNavigator.AllowPageReorder = false;
            kryptonNavigator.Button.ButtonDisplayLogic = Krypton.Navigator.ButtonDisplayLogic.Context;
            kryptonNavigator.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.RemovePageAndDispose;
            kryptonNavigator.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            kryptonNavigator.Button.ContextButtonAction = Krypton.Navigator.ContextButtonAction.SelectPage;
            kryptonNavigator.Button.ContextButtonDisplay = Krypton.Navigator.ButtonDisplay.Logic;
            kryptonNavigator.Button.ContextMenuMapImage = Krypton.Navigator.MapKryptonPageImage.Small;
            kryptonNavigator.Button.ContextMenuMapText = Krypton.Navigator.MapKryptonPageText.TextTitle;
            kryptonNavigator.Button.NextButtonAction = Krypton.Navigator.DirectionButtonAction.ModeAppropriateAction;
            kryptonNavigator.Button.NextButtonDisplay = Krypton.Navigator.ButtonDisplay.Logic;
            kryptonNavigator.Button.PreviousButtonAction = Krypton.Navigator.DirectionButtonAction.ModeAppropriateAction;
            kryptonNavigator.Button.PreviousButtonDisplay = Krypton.Navigator.ButtonDisplay.Logic;
            kryptonNavigator.ControlKryptonFormFeatures = false;
            kryptonNavigator.Dock = DockStyle.Fill;
            kryptonNavigator.Location = new Point(0, 0);
            kryptonNavigator.NavigatorMode = Krypton.Navigator.NavigatorMode.BarRibbonTabGroup;
            kryptonNavigator.Owner = null;
            kryptonNavigator.PageBackStyle = PaletteBackStyle.PanelClient;
            kryptonNavigator.Pages.AddRange(new Krypton.Navigator.KryptonPage[] { kryptonPageNotifications, kryptonPageMessages, kryptonPageUI, kryptonPageServer, kryptonPageAdvanced, kryptonPageCryptography });
            kryptonNavigator.PopupPages.AllowPopupPages = Krypton.Navigator.PopupPageAllow.Never;
            kryptonNavigator.PopupPages.Element = Krypton.Navigator.PopupPageElement.Item;
            kryptonNavigator.PopupPages.Position = Krypton.Navigator.PopupPagePosition.ModeAppropriate;
            kryptonNavigator.SelectedIndex = 0;
            kryptonNavigator.Size = new Size(468, 288);
            kryptonNavigator.TabIndex = 5;
            // 
            // kryptonPageNotifications
            // 
            kryptonPageNotifications.AutoHiddenSlideSize = new Size(200, 200);
            kryptonPageNotifications.Controls.Add(textBoxToastTimeoutSeconds);
            kryptonPageNotifications.Controls.Add(labelToastTimeoutSeconds);
            kryptonPageNotifications.Controls.Add(checkBoxAlertToastErrorMessages);
            kryptonPageNotifications.Controls.Add(checkBoxAlertToastWhenMyOnlineStatusChanges);
            kryptonPageNotifications.Controls.Add(checkBoxFlashWindowWhenMessageReceived);
            kryptonPageNotifications.Controls.Add(checkBoxAlertToastWhenContactComesOnline);
            kryptonPageNotifications.Controls.Add(checkBoxPlaySoundWhenMessageReceived);
            kryptonPageNotifications.Controls.Add(checkBoxPlaySoundWhenContactComesOnline);
            kryptonPageNotifications.Controls.Add(checkBoxAlertToastWhenMessageReceived);
            kryptonPageNotifications.Controls.Add(textBoxAutoAwayIdleMinutes);
            kryptonPageNotifications.Controls.Add(labelAutoAwayIdleSeconds);
            kryptonPageNotifications.Flags = 65534;
            kryptonPageNotifications.LastVisibleSet = true;
            kryptonPageNotifications.MinimumSize = new Size(150, 50);
            kryptonPageNotifications.Name = "kryptonPageNotifications";
            kryptonPageNotifications.Size = new Size(466, 259);
            kryptonPageNotifications.Text = "Notifications";
            kryptonPageNotifications.TextDescription = "Notifications";
            kryptonPageNotifications.TextTitle = "Notifications";
            kryptonPageNotifications.ToolTipTitle = "Notifications";
            kryptonPageNotifications.UniqueName = "c7d547960feb40a6ab6e1917d01c78ac";
            // 
            // textBoxToastTimeoutSeconds
            // 
            textBoxToastTimeoutSeconds.Location = new Point(284, 25);
            textBoxToastTimeoutSeconds.Name = "textBoxToastTimeoutSeconds";
            textBoxToastTimeoutSeconds.Size = new Size(100, 23);
            textBoxToastTimeoutSeconds.TabIndex = 7;
            // 
            // labelToastTimeoutSeconds
            // 
            labelToastTimeoutSeconds.Location = new Point(280, 5);
            labelToastTimeoutSeconds.Name = "labelToastTimeoutSeconds";
            labelToastTimeoutSeconds.Size = new Size(175, 20);
            labelToastTimeoutSeconds.TabIndex = 7;
            labelToastTimeoutSeconds.Values.Text = "Visual alert duration (seconds)";
            // 
            // checkBoxAlertToastErrorMessages
            // 
            checkBoxAlertToastErrorMessages.Location = new Point(11, 83);
            checkBoxAlertToastErrorMessages.Name = "checkBoxAlertToastErrorMessages";
            checkBoxAlertToastErrorMessages.Size = new Size(179, 20);
            checkBoxAlertToastErrorMessages.TabIndex = 3;
            checkBoxAlertToastErrorMessages.Values.Text = "Visual alert on various errors";
            // 
            // checkBoxAlertToastWhenMyOnlineStatusChanges
            // 
            checkBoxAlertToastWhenMyOnlineStatusChanges.Location = new Point(11, 161);
            checkBoxAlertToastWhenMyOnlineStatusChanges.Name = "checkBoxAlertToastWhenMyOnlineStatusChanges";
            checkBoxAlertToastWhenMyOnlineStatusChanges.Size = new Size(257, 20);
            checkBoxAlertToastWhenMyOnlineStatusChanges.TabIndex = 6;
            checkBoxAlertToastWhenMyOnlineStatusChanges.Values.Text = "Visual alert when my online status changes";
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
            kryptonPageMessages.Size = new Size(458, 259);
            kryptonPageMessages.Text = "Messages";
            kryptonPageMessages.TextDescription = "Messages";
            kryptonPageMessages.TextTitle = "Messages";
            kryptonPageMessages.ToolTipTitle = "Messages";
            kryptonPageMessages.UniqueName = "e020213f7d4e4417a1a5ab280b43a603";
            // 
            // kryptonPageUI
            // 
            kryptonPageUI.AutoHiddenSlideSize = new Size(200, 200);
            kryptonPageUI.Controls.Add(checkBoxAutoStartAtWindowsLogin);
            kryptonPageUI.Controls.Add(kryptonComboBoxTheme);
            kryptonPageUI.Controls.Add(labelTheme);
            kryptonPageUI.Flags = 65534;
            kryptonPageUI.LastVisibleSet = true;
            kryptonPageUI.MinimumSize = new Size(150, 50);
            kryptonPageUI.Name = "kryptonPageUI";
            kryptonPageUI.Size = new Size(458, 259);
            kryptonPageUI.Text = "UI";
            kryptonPageUI.TextDescription = "UI";
            kryptonPageUI.TextTitle = "UI";
            kryptonPageUI.ToolTipTitle = "UI";
            kryptonPageUI.UniqueName = "7bf4e67f9d584f3ab4761a9eb625f6eb";
            // 
            // kryptonComboBoxTheme
            // 
            kryptonComboBoxTheme.DropDownStyle = ComboBoxStyle.DropDownList;
            kryptonComboBoxTheme.DropDownWidth = 351;
            kryptonComboBoxTheme.FormattingEnabled = true;
            kryptonComboBoxTheme.Location = new Point(7, 23);
            kryptonComboBoxTheme.Name = "kryptonComboBoxTheme";
            kryptonComboBoxTheme.Size = new Size(413, 22);
            kryptonComboBoxTheme.TabIndex = 1;
            kryptonComboBoxTheme.SelectedIndexChanged += kryptonComboBoxTheme_SelectedIndexChanged;
            // 
            // labelTheme
            // 
            labelTheme.Location = new Point(3, 3);
            labelTheme.Name = "labelTheme";
            labelTheme.Size = new Size(48, 20);
            labelTheme.TabIndex = 2;
            labelTheme.Values.Text = "Theme";
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
            kryptonPageServer.Size = new Size(458, 259);
            kryptonPageServer.Text = "Server";
            kryptonPageServer.TextDescription = "Server";
            kryptonPageServer.TextTitle = "Server";
            kryptonPageServer.ToolTipTitle = "Server";
            kryptonPageServer.UniqueName = "2d61d89bb1d64529b3ad59d7442561b6";
            // 
            // kryptonPageAdvanced
            // 
            kryptonPageAdvanced.AutoHiddenSlideSize = new Size(200, 200);
            kryptonPageAdvanced.Controls.Add(textBoxFileTransferChunkSize);
            kryptonPageAdvanced.Controls.Add(textBoxMaxMessages);
            kryptonPageAdvanced.Controls.Add(labelMaxMessages);
            kryptonPageAdvanced.Controls.Add(labelFileTransferChunkSize);
            kryptonPageAdvanced.Flags = 65534;
            kryptonPageAdvanced.LastVisibleSet = true;
            kryptonPageAdvanced.MinimumSize = new Size(150, 50);
            kryptonPageAdvanced.Name = "kryptonPageAdvanced";
            kryptonPageAdvanced.Size = new Size(458, 259);
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
            kryptonPageCryptography.Size = new Size(458, 259);
            kryptonPageCryptography.Text = "Cryptography";
            kryptonPageCryptography.TextDescription = "Cryptography";
            kryptonPageCryptography.TextTitle = "Cryptography";
            kryptonPageCryptography.ToolTipTitle = "Cryptography";
            kryptonPageCryptography.UniqueName = "7e87f954d3c5433fade882e370a2c59b";
            // 
            // splitContainer1
            // 
            splitContainer1.BackColor = Color.Transparent;
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel2;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(kryptonNavigator);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(buttonSave);
            splitContainer1.Panel2.Controls.Add(buttonCancel);
            splitContainer1.Size = new Size(468, 335);
            splitContainer1.SplitterDistance = 288;
            splitContainer1.TabIndex = 6;
            // 
            // FormSettings
            // 
            ClientSize = new Size(468, 335);
            Controls.Add(splitContainer1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimumSize = new Size(450, 380);
            Name = "FormSettings";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Talkster";
            ((ISupportInitialize)comboBoxFont).EndInit();
            ((ISupportInitialize)kryptonNavigator).EndInit();
            ((ISupportInitialize)kryptonPageNotifications).EndInit();
            kryptonPageNotifications.ResumeLayout(false);
            kryptonPageNotifications.PerformLayout();
            ((ISupportInitialize)kryptonPageMessages).EndInit();
            kryptonPageMessages.ResumeLayout(false);
            kryptonPageMessages.PerformLayout();
            ((ISupportInitialize)kryptonPageUI).EndInit();
            kryptonPageUI.ResumeLayout(false);
            kryptonPageUI.PerformLayout();
            ((ISupportInitialize)kryptonComboBoxTheme).EndInit();
            ((ISupportInitialize)kryptonPageServer).EndInit();
            kryptonPageServer.ResumeLayout(false);
            kryptonPageServer.PerformLayout();
            ((ISupportInitialize)kryptonPageAdvanced).EndInit();
            kryptonPageAdvanced.ResumeLayout(false);
            kryptonPageAdvanced.PerformLayout();
            ((ISupportInitialize)kryptonPageCryptography).EndInit();
            kryptonPageCryptography.ResumeLayout(false);
            kryptonPageCryptography.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
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
        private KryptonTextBox textBoxAutoAwayIdleMinutes;
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
        private Krypton.Navigator.KryptonNavigator kryptonNavigator;
        private Krypton.Navigator.KryptonPage kryptonPageMessages;
        private Krypton.Navigator.KryptonPage kryptonPageUI;
        private Krypton.Navigator.KryptonPage kryptonPageNotifications;
        private Krypton.Navigator.KryptonPage kryptonPageServer;
        private Krypton.Navigator.KryptonPage kryptonPageAdvanced;
        private Krypton.Navigator.KryptonPage kryptonPageCryptography;
        private KryptonComboBox kryptonComboBoxTheme;
        private KryptonLabel labelTheme;
        private SplitContainer splitContainer1;
        private KryptonCheckBox checkBoxAlertToastErrorMessages;
        private KryptonCheckBox checkBoxAlertToastWhenMyOnlineStatusChanges;
        private KryptonTextBox textBoxToastTimeoutSeconds;
        private KryptonLabel labelToastTimeoutSeconds;
    }
}
