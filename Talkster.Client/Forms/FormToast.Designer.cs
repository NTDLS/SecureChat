namespace Talkster.Client.Forms
{
    partial class FormToast
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormToast));
            pictureBoxIcon = new PictureBox();
            labelHeader = new Label();
            labelBody = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBoxIcon).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxIcon
            // 
            pictureBoxIcon.Dock = DockStyle.Left;
            pictureBoxIcon.Location = new Point(0, 0);
            pictureBoxIcon.Name = "pictureBoxIcon";
            pictureBoxIcon.Size = new Size(43, 90);
            pictureBoxIcon.TabIndex = 0;
            pictureBoxIcon.TabStop = false;
            // 
            // labelHeader
            // 
            labelHeader.Dock = DockStyle.Top;
            labelHeader.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelHeader.Location = new Point(43, 0);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new Size(307, 20);
            labelHeader.TabIndex = 1;
            labelHeader.Text = "labelHeader";
            // 
            // labelBody
            // 
            labelBody.Dock = DockStyle.Fill;
            labelBody.Location = new Point(43, 20);
            labelBody.Name = "labelBody";
            labelBody.Size = new Size(307, 70);
            labelBody.TabIndex = 2;
            labelBody.Text = "labelBody";
            // 
            // FormToast
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(350, 90);
            ControlBox = false;
            Controls.Add(labelBody);
            Controls.Add(labelHeader);
            Controls.Add(pictureBoxIcon);
            Cursor = Cursors.Hand;
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(350, 90);
            Name = "FormToast";
            Opacity = 0.9D;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            ((System.ComponentModel.ISupportInitialize)pictureBoxIcon).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBoxIcon;
        private Label labelHeader;
        private Label labelBody;
    }
}