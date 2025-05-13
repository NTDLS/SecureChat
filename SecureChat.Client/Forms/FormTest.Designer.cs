namespace SecureChat.Client.Forms
{
    partial class FormTest
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
            button1 = new Button();
            panelScroll = new Panel();
            flowLayoutPanelChat = new FlowLayoutPanel();
            panelScroll.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(510, 358);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 1;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // panelScroll
            // 
            panelScroll.AutoScroll = true;
            panelScroll.Controls.Add(flowLayoutPanelChat);
            panelScroll.Location = new Point(12, 12);
            panelScroll.Name = "panelScroll";
            panelScroll.Size = new Size(573, 337);
            panelScroll.TabIndex = 2;
            // 
            // flowLayoutPanelChat
            // 
            flowLayoutPanelChat.AutoSize = true;
            flowLayoutPanelChat.Dock = DockStyle.Top;
            flowLayoutPanelChat.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelChat.Location = new Point(0, 0);
            flowLayoutPanelChat.Name = "flowLayoutPanelChat";
            flowLayoutPanelChat.Size = new Size(573, 0);
            flowLayoutPanelChat.TabIndex = 0;
            flowLayoutPanelChat.WrapContents = false;
            // 
            // FormTest
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(609, 393);
            Controls.Add(panelScroll);
            Controls.Add(button1);
            Name = "FormTest";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormTest";
            panelScroll.ResumeLayout(false);
            panelScroll.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Button button1;
        private Panel panelScroll;
        private FlowLayoutPanel flowLayoutPanelChat;
    }
}