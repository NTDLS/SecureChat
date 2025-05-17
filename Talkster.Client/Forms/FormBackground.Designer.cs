namespace Talkster.Client.Forms
{
    partial class FormBackground
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBackground));
            pictureBoxBackground = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBackground).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxBackground
            // 
            pictureBoxBackground.BackgroundImageLayout = ImageLayout.Center;
            pictureBoxBackground.Dock = DockStyle.Fill;
            pictureBoxBackground.Image = (Image)resources.GetObject("pictureBoxBackground.Image");
            pictureBoxBackground.Location = new Point(0, 0);
            pictureBoxBackground.Name = "pictureBoxBackground";
            pictureBoxBackground.Size = new Size(656, 658);
            pictureBoxBackground.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBoxBackground.TabIndex = 0;
            pictureBoxBackground.TabStop = false;
            // 
            // FormBackground
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImageLayout = ImageLayout.Center;
            CausesValidation = false;
            ClientSize = new Size(656, 658);
            ControlBox = false;
            Controls.Add(pictureBoxBackground);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimizeBox = false;
            Name = "FormBackground";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            ((System.ComponentModel.ISupportInitialize)pictureBoxBackground).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBoxBackground;
    }
}