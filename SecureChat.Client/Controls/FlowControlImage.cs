namespace SecureChat.Client.Controls
{
    public class FlowControlImage : FlowLayoutPanel
    {
        public FlowControlImage(byte[] imageBytes)
        {
            AutoSize = true;
            Margin = new Padding(1);

            using var ms = new MemoryStream(imageBytes);
            var picture = new PictureBox
            {
                Image = Image.FromStream(ms), // Convert bytes to Image
                SizeMode = PictureBoxSizeMode.AutoSize
                //Width = 100,
                //Height = 100
            };
            Controls.Add(picture);
        }
    }
}
