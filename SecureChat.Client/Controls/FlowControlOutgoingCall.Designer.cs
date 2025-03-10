namespace SecureChat.Client.Controls
{
    partial class FlowControlOutgoingCall
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
            labelOutgoingCallTo = new Label();
            buttonCancel = new Button();
            SuspendLayout();
            // 
            // labelIncomingCallFrom
            // 
            labelOutgoingCallTo.AutoSize = true;
            labelOutgoingCallTo.Location = new Point(3, 0);
            labelOutgoingCallTo.Name = "labelIncomingCallFrom";
            labelOutgoingCallTo.Size = new Size(132, 15);
            labelOutgoingCallTo.TabIndex = 5;
            labelOutgoingCallTo.Text = "Outgoing call to ............";
            // 
            // buttonCancel
            // 
            buttonCancel.ForeColor = Color.DarkRed;
            buttonCancel.Location = new Point(3, 34);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 4;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // FlowControlOutgoingCall
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(labelOutgoingCallTo);
            Controls.Add(buttonCancel);
            Name = "FlowControlOutgoingCall";
            Size = new Size(220, 60);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelOutgoingCallTo;
        private Button buttonCancel;
    }
}
