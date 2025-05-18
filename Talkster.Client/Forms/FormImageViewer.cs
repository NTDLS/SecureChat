using Krypton.Toolkit;
using System.Diagnostics;
using Talkster.Client.Helpers;
using Talkster.Library;

namespace Talkster.Client.Forms
{
    public partial class FormImageViewer : KryptonForm
    {
        private readonly Image _image;

        private float _zoomFactor = 1.0f;
        private const float ZoomStep = 0.1f;
        private const float ZoomMin = 0.1f;
        private const float ZoomMax = 10f;

        public FormImageViewer(Image image)
        {
            InitializeComponent();

            BackColor = KryptonManager.CurrentGlobalPalette.GetBackColor1(PaletteBackStyle.PanelClient, PaletteState.Normal);

            _image = image;

            Resize += (object? sender, EventArgs e) =>
            {
                DoResize();
            };

            pictureBoxImage.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxImage.Image = image;
            pictureBoxImage.MouseClick += Image_MouseClick;
            pictureBoxImage.BorderStyle = BorderStyle.None;

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

            DoResize();

            pictureBoxImage.MouseWheel += PictureBoxImage_MouseWheel;
            pictureBoxImage.Focus();
            pictureBoxImage.MouseEnter += (s, e) => pictureBoxImage.Focus();
        }

        private void PictureBoxImage_MouseWheel(object? sender, MouseEventArgs e)
        {
            var previousZoomFactor = _zoomFactor;

            if (e.Delta > 0)
            {
                _zoomFactor += ZoomStep;
            }
            else if (e.Delta < 0)
            {
                _zoomFactor -= ZoomStep;
            }

            //Prevent the size from getting too small.
            if ((pictureBoxImage.Size.Width < 32 || pictureBoxImage.Width < 32) && (_zoomFactor < previousZoomFactor))
            {
                _zoomFactor = previousZoomFactor;
            }

            DoResize();
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

        private void DoResize()
        {
            try
            {
                _zoomFactor = Math.Max(ZoomMin, Math.Min(ZoomMax, _zoomFactor));

                int newWidth = (int)(_image.Width * _zoomFactor);
                int newHeight = (int)(_image.Height * _zoomFactor);

                pictureBoxImage.Size = new Size(newWidth, newHeight);

                pictureBoxImage.Location = new Point(
                    (ClientSize.Width - pictureBoxImage.Width) / 2,
                    (ClientSize.Height - pictureBoxImage.Height) / 2);

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
            try
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
            catch
            {
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