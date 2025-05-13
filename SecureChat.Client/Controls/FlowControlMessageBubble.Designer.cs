namespace SecureChat.Client.Controls
{
    partial class FlowControlMessageBubble
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
            labelMessage = new Label();
            labelDisplayName = new Label();
            SuspendLayout();
            // 
            // labelMessage
            // 
            labelMessage.AutoSize = true;
            labelMessage.Dock = DockStyle.Top;
            labelMessage.ForeColor = Color.White;
            labelMessage.Location = new Point(0, 21);
            labelMessage.Name = "labelMessage";
            labelMessage.Size = new Size(38, 15);
            labelMessage.TabIndex = 0;
            labelMessage.Text = "label1";
            // 
            // labelDisplayName
            // 
            labelDisplayName.AutoSize = true;
            labelDisplayName.Dock = DockStyle.Top;
            labelDisplayName.ForeColor = Color.White;
            labelDisplayName.Location = new Point(0, 6);
            labelDisplayName.Name = "labelDisplayName";
            labelDisplayName.Size = new Size(102, 15);
            labelDisplayName.TabIndex = 1;
            labelDisplayName.Text = "labelDisplayName";
            // 
            // FlowControlMessageBubble
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(labelMessage);
            Controls.Add(labelDisplayName);
            Margin = new Padding(4, 3, 4, 3);
            MinimumSize = new Size(0, 47);
            Name = "FlowControlMessageBubble";
            Padding = new Padding(0, 6, 0, 6);
            Size = new Size(126, 47);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private Label labelMessage;
        private Label labelDisplayName;
    }
}
