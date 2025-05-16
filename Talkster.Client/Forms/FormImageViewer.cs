using Krypton.Toolkit;
using System.Diagnostics;
using Talkster.Client.Helpers;
using Talkster.Library;

namespace Talkster.Client.Forms
{
    public partial class FormImageViewer : KryptonForm
    {
        private readonly Image _image;

        public FormImageViewer(Image image)
        {
            InitializeComponent();

            BackColor = KryptonManager.CurrentGlobalPalette.GetBackColor1(PaletteBackStyle.PanelClient, PaletteState.Normal);

            _image = image;

            Resize += OnResize;

            pictureBoxImage.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxImage.Dock = DockStyle.Fill;
            pictureBoxImage.Image = image;
            pictureBoxImage.MouseClick += Image_MouseClick;

            if (image.Width > 800 || image.Height > 600)
            {
                Width = 800;
                Height = 600;
            }
            else if (image.Width < 300 || image.Height < 200)
            {
                Width = 300;
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

        private void Image_MouseClick(object? sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    var contextMenu = new ContextMenuStrip();
                    contextMenu.Items.Add("Save", null, OnSaveImage);
                    contextMenu.Items.Add("Copy", null, OnCopyImage);
                    contextMenu.Show(pictureBoxImage, e.Location);
                }
            }
            catch (Exception ex)
            {
                Program.Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AdjustImageSize()
        {
            try
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
            catch (Exception ex)
            {
                Program.Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnSaveImage(sender, e);
        }

        private void OnSaveImage(object? sender, EventArgs e)
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

        private void OnCopyImage(object? sender, EventArgs e)
        {
            try
            {
                if (pictureBoxImage.Image != null)
                {
                    Clipboard.SetImage(pictureBoxImage.Image);
                }
            }
            catch
            {
            }
        }
    }
}