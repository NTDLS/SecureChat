using Krypton.Toolkit;
using NTDLS.WinFormsHelpers.Controls;

namespace Talkster.Client.Forms
{
    /// <summary>
    /// Progress form used for multi-threaded progress reporting.
    /// </summary>
    internal partial class FormThemedProgress
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormThemedProgress));
            buttonCancel = new KryptonButton();
            pbProgress = new KryptonProgressBar();
            labelHeader = new KryptonLabel();
            labelBody = new KryptonLabel();
            spinningActivity = new ActivityIndicator();
            SuspendLayout();
            // 
            // buttonCancel
            // 
            buttonCancel.Enabled = false;
            buttonCancel.Location = new Point(282, 120);
            buttonCancel.Margin = new Padding(4, 3, 4, 3);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(88, 27);
            buttonCancel.TabIndex = 1;
            buttonCancel.Values.DropDownArrowColor = Color.Empty;
            buttonCancel.Values.Text = "Cancel";
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // pbProgress
            // 
            pbProgress.Location = new Point(59, 87);
            pbProgress.Margin = new Padding(4, 3, 4, 3);
            pbProgress.Name = "pbProgress";
            pbProgress.Size = new Size(311, 27);
            pbProgress.Style = ProgressBarStyle.Marquee;
            pbProgress.TabIndex = 2;
            pbProgress.Values.Text = "";
            // 
            // labelHeader
            // 
            labelHeader.Location = new Point(56, 14);
            labelHeader.Margin = new Padding(4, 0, 4, 0);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new Size(79, 20);
            labelHeader.TabIndex = 3;
            labelHeader.Values.Text = "Please wait...";
            // 
            // labelBody
            // 
            labelBody.Location = new Point(56, 52);
            labelBody.Margin = new Padding(4, 0, 4, 0);
            labelBody.Name = "labelBody";
            labelBody.Size = new Size(79, 20);
            labelBody.TabIndex = 4;
            labelBody.Values.Text = "Please wait...";
            // 
            // spinningActivity
            // 
            spinningActivity.Location = new Point(13, 20);
            spinningActivity.Margin = new Padding(4, 0, 4, 0);
            spinningActivity.Name = "spinningActivity";
            spinningActivity.Size = new Size(35, 32);
            spinningActivity.TabIndex = 4;
            // 
            // FormThemedProgress
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(383, 159);
            Controls.Add(labelBody);
            Controls.Add(spinningActivity);
            Controls.Add(labelHeader);
            Controls.Add(pbProgress);
            Controls.Add(buttonCancel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MaximumSize = new Size(397, 204);
            MinimizeBox = false;
            MinimumSize = new Size(397, 204);
            Name = "FormThemedProgress";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Talkster";
            Shown += FormProgress_Shown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private KryptonButton buttonCancel;
        private KryptonProgressBar pbProgress;
        private KryptonLabel labelHeader;
        private KryptonLabel labelBody;
        private ActivityIndicator spinningActivity;
    }
}