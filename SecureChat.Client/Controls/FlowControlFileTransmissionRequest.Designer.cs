namespace SecureChat.Client.Controls
{
    partial class FlowControlFileTransmissionRequest
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            labelHeader = new Label();
            buttonDecline = new Button();
            buttonAccept = new Button();
            labelFileName = new Label();
            SuspendLayout();
            // 
            // labelHeader
            // 
            labelHeader.AutoSize = true;
            labelHeader.Font = new Font("Segoe UI", 12F);
            labelHeader.Location = new Point(3, 0);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new Size(237, 21);
            labelHeader.TabIndex = 5;
            labelHeader.Text = "%% is sending you a 0.00MB file.";
            // 
            // buttonDecline
            // 
            buttonDecline.ForeColor = Color.DarkRed;
            buttonDecline.Location = new Point(84, 44);
            buttonDecline.Name = "buttonDecline";
            buttonDecline.Size = new Size(75, 23);
            buttonDecline.TabIndex = 4;
            buttonDecline.Text = "Decline";
            buttonDecline.UseVisualStyleBackColor = true;
            buttonDecline.Click += ButtonDecline_Click;
            // 
            // buttonAccept
            // 
            buttonAccept.ForeColor = Color.ForestGreen;
            buttonAccept.Location = new Point(3, 44);
            buttonAccept.Name = "buttonAccept";
            buttonAccept.Size = new Size(75, 23);
            buttonAccept.TabIndex = 3;
            buttonAccept.Text = "Accept";
            buttonAccept.UseVisualStyleBackColor = true;
            buttonAccept.Click += ButtonAccept_Click;
            // 
            // labelFileName
            // 
            labelFileName.AutoEllipsis = true;
            labelFileName.AutoSize = true;
            labelFileName.Location = new Point(3, 22);
            labelFileName.Name = "labelFileName";
            labelFileName.Size = new Size(38, 15);
            labelFileName.TabIndex = 6;
            labelFileName.Text = "label1";
            // 
            // FlowControlFileTransmissionRequest
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(labelFileName);
            Controls.Add(labelHeader);
            Controls.Add(buttonDecline);
            Controls.Add(buttonAccept);
            Name = "FlowControlFileTransmissionRequest";
            Size = new Size(400, 70);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelHeader;
        private Button buttonDecline;
        private Button buttonAccept;
        private Label labelFileName;
    }
}
