namespace SecureChat.Client.Controls
{
    public class FlowControlTextMessage : FlowLayoutPanel
    {
        public FlowControlTextMessage(string displayName, string message, Color? color)
        {
            FlowDirection = FlowDirection.LeftToRight;
            AutoSize = true;
            Margin = new Padding(0);
            Padding = new Padding(0);

            var lblDisplayName = new Label
            {
                Text = displayName,
                AutoSize = true,
                ForeColor = color ?? Color.Black,
                Font = ConstantFonts.Bold,
                //BackColor = Color.Gray,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            Controls.Add(lblDisplayName);

            var lblMessage = new Label
            {
                Text = message,
                AutoSize = true,
                ForeColor = Color.Black,
                Font = ConstantFonts.Regular,
                //BackColor = Color.LightGray,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            Controls.Add(lblMessage);
        }
    }
}
