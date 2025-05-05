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
            // labelOutgoingCallTo
            // 
            labelOutgoingCallTo.AutoSize = true;
            labelOutgoingCallTo.Font = new Font("Segoe UI", 12F);
            labelOutgoingCallTo.Location = new Point(3, 0);
            labelOutgoingCallTo.Name = "labelOutgoingCallTo";
            labelOutgoingCallTo.Size = new Size(161, 21);
            labelOutgoingCallTo.TabIndex = 5;
            labelOutgoingCallTo.Text = "Outgoing call to ............";
            // 
            // buttonCancel
            // 
            buttonCancel.ForeColor = Color.DarkRed;
            buttonCancel.Location = new Point(3, 24);
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
            Size = new Size(400, 50);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelOutgoingCallTo;
        private Button buttonCancel;
    }
}
