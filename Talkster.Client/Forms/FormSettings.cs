using Krypton.Toolkit;
using NTDLS.Helpers;
using NTDLS.Persistence;
using NTDLS.WinFormsHelpers;
using Talkster.Client.Controls;
using Talkster.Client.Helpers;
using Talkster.Library;
using System.Diagnostics;

namespace Talkster.Client.Forms
{
    public partial class FormSettings : KryptonForm
    {
        public FormSettings(bool showInTaskbar)
        {
            InitializeComponent();

            BackColor = KryptonManager.CurrentGlobalPalette.GetBackColor1(PaletteBackStyle.PanelClient, PaletteState.Normal);

            kryptonNavigator.SelectedPage = kryptonPageNotifications;

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

            Exceptions.Ignore(() => checkBoxAutoStartAtWindowsLogin.Checked = RegistryHelper.IsAutoStartEnabled());

            textBoxRsaKeySize.Text = $"{Settings.Instance.RsaKeySize:n0}";
            textBoxAesKeySize.Text = $"{Settings.Instance.AesKeySize:n0}";
            textBoxEndToEndKeySize.Text = $"{Settings.Instance.EndToEndKeySize:n0}";
            textBoxServerAddress.Text = Settings.Instance.ServerAddress;
            textBoxServerPort.Text = $"{Settings.Instance.ServerPort:n0}";
            textBoxAutoAwayIdleMinutes.Text = $"{Settings.Instance.AutoAwayIdleMinutes:n0}";
            textBoxMaxMessages.Text = $"{Settings.Instance.MaxMessages:n0}";
            textBoxFileTransferChunkSize.Text = $"{Settings.Instance.FileTransferChunkSize:n0}";
            checkBoxAlertToastWhenContactComesOnline.Checked = Settings.Instance.AlertToastWhenContactComesOnline;
            checkBoxAlertToastWhenMessageReceived.Checked = Settings.Instance.AlertToastWhenMessageReceived;
            checkBoxPlaySoundWhenContactComesOnline.Checked = Settings.Instance.PlaySoundWhenContactComesOnline;
            checkBoxPlaySoundWhenMessageReceived.Checked = Settings.Instance.PlaySoundWhenMessageReceived;
            checkBoxFlashWindowWhenMessageReceived.Checked = Settings.Instance.FlashWindowWhenMessageReceived;
            checkBoxAlertToastWhenMyOnlineStatusChanges.Checked = Settings.Instance.AlertToastWhenMyOnlineStatusChanges;
            checkBoxAlertToastErrorMessages.Checked = Settings.Instance.AlertToastErrorMessages;

            textBoxFontSample.Text = "John: Hey, how's is been going?\r\nJane: Pretty good. I've about to head out.\r\nJohn: Wanna grab some lunch?\r\nJane: Thai?\r\nJohn: Are you kidding me? Absolutely!\r\n";

            foreach (var font in FontFamily.Families.OrderBy(o => o.Name))
            {
                comboBoxFont.Items.Add(font.Name);
            }
            comboBoxFont.Text = Settings.Instance.Font;
            numericUpDownFontSize.Value = (decimal)Settings.Instance.FontSize;

            numericUpDownFontSize.ValueChanged += (object? sender, EventArgs e) => UpdateFontSample();
            comboBoxFont.SelectedIndexChanged += (object? sender, EventArgs e) => UpdateFontSample();

            #region Themes.

            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Cloud Black (Dark)", PaletteMode.Microsoft365BlackDarkMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Cloud Blue (Light)", PaletteMode.Microsoft365Blue));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Cloud Gray (Dark)", PaletteMode.Microsoft365Black));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Cloud Gray (Light)", PaletteMode.Microsoft365Silver));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Cloud Refuge (Dark)", PaletteMode.Microsoft365BlackDarkModeAlternate));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Cloud Silver (Dark)", PaletteMode.Microsoft365SilverDarkMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Cloud Silver (Light)", PaletteMode.Microsoft365SilverLightMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Cloud Stark (Light)", PaletteMode.Microsoft365BlueDarkMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Cloud Vibrant (Light)", PaletteMode.Microsoft365BlueLightMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Cloud White (Light)", PaletteMode.Microsoft365White));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office Classic (Light)", PaletteMode.ProfessionalOffice2003));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office Metro Azure (Light)", PaletteMode.Office2010Blue));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office Metro Black (Dark)", PaletteMode.Office2010BlackDarkMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office Metro Blue (Light)", PaletteMode.Office2010BlueDarkMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office Metro Blue (Light)", PaletteMode.Office2010BlueLightMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office Metro Gray (Dark)", PaletteMode.Office2010Black));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office Metro Silver (Dark)", PaletteMode.Office2010SilverDarkMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office Metro Silver (Light)", PaletteMode.Office2010SilverLightMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office Metro Vibrant (Light)", PaletteMode.Office2010Silver));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office Metro White (Light)", PaletteMode.Office2010White));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office Modern (Light)", PaletteMode.Office2013White));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office Slate Azure (Light)", PaletteMode.Office2007Blue));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office Slate Black (Dark)", PaletteMode.Office2007BlackDarkMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office Slate Blue (Light)", PaletteMode.Office2007BlueLightMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office Slate Gray (Dark)", PaletteMode.Office2007Black));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office Slate Gray (Light)", PaletteMode.Office2007Silver));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office Slate Silver (Dark)", PaletteMode.Office2007SilverDarkMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office Slate Silver (Light)", PaletteMode.Office2007SilverLightMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office Slate Vibrant (Light)", PaletteMode.Office2007BlueDarkMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office Slate White (Light)", PaletteMode.Office2007White));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Professional (Light)", PaletteMode.ProfessionalSystem));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Sparkle Blue (Dark)", PaletteMode.SparkleBlue));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Sparkle Orange (Dark)", PaletteMode.SparkleOrange));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Sparkle Purple (Dark)", PaletteMode.SparklePurple));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Studio Render Cloud (Light)", PaletteMode.VisualStudio2010Render365));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Studio Render Metro (Light)", PaletteMode.VisualStudio2010Render2010));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Studio Render Modern (Light)", PaletteMode.VisualStudio2010Render2013));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Studio Render Slate (Light)", PaletteMode.VisualStudio2010Render2007));

            foreach (var item in kryptonComboBoxTheme.Items)
            {
                if (item is ThemeComboItem themeItem && themeItem.Mode == Settings.Instance.Theme)
                {
                    kryptonComboBoxTheme.SelectedItem = item;
                    break;
                }
            }

            #endregion

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
                        textBoxFontSample.StateCommon.Content.Font = new Font(selectedFontName, (float)fontSize);
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

                settings.Theme = (kryptonComboBoxTheme.SelectedItem as ThemeComboItem)?.Mode
                    ?? throw new ArgumentNullException("Theme must be selected.");

                settings.ServerAddress = textBoxServerAddress.TextBox.GetAndValidateText("Server address must not be empty.");
                settings.Font = comboBoxFont.Text;
                settings.FontSize = (float)numericUpDownFontSize.Value;
                settings.ServerPort = textBoxServerPort.TextBox.GetAndValidateNumeric(1, 65535, "Server port must be between [min] and [max].");
                settings.AutoAwayIdleMinutes = textBoxAutoAwayIdleMinutes.TextBox.GetAndValidateNumeric(1, 1440, "Auto-away idle minutes must be between [min] and [max].");
                settings.MaxMessages = textBoxMaxMessages.TextBox.GetAndValidateNumeric(10, 10000, "Max messages must be between [min] and [max].");
                settings.RsaKeySize = textBoxRsaKeySize.TextBox.GetAndValidateNumeric(1024, 4096, "Max messages must be between [min] and [max].");
                settings.AesKeySize = textBoxAesKeySize.TextBox.GetAndValidateNumeric(128, 256, "Max messages must be between [min] and [max].");
                settings.EndToEndKeySize = textBoxEndToEndKeySize.TextBox.GetAndValidateNumeric(128, 10240, "Max messages must be between [min] and [max].");
                settings.FileTransferChunkSize = textBoxFileTransferChunkSize.TextBox.GetAndValidateNumeric(128, 1024 * 1024, "File transfer chunk size must be between [min] and [max].");

                settings.AlertToastWhenContactComesOnline = checkBoxAlertToastWhenContactComesOnline.Checked;
                settings.AlertToastWhenMessageReceived = checkBoxAlertToastWhenMessageReceived.Checked;
                settings.PlaySoundWhenContactComesOnline = checkBoxPlaySoundWhenContactComesOnline.Checked;
                settings.PlaySoundWhenMessageReceived = checkBoxPlaySoundWhenMessageReceived.Checked;
                settings.FlashWindowWhenMessageReceived = checkBoxFlashWindowWhenMessageReceived.Checked;
                settings.AlertToastWhenMyOnlineStatusChanges = checkBoxAlertToastWhenMyOnlineStatusChanges.Checked;
                settings.AlertToastErrorMessages = checkBoxAlertToastErrorMessages.Checked;

                if (ScConstants.AcceptableAesKeySizes.Contains(settings.AesKeySize) == false)
                {
                    throw new ArgumentOutOfRangeException("AES key size must be 128, 192, or 256.");
                }

                if (ScConstants.AcceptableRsaKeySizes.Contains(settings.RsaKeySize) == false)
                {
                    throw new ArgumentOutOfRangeException("RSA key size must be 1024, 2048, 3072, or 4096.");
                }

                try
                {
                    if (checkBoxAutoStartAtWindowsLogin.Checked)
                    {
                        RegistryHelper.EnableAutoStart();
                    }
                    else
                    {
                        RegistryHelper.DisableAutoStart();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to set auto-start registry entry. Error: {ex.GetBaseException().Message}", ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                Settings.Instance = settings;

                this.InvokeClose(DialogResult.OK);
            }
            catch (Exception ex)
            {
                Program.Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            try
            {
                //On cancel, we need to reset the theme to the original value.
                Program.ThemeManager.GlobalPaletteMode = Settings.Instance.Theme;
                foreach (Form form in Application.OpenForms)
                {
                    form.BackColor = KryptonManager.CurrentGlobalPalette.GetBackColor1(PaletteBackStyle.PanelClient, PaletteState.Normal);
                }
            }
            catch
            {
            }

            this.InvokeClose(DialogResult.Cancel);
        }

        private void kryptonComboBoxTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kryptonComboBoxTheme.SelectedItem is ThemeComboItem item)
            {
                Program.ThemeManager.GlobalPaletteMode = item.Mode;
                try
                {
                    foreach (Form form in Application.OpenForms)
                    {
                        form.BackColor = KryptonManager.CurrentGlobalPalette.GetBackColor1(PaletteBackStyle.PanelClient, PaletteState.Normal);
                    }
                }
                catch
                {
                }
            }
        }
    }
}
