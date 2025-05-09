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
            buttonDecline = new Button();
            buttonAccept = new Button();
            labelIncomingCallFrom = new Krypton.Toolkit.KryptonLabel();
            SuspendLayout();
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
            // labelIncomingCallFrom
            // 
            labelIncomingCallFrom.LabelStyle = Krypton.Toolkit.LabelStyle.BoldControl;
            labelIncomingCallFrom.Location = new Point(3, 3);
            labelIncomingCallFrom.Name = "labelIncomingCallFrom";
            labelIncomingCallFrom.Size = new Size(117, 20);
            labelIncomingCallFrom.TabIndex = 10;
            labelIncomingCallFrom.Values.Text = "IncomingCallFrom";
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
        private Button buttonDecline;
        private Button buttonAccept;
        private Krypton.Toolkit.KryptonLabel labelIncomingCallFrom;
    }
}
