namespace SecureChat.Client.Controls
{
    partial class FlowControlFileTransmissionSendProgress
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
            labelHeaderText = new Label();
            buttonCancel = new Button();
            progressBarCompletion = new ProgressBar();
            labelWaitingStatus = new Label();
            SuspendLayout();
            // 
            // labelHeaderText
            // 
            labelHeaderText.AutoEllipsis = true;
            labelHeaderText.AutoSize = true;
            labelHeaderText.Font = new Font("Segoe UI", 12F);
            labelHeaderText.Location = new Point(3, 0);
            labelHeaderText.Name = "labelHeaderText";
            labelHeaderText.Size = new Size(106, 21);
            labelHeaderText.TabIndex = 5;
            labelHeaderText.Text = "Transmitting...";
            // 
            // buttonCancel
            // 
            buttonCancel.ForeColor = Color.DarkRed;
            buttonCancel.Location = new Point(3, 39);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 4;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += ButtonDecline_Click;
            // 
            // progressBarCompletion
            // 
            progressBarCompletion.Location = new Point(9, 25);
            progressBarCompletion.Name = "progressBarCompletion";
            progressBarCompletion.Size = new Size(223, 10);
            progressBarCompletion.TabIndex = 6;
            // 
            // labelStatus
            // 
            labelWaitingStatus.AutoSize = true;
            labelWaitingStatus.Location = new Point(9, 21);
            labelWaitingStatus.Name = "labelStatus";
            labelWaitingStatus.Size = new Size(131, 15);
            labelWaitingStatus.TabIndex = 7;
            labelWaitingStatus.Text = "Waiting on acceptance.";
            // 
            // FlowControlFileTransmissionProgress
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(labelWaitingStatus);
            Controls.Add(progressBarCompletion);
            Controls.Add(labelHeaderText);
            Controls.Add(buttonCancel);
            Name = "FlowControlFileTransmissionProgress";
            Size = new Size(400, 65);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelHeaderText;
        private Button buttonCancel;
        private ProgressBar progressBarCompletion;
        private Label labelWaitingStatus;
    }
}
