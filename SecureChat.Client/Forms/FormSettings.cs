using Krypton.Toolkit;
using NTDLS.Helpers;
using NTDLS.Persistence;
using NTDLS.WinFormsHelpers;
using SecureChat.Client.Controls;
using SecureChat.Client.Helpers;
using SecureChat.Library;
using Serilog;
using System.Diagnostics;

namespace SecureChat.Client.Forms
{
    public partial class FormSettings : KryptonForm
    {
        public FormSettings(bool showInTaskbar)
        {
            InitializeComponent();

            BackColor = KryptonManager.CurrentGlobalPalette.GetBackColor1(PaletteBackStyle.PanelClient, PaletteState.Normal);

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
            textBoxAutoAwayIdleSeconds.Text = $"{Settings.Instance.AutoAwayIdleSeconds:n0}";
            textBoxMaxMessages.Text = $"{Settings.Instance.MaxMessages:n0}";
            textBoxFileTransferChunkSize.Text = $"{Settings.Instance.FileTransferChunkSize:n0}";
            checkBoxAlertToastWhenContactComesOnline.Checked = Settings.Instance.AlertToastWhenContactComesOnline;
            checkBoxAlertToastWhenMessageReceived.Checked = Settings.Instance.AlertToastWhenMessageReceived;
            checkBoxPlaySoundWhenContactComesOnline.Checked = Settings.Instance.PlaySoundWhenContactComesOnline;
            checkBoxPlaySoundWhenMessageReceived.Checked = Settings.Instance.PlaySoundWhenMessageReceived;
            checkBoxFlashWindowWhenMessageReceived.Checked = Settings.Instance.FlashWindowWhenMessageReceived;

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

            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("365 Black Dark Mode Alternate", PaletteMode.Microsoft365BlackDarkModeAlternate));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("365 Black Dark Mode", PaletteMode.Microsoft365BlackDarkMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("365 Black", PaletteMode.Microsoft365Black));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("365 Blue Dark Mode", PaletteMode.Microsoft365BlueDarkMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("365 Blue Light Mode", PaletteMode.Microsoft365BlueLightMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("365 Blue", PaletteMode.Microsoft365Blue));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("365 Silver Dark Mode", PaletteMode.Microsoft365SilverDarkMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("365 Silver Light Mode", PaletteMode.Microsoft365SilverLightMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("365 Silver", PaletteMode.Microsoft365Silver));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("365 White", PaletteMode.Microsoft365White));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office 2003", PaletteMode.ProfessionalOffice2003));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office 2007 Black Dark Mode", PaletteMode.Office2007BlackDarkMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office 2007 Black", PaletteMode.Office2007Black));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office 2007 Blue Dark Mode", PaletteMode.Office2007BlueDarkMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office 2007 Blue Light Mode", PaletteMode.Office2007BlueLightMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office 2007 Blue", PaletteMode.Office2007Blue));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office 2007 Silver Dark Mode", PaletteMode.Office2007SilverDarkMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office 2007 Silver Light Mode", PaletteMode.Office2007SilverLightMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office 2007 Silver", PaletteMode.Office2007Silver));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office 2007 White", PaletteMode.Office2007White));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office 2010 Black Dark Mode", PaletteMode.Office2010BlackDarkMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office 2010 Black", PaletteMode.Office2010Black));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office 2010 Blue Dark Mode", PaletteMode.Office2010BlueDarkMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office 2010 Blue Light Mode", PaletteMode.Office2010BlueLightMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office 2010 Blue", PaletteMode.Office2010Blue));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office 2010 Silver Dark Mode", PaletteMode.Office2010SilverDarkMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office 2010 Silver Light Mode", PaletteMode.Office2010SilverLightMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office 2010 Silver", PaletteMode.Office2010Silver));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office 2010 White", PaletteMode.Office2010White));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Office 2013 White", PaletteMode.Office2013White));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Professional", PaletteMode.ProfessionalSystem));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Sparkle Blue Dark Mode", PaletteMode.SparkleBlueDarkMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Sparkle Blue Light Mode", PaletteMode.SparkleBlueLightMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Sparkle Blue", PaletteMode.SparkleBlue));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Sparkle Orange Dark Mode", PaletteMode.SparkleOrangeDarkMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Sparkle Orange Light Mode", PaletteMode.SparkleOrangeLightMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Sparkle Orange", PaletteMode.SparkleOrange));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Sparkle Purple Dark Mode", PaletteMode.SparklePurpleDarkMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Sparkle Purple Light Mode", PaletteMode.SparklePurpleLightMode));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Sparkle Purple", PaletteMode.SparklePurple));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Studio Render 2007", PaletteMode.VisualStudio2010Render2007));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Studio Render 2010", PaletteMode.VisualStudio2010Render2010));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Studio Render 2013", PaletteMode.VisualStudio2010Render2013));
            kryptonComboBoxTheme.Items.Add(new ThemeComboItem("Studio Render 365", PaletteMode.VisualStudio2010Render365));

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
                settings.ServerAddress = textBoxServerAddress.TextBox.GetAndValidateText("Server address must not be empty.");
                settings.Font = comboBoxFont.Text;
                settings.FontSize = (float)numericUpDownFontSize.Value;
                settings.ServerPort = textBoxServerPort.TextBox.GetAndValidateNumeric(1, 65535, "Server port must be between [min] and [max].");
                settings.AutoAwayIdleSeconds = textBoxAutoAwayIdleSeconds.TextBox.GetAndValidateNumeric(60, 86400, "Auto-away idle seconds must be between [min] and [max].");
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
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.InvokeClose(DialogResult.Cancel);
        }


        private void kryptonComboBoxTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kryptonComboBoxTheme.SelectedItem is ThemeComboItem item)
            {
                Program.ThemeManager.GlobalPaletteMode = item.Mode;
            }
        }
    }
}
