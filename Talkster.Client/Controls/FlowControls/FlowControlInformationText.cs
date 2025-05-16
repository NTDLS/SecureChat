using Krypton.Toolkit;
using NTDLS.Helpers;
using Talkster.Client.Helpers;

namespace Talkster.Client.Controls.FlowControls
{
    public class FlowControlInformationText
        : FlowLayoutPanel, IFlowControl
    {
        private readonly FlowLayoutPanel _parent;
        private readonly Label _labelMessage;

        public FlowControlInformationText(FlowLayoutPanel parent, string message, Color requestedForeColor)
        {
            BackColor = KryptonManager.CurrentGlobalPalette.GetBackColor1(PaletteBackStyle.PanelClient, PaletteState.Normal);

            var foreColor = Theming.ShiftToContrast(requestedForeColor, BackColor);

            _parent = parent;
            FlowDirection = FlowDirection.TopDown;
            AutoSize = true;
            Margin = new Padding(0);

            _labelMessage = new Label
            {
                Text = message,
                AutoSize = true,
                ForeColor = foreColor,
                Font = Fonts.Instance.Italic,
                Padding = new Padding(0)
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
                contextMenu.Show(sender as Control ?? this, e.Location);
            }
        }

        private void OnRemove(object? sender, EventArgs e)
        {
            Remove();
        }

        private void OnCopy(object? sender, EventArgs e)
        {
            Exceptions.Ignore(() =>
            {
                Clipboard.SetText(_labelMessage.Text);
            });
        }

        public void Remove()
        {
            Exceptions.Ignore(() =>
            {
                _labelMessage.Text = string.Empty;
                _labelMessage.Dispose();
                _parent.Controls.Remove(this);
            });
        }
    }
}
