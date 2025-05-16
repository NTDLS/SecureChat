using Krypton.Toolkit;

namespace Talkster.Client.Controls
{
    partial class FlowControlFileTransferReceiveProgress
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
            buttonCancel = new Button();
            progressBarCompletion = new KryptonProgressBar();
            labelHeaderText = new Krypton.Toolkit.KryptonLabel();
            labelWaitingStatus = new Krypton.Toolkit.KryptonLabel();
            SuspendLayout();
            // 
            // buttonCancel
            // 
            buttonCancel.ForeColor = Color.DarkRed;
            buttonCancel.Location = new Point(3, 40);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 4;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // progressBarCompletion
            // 
            progressBarCompletion.Location = new Point(9, 25);
            progressBarCompletion.Name = "progressBarCompletion";
            progressBarCompletion.Size = new Size(223, 10);
            progressBarCompletion.TabIndex = 6;
            // 
            // labelHeaderText
            // 
            labelHeaderText.LabelStyle = Krypton.Toolkit.LabelStyle.BoldControl;
            labelHeaderText.Location = new Point(3, 3);
            labelHeaderText.Name = "labelHeaderText";
            labelHeaderText.Size = new Size(78, 20);
            labelHeaderText.TabIndex = 8;
            labelHeaderText.Values.Text = "HeaderText";
            // 
            // labelWaitingStatus
            // 
            labelWaitingStatus.Location = new Point(3, 20);
            labelWaitingStatus.Name = "labelWaitingStatus";
            labelWaitingStatus.Size = new Size(86, 20);
            labelWaitingStatus.TabIndex = 9;
            labelWaitingStatus.Values.Text = "WaitingStatus";
            // 
            // FlowControlFileTransferReceiveProgress
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(labelWaitingStatus);
            Controls.Add(labelHeaderText);
            Controls.Add(progressBarCompletion);
            Controls.Add(buttonCancel);
            Name = "FlowControlFileTransferReceiveProgress";
            Size = new Size(400, 65);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button buttonCancel;
        private KryptonProgressBar progressBarCompletion;
        private Krypton.Toolkit.KryptonLabel labelHeaderText;
        private Krypton.Toolkit.KryptonLabel labelWaitingStatus;
    }
}
