using SecureChat.Client.Helpers;

namespace SecureChat.Client.Forms
{
    public partial class FormImageViewer : Form
    {
        private readonly Image _image;

        public FormImageViewer(Image image)
        {
            InitializeComponent();

            _image = image;

            Resize += OnResize;

            pictureBoxImage.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxImage.Dock = DockStyle.Fill;
            pictureBoxImage.Image = image;

            if (image.Width > 800 || image.Height > 600)
            {
                Width = 800;
                Height = 600;
            }
            else if (image.Width < 300 || image.Height < 200)
            {
                Width = 250;
                Height = 200;
            }
            else
            {
                Width = image.Width < 800 ? image.Width + 20 : 800;
                Height = image.Height < 600 ? image.Height + 20 : 600;
            }

            AdjustImageSize();
        }

        private void OnResize(object? sender, EventArgs e)
        {
            AdjustImageSize();
        }

        private void AdjustImageSize()
        {
            if (_image == null) return;

            if (ClientSize.Width > _image.Width && ClientSize.Height > _image.Height)
            {
                pictureBoxImage.SizeMode = PictureBoxSizeMode.CenterImage;
            }
            else
            {
                pictureBoxImage.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBoxImage.Image != null)
            {
                var imageBytes = Imaging.ImageToPngBytes(pictureBoxImage.Image);

                using var sfd = new SaveFileDialog();
                sfd.Filter = "PNG Image|*.png";
                sfd.Title = "Save Image As";
                sfd.FileName = "image.png";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(sfd.FileName, imageBytes);
                }
            }
        }
    }
}