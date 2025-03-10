using SecureChat.Client.Helpers;

namespace SecureChat.Client.Controls
{
    public class FlowControlSystemText : FlowLayoutPanel
    {
        private readonly FlowLayoutPanel _parent;

        public FlowControlSystemText(FlowLayoutPanel parent, string message, Color? color = null)
        {
            _parent = parent;
            FlowDirection = FlowDirection.TopDown;
            AutoSize = true;
            Margin = new Padding(0);

            var labelMessage = new Label
            {
                Text = message,
                AutoSize = true,
                ForeColor = color ?? Color.Gray,
                Font = Fonts.Instance.Italic,
                Padding = new Padding(0)
            };
            Controls.Add(labelMessage);
        }
    }
}
