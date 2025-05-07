using NTDLS.Helpers;
using SecureChat.Client.Forms;
using SecureChat.Client.Helpers;

namespace SecureChat.Client.Controls
{
    public class FlowControlImage : FlowLayoutPanel
    {
        private readonly FlowLayoutPanel _parent;
        private readonly PictureBox _pictureBox;

        public FlowControlImage(FlowLayoutPanel parent, string displayName, byte[] imageBytes, Color? color)
        {
            _parent = parent;
            FlowDirection = FlowDirection.TopDown;
            AutoSize = true;
            Margin = new Padding(0);
            Padding = new Padding(0);

            var labelDisplayName = new Label
            {
                Text = displayName,
                AutoSize = true,
                ForeColor = color ?? Color.Black,
                Font = Fonts.Instance.Bold,
                //BackColor = Color.Gray,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            labelDisplayName.MouseClick += Image_MouseClick;
            Controls.Add(labelDisplayName);

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
                contextMenu.Show((sender as Control) ?? this, e.Location);
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
            Exceptions.Ignore(() =>
            {

                _pictureBox.Image = null;
                _parent.Controls.Remove(this);
            });
        }

        private void OnCopyImage(object? sender, EventArgs e)
        {
            Exceptions.Ignore(() =>
            {

                if (_pictureBox.Image != null)
                {
                    Clipboard.SetImage(_pictureBox.Image);
                }
            });
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
