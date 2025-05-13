namespace SecureChat.Client.Controls
{
    partial class FlowControlFileTransferRequest
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
            buttonDecline = new Button();
            buttonAccept = new Button();
            labelHeader = new Krypton.Toolkit.KryptonLabel();
            labelFileName = new Krypton.Toolkit.KryptonLabel();
            SuspendLayout();
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
            // labelHeader
            // 
            labelHeader.LabelStyle = Krypton.Toolkit.LabelStyle.BoldControl;
            labelHeader.Location = new Point(3, 3);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new Size(53, 20);
            labelHeader.TabIndex = 9;
            labelHeader.Values.Text = "Header";
            // 
            // labelFileName
            // 
            labelFileName.Location = new Point(3, 21);
            labelFileName.Name = "labelFileName";
            labelFileName.Size = new Size(62, 20);
            labelFileName.TabIndex = 10;
            labelFileName.Values.Text = "FileName";
            // 
            // FlowControlFileTransferRequest
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(labelFileName);
            Controls.Add(labelHeader);
            Controls.Add(buttonDecline);
            Controls.Add(buttonAccept);
            Name = "FlowControlFileTransferRequest";
            Size = new Size(400, 70);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button buttonDecline;
        private Button buttonAccept;
        private Krypton.Toolkit.KryptonLabel labelHeader;
        private Krypton.Toolkit.KryptonLabel labelFileName;
    }
}
