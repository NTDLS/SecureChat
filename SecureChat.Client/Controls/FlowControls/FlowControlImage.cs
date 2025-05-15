using NTDLS.Helpers;
using SecureChat.Client.Forms;
using SecureChat.Client.Helpers;
using System.ComponentModel;
using static SecureChat.Library.ScConstants;

namespace SecureChat.Client.Controls.FlowControls
{
    public class FlowControlImage
        : FlowControlOriginBubble, IFlowControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Guid FileId { get; private set; }

        public FlowControlImage(FlowLayoutPanel parent, byte[] imageBytes, Guid fileId, ScOrigin origin, Image? initialStatusImage = null, string? displayName = null)
            : base(parent, new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                MaximumSize = new Size(100, 100),
            }, origin, initialStatusImage, displayName)
        {
            FileId = fileId;

            using var ms = new MemoryStream(imageBytes);
            var image = Image.FromStream(ms);

            if (ChildControl is PictureBox child)
            {
                child.Image = Image.FromStream(ms);
                child.SizeMode = PictureBoxSizeMode.Zoom;
                child.MouseEnter += Image_MouseEnter;
                child.MouseLeave += Image_MouseLeave;
                child.MouseClick += Image_MouseClick;
            }
        }

        private void Image_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (ChildControl is PictureBox child)
                {
                    if (child.Image != null)
                    {
                        using var formImageViewer = new FormImageViewer(child.Image);
                        formImageViewer.ShowDialog();
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                var contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add("Save", null, OnSaveImage);
                contextMenu.Items.Add("Copy", null, OnCopyImage);
                contextMenu.Items.Add(new ToolStripSeparator());
                contextMenu.Items.Add("Remove", null, OnRemove);
                contextMenu.Show(sender as Control ?? this, e.Location);
            }
        }

        private void OnSaveImage(object? sender, EventArgs e)
        {
            if (ChildControl is PictureBox child && child.Image != null)
            {
                var imageBytes = Imaging.ImageToPngBytes(child.Image);

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
            Exceptions.Ignore(() =>
            {
                if (ChildControl is PictureBox child && child.Image != null)
                {
                    Clipboard.SetImage(child.Image);
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
