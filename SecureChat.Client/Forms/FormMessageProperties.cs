using Krypton.Toolkit;
using SecureChat.Library;

namespace SecureChat.Client.Forms
{
    public partial class FormMessageProperties : KryptonForm
    {
        private ActiveChat _activeChat;

        internal FormMessageProperties(ActiveChat activeChat)
        {
            InitializeComponent();
            _activeChat = activeChat;

            BackColor = KryptonManager.CurrentGlobalPalette.GetBackColor1(PaletteBackStyle.PanelClient, PaletteState.Normal);

            textBoxAccountId.Text = activeChat.AccountId.ToString();
            textBoxDisplayName.Text = activeChat.DisplayName;
            textBoxSessionId.Text = activeChat.SessionId.ToString();
            textBoxPublicRsaKey.Text = Crypto.ComputeSha256Hash(_activeChat.PublicPrivateKeyPair.PublicRsaKey);
            labelPublicRsaKeyLength.Text = $"{(_activeChat.PublicPrivateKeyPair.PublicRsaKey.Length * 8):n0}bits";
            textBoxPrivateRsaKey.Text = Crypto.ComputeSha256Hash(_activeChat.PublicPrivateKeyPair.PrivateRsaKey);
            labelPrivateRsaKeyLength.Text = $"{(_activeChat.PublicPrivateKeyPair.PrivateRsaKey.Length * 8):n0}bits";
            textBoxSharedSecret.Text = Crypto.ComputeSha256Hash(_activeChat.SharedSecret);
            labelSharedSecretLength.Text = $"{(_activeChat.SharedSecret.Length * 8):n0}bits";
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}