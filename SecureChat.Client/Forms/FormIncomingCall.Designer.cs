namespace SecureChat.Client.Forms
{
    partial class FormIncomingCall
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormIncomingCall));
            buttonAccept = new Button();
            buttonDecline = new Button();
            labelIncomingCallFrom = new Label();
            SuspendLayout();
            // 
            // buttonAccept
            // 
            buttonAccept.Location = new Point(12, 60);
            buttonAccept.Name = "buttonAccept";
            buttonAccept.Size = new Size(75, 23);
            buttonAccept.TabIndex = 0;
            buttonAccept.Text = "Accept";
            buttonAccept.UseVisualStyleBackColor = true;
            buttonAccept.Click += ButtonAccept_Click;
            // 
            // buttonDecline
            // 
            buttonDecline.Location = new Point(186, 60);
            buttonDecline.Name = "buttonDecline";
            buttonDecline.Size = new Size(75, 23);
            buttonDecline.TabIndex = 1;
            buttonDecline.Text = "Decline";
            buttonDecline.UseVisualStyleBackColor = true;
            buttonDecline.Click += ButtonDecline_Click;
            // 
            // labelIncomingCallFrom
            // 
            labelIncomingCallFrom.AutoSize = true;
            labelIncomingCallFrom.Location = new Point(17, 14);
            labelIncomingCallFrom.Name = "labelIncomingCallFrom";
            labelIncomingCallFrom.Size = new Size(147, 15);
            labelIncomingCallFrom.TabIndex = 2;
            labelIncomingCallFrom.Text = "Incoming call from ............";
            // 
            // FormIncomingCall
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(271, 93);
            Controls.Add(labelIncomingCallFrom);
            Controls.Add(buttonDecline);
            Controls.Add(buttonAccept);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormIncomingCall";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Secure Chat";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonAccept;
        private Button buttonDecline;
        private Label labelIncomingCallFrom;
    }
}