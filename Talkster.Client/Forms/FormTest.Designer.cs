namespace Talkster.Client.Forms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTest));
            splitContainer1 = new Krypton.Toolkit.KryptonSplitContainer();
            flowLayoutPanelChat = new FlowLayoutPanel();
            kryptonTextBoxMessage = new Krypton.Toolkit.KryptonTextBox();
            kryptonButtonSend = new Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            (splitContainer1.Panel1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            (splitContainer1.Panel2).BeginInit();
            splitContainer1.Panel2.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel2;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // 
            // 
            splitContainer1.Panel1.Controls.Add(flowLayoutPanelChat);
            // 
            // 
            // 
            splitContainer1.Panel2.Controls.Add(kryptonTextBoxMessage);
            splitContainer1.Panel2.Controls.Add(kryptonButtonSend);
            splitContainer1.Size = new Size(584, 404);
            splitContainer1.SplitterDistance = 353;
            splitContainer1.TabIndex = 2;
            // 
            // flowLayoutPanelChat
            // 
            flowLayoutPanelChat.AutoScroll = true;
            flowLayoutPanelChat.BackColor = Color.Transparent;
            flowLayoutPanelChat.Dock = DockStyle.Fill;
            flowLayoutPanelChat.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelChat.Location = new Point(0, 0);
            flowLayoutPanelChat.Name = "flowLayoutPanelChat";
            flowLayoutPanelChat.Size = new Size(584, 353);
            flowLayoutPanelChat.TabIndex = 0;
            flowLayoutPanelChat.WrapContents = false;
            // 
            // kryptonTextBoxMessage
            // 
            kryptonTextBoxMessage.Dock = DockStyle.Fill;
            kryptonTextBoxMessage.Location = new Point(0, 0);
            kryptonTextBoxMessage.Multiline = true;
            kryptonTextBoxMessage.Name = "kryptonTextBoxMessage";
            kryptonTextBoxMessage.Size = new Size(531, 46);
            kryptonTextBoxMessage.TabIndex = 1;
            // 
            // kryptonButtonSend
            // 
            kryptonButtonSend.Dock = DockStyle.Right;
            kryptonButtonSend.Location = new Point(531, 0);
            kryptonButtonSend.Name = "kryptonButtonSend";
            kryptonButtonSend.Size = new Size(53, 46);
            kryptonButtonSend.TabIndex = 2;
            kryptonButtonSend.Values.DropDownArrowColor = Color.Empty;
            kryptonButtonSend.Values.Text = "Send";
            kryptonButtonSend.Click += kryptonButtonSend_Click;
            // 
            // FormTest
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 404);
            Controls.Add(splitContainer1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormTest";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormTest";
            (splitContainer1.Panel1).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            (splitContainer1.Panel2).EndInit();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Krypton.Toolkit.KryptonSplitContainer splitContainer1;
        private Krypton.Toolkit.KryptonTextBox kryptonTextBoxMessage;
        private Krypton.Toolkit.KryptonButton kryptonButtonSend;
        private FlowLayoutPanel flowLayoutPanelChat;
    }
}