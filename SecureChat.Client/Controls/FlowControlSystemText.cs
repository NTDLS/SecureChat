using SecureChat.Client.Helpers;

namespace SecureChat.Client.Controls
{
    public class FlowControlSystemText : FlowLayoutPanel
    {
        public FlowControlSystemText(string message, Color? color = null)
        {
            FlowDirection = FlowDirection.TopDown;
            AutoSize = true;
            Margin = new Padding(0);


            var lblMessage = new Label
            {
                Text = message,
                AutoSize = true,
                ForeColor = color ?? Color.Gray,
                Font = Fonts.Instance.Italic,
                Padding = new Padding(0)
            };
            Controls.Add(lblMessage);
        }
    }
}
