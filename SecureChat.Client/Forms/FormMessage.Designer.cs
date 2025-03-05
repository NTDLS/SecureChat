namespace SecureChat.Client.Forms
{
    partial class FormMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMessage));
            splitContainer1 = new SplitContainer();
            flowPanel = new FlowLayoutPanel();
            textBoxMessage = new TextBox();
            buttonSend = new Button();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            exportToolStripMenuItem = new ToolStripMenuItem();
            attachFileToolStripMenuItem = new ToolStripMenuItem();
            imageToolStripMenuItem = new ToolStripMenuItem();
            connectionToolStripMenuItem = new ToolStripMenuItem();
            terminateToolStripMenuItem = new ToolStripMenuItem();
            propertiesToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(5, 24);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(flowPanel);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(textBoxMessage);
            splitContainer1.Panel2.Controls.Add(buttonSend);
            splitContainer1.Size = new Size(524, 482);
            splitContainer1.SplitterDistance = 424;
            splitContainer1.TabIndex = 2;
            // 
            // flowPanel
            // 
            flowPanel.AutoScroll = true;
            flowPanel.AutoSize = true;
            flowPanel.BorderStyle = BorderStyle.Fixed3D;
            flowPanel.Dock = DockStyle.Fill;
            flowPanel.FlowDirection = FlowDirection.TopDown;
            flowPanel.Location = new Point(0, 0);
            flowPanel.Name = "flowPanel";
            flowPanel.Size = new Size(524, 424);
            flowPanel.TabIndex = 2;
            flowPanel.WrapContents = false;
            // 
            // textBoxMessage
            // 
            textBoxMessage.Dock = DockStyle.Fill;
            textBoxMessage.Location = new Point(0, 0);
            textBoxMessage.Multiline = true;
            textBoxMessage.Name = "textBoxMessage";
            textBoxMessage.Size = new Size(449, 54);
            textBoxMessage.TabIndex = 0;
            // 
            // buttonSend
            // 
            buttonSend.Dock = DockStyle.Right;
            buttonSend.Location = new Point(449, 0);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new Size(75, 54);
            buttonSend.TabIndex = 1;
            buttonSend.Text = "Send";
            buttonSend.UseVisualStyleBackColor = true;
            buttonSend.Click += ButtonSend_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, connectionToolStripMenuItem });
            menuStrip1.Location = new Point(5, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(524, 24);
            menuStrip1.TabIndex = 3;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { exportToolStripMenuItem, attachFileToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // exportToolStripMenuItem
            // 
            exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            exportToolStripMenuItem.Size = new Size(130, 22);
            exportToolStripMenuItem.Text = "Export";
            // 
            // attachFileToolStripMenuItem
            // 
            attachFileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { imageToolStripMenuItem });
            attachFileToolStripMenuItem.Name = "attachFileToolStripMenuItem";
            attachFileToolStripMenuItem.Size = new Size(130, 22);
            attachFileToolStripMenuItem.Text = "Attach File";
            // 
            // imageToolStripMenuItem
            // 
            imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            imageToolStripMenuItem.Size = new Size(107, 22);
            imageToolStripMenuItem.Text = "Image";
            imageToolStripMenuItem.Click += ImageToolStripMenuItem_Click;
            // 
            // connectionToolStripMenuItem
            // 
            connectionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { terminateToolStripMenuItem, propertiesToolStripMenuItem });
            connectionToolStripMenuItem.Name = "connectionToolStripMenuItem";
            connectionToolStripMenuItem.Size = new Size(81, 20);
            connectionToolStripMenuItem.Text = "Connection";
            // 
            // terminateToolStripMenuItem
            // 
            terminateToolStripMenuItem.Name = "terminateToolStripMenuItem";
            terminateToolStripMenuItem.Size = new Size(127, 22);
            terminateToolStripMenuItem.Text = "Terminate";
            terminateToolStripMenuItem.Click += TerminateToolStripMenuItem_Click;
            // 
            // propertiesToolStripMenuItem
            // 
            propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            propertiesToolStripMenuItem.Size = new Size(127, 22);
            propertiesToolStripMenuItem.Text = "Properties";
            // 
            // FormMessage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(534, 511);
            Controls.Add(splitContainer1);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "FormMessage";
            Opacity = 0.95D;
            Padding = new Padding(5, 0, 5, 5);
            SizeGripStyle = SizeGripStyle.Show;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Secure Chat";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private SplitContainer splitContainer1;
        private TextBox textBoxMessage;
        private Button buttonSend;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exportToolStripMenuItem;
        private ToolStripMenuItem attachFileToolStripMenuItem;
        private ToolStripMenuItem connectionToolStripMenuItem;
        private ToolStripMenuItem terminateToolStripMenuItem;
        private ToolStripMenuItem propertiesToolStripMenuItem;
        private ToolStripMenuItem imageToolStripMenuItem;
        private FlowLayoutPanel flowPanel;
    }
}