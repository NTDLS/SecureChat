using SecureChat.Client.Forms;

namespace SecureChat.Client.Controls
{
    public class FlowControlImage : FlowLayoutPanel
    {
        private readonly Image _image;

        public FlowControlImage(byte[] imageBytes)
        {
            AutoSize = true;
            Margin = new Padding(1);

            using var ms = new MemoryStream(imageBytes);
            _image = Image.FromStream(ms);

            var picture = new PictureBox
            {
                Image = Image.FromStream(ms),
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = 200,
                Height = 200
            };

            picture.MouseEnter += Image_MouseEnter;
            picture.MouseLeave += Image_MouseLeave;
            picture.MouseClick += Image_MouseClick;

            Controls.Add(picture);
        }

        private void Image_MouseClick(object? sender, MouseEventArgs e)
        {
            using var formImageViewer = new FormImageViewer(_image);
            formImageViewer.ShowDialog();
        }

        private void Image_MouseLeave(object? sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void Image_MouseEnter(object? sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }
    }
}
