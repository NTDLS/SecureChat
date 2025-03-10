namespace SecureChat.Client.Controls
{
    partial class FlowControlIncomingCall
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
            labelIncomingCallFrom = new Label();
            buttonDecline = new Button();
            buttonAccept = new Button();
            SuspendLayout();
            // 
            // labelIncomingCallFrom
            // 
            labelIncomingCallFrom.AutoSize = true;
            labelIncomingCallFrom.Font = new Font("Segoe UI", 12F);
            labelIncomingCallFrom.Location = new Point(3, 0);
            labelIncomingCallFrom.Name = "labelIncomingCallFrom";
            labelIncomingCallFrom.Size = new Size(180, 21);
            labelIncomingCallFrom.TabIndex = 5;
            labelIncomingCallFrom.Text = "Incoming call from ............";
            // 
            // buttonDecline
            // 
            buttonDecline.ForeColor = Color.DarkRed;
            buttonDecline.Location = new Point(84, 24);
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
            buttonAccept.Location = new Point(3, 24);
            buttonAccept.Name = "buttonAccept";
            buttonAccept.Size = new Size(75, 23);
            buttonAccept.TabIndex = 3;
            buttonAccept.Text = "Accept";
            buttonAccept.UseVisualStyleBackColor = true;
            buttonAccept.Click += ButtonAccept_Click;
            // 
            // FlowControlIncomingCall
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(labelIncomingCallFrom);
            Controls.Add(buttonDecline);
            Controls.Add(buttonAccept);
            Name = "FlowControlIncomingCall";
            Size = new Size(400, 50);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelIncomingCallFrom;
        private Button buttonDecline;
        private Button buttonAccept;
    }
}
