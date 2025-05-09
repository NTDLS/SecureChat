using Krypton.Toolkit;
using NTDLS.Helpers;
using SecureChat.Client.Helpers;

namespace SecureChat.Client.Controls
{
    public class FlowControlTextMessage : FlowLayoutPanel
    {
        private readonly FlowLayoutPanel _parent;
        private readonly Label _labelMessage;

        public FlowControlTextMessage(FlowLayoutPanel parent, string displayName, string message, Color? displayNameColor)
        {
            BackColor = KryptonManager.CurrentGlobalPalette.GetBackColor1(PaletteBackStyle.PanelClient, PaletteState.Normal);

            _parent = parent;
            FlowDirection = FlowDirection.LeftToRight;
            AutoSize = true;
            Margin = new Padding(0);
            Padding = new Padding(0);

            displayNameColor ??= Themes.ChooseColor(Color.White, Color.Black);

            var labelDisplayName = new Label
            {
                Text = displayName,
                AutoSize = true,
                ForeColor = displayNameColor.EnsureNotNull(),
                Font = Fonts.Instance.Bold,
                //BackColor = Color.Gray,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            labelDisplayName.MouseClick += LabelMessage_MouseClick;
            Controls.Add(labelDisplayName);

            _labelMessage = new Label
            {
                Text = message,
                AutoSize = true,
                ForeColor = Themes.ChooseColor(Color.White, Color.Black),
                Font = Fonts.Instance.Regular,
                //BackColor = Color.LightGray,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            _labelMessage.MouseClick += LabelMessage_MouseClick;
            Controls.Add(_labelMessage);
        }

        private void LabelMessage_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add("Copy", null, OnCopy);
                contextMenu.Items.Add(new ToolStripSeparator());
                contextMenu.Items.Add("Remove", null, OnRemove);
                contextMenu.Show((sender as Control) ?? this, e.Location);
            }
        }

        private void OnRemove(object? sender, EventArgs e)
        {
            Exceptions.Ignore(() =>
            {

                _labelMessage.Text = string.Empty;
                _parent.Controls.Remove(this);
            });
        }

        private void OnCopy(object? sender, EventArgs e)
        {
            Exceptions.Ignore(() =>
            {
                Clipboard.SetText(_labelMessage.Text);
            });
        }
    }
}
