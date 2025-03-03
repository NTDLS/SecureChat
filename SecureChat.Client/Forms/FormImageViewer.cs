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

            Width = image.Width < 800 ? image.Width + 20 : 800;
            Height = image.Height < 600 ? image.Height + 20 : 600;

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
    }
}