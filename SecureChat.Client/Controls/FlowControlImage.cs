using SecureChat.Client.Forms;
using SecureChat.Client.Helpers;

namespace SecureChat.Client.Controls
{
    public class FlowControlImage : FlowLayoutPanel
    {
        private readonly FlowLayoutPanel _parent;
        private readonly PictureBox _pictureBox;

        public FlowControlImage(FlowLayoutPanel parent, byte[] imageBytes)
        {
            _parent = parent;
            AutoSize = true;
            Margin = new Padding(1);

            using var ms = new MemoryStream(imageBytes);
            var image = Image.FromStream(ms);

            _pictureBox = new PictureBox
            {
                Image = Image.FromStream(ms),
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = 200,
                Height = 200
            };

            _pictureBox.MouseEnter += Image_MouseEnter;
            _pictureBox.MouseLeave += Image_MouseLeave;
            _pictureBox.MouseClick += Image_MouseClick;

            Controls.Add(_pictureBox);
        }

        private void Image_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_pictureBox.Image != null)
                {
                    using var formImageViewer = new FormImageViewer(_pictureBox.Image);
                    formImageViewer.ShowDialog();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                var contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add("Save", null, OnSaveImage);
                contextMenu.Items.Add("Copy", null, OnCopyImage);
                contextMenu.Items.Add(new ToolStripSeparator());
                contextMenu.Items.Add("Remove", null, OnRemoveImage);
                contextMenu.Show(_pictureBox, e.Location);
            }
        }

        private void OnSaveImage(object? sender, EventArgs e)
        {
            if (_pictureBox.Image != null)
            {
                var imageBytes = Imaging.ImageToPngBytes(_pictureBox.Image);

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

        private void OnRemoveImage(object? sender, EventArgs e)
        {
            try
            {
                _pictureBox.Image = null;
                _parent.Controls.Remove(this);
            }
            catch
            {
            }
        }

        private void OnCopyImage(object? sender, EventArgs e)
        {
            try
            {
                if (_pictureBox.Image != null)
                {
                    Clipboard.SetImage(_pictureBox.Image);
                }
            }
            catch
            {
            }
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
