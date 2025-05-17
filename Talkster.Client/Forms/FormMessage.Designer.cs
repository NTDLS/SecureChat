using Krypton.Toolkit;

namespace Talkster.Client.Forms
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
            splitContainer1 = new KryptonSplitContainer();
            flowPanel = new FlowLayoutPanel();
            textBoxMessage = new KryptonTextBox();
            buttonSend = new KryptonButton();
            toolStrip1 = new ToolStrip();
            toolStripButtonTerminate = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripButtonAttachFile = new ToolStripButton();
            toolStripButtonVoiceCall = new ToolStripButton();
            toolStripButtonVoiceCallEnd = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            toolStripButtonExport = new ToolStripButton();
            toolStripButtonProperties = new ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            (splitContainer1.Panel1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            (splitContainer1.Panel2).BeginInit();
            splitContainer1.Panel2.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel2;
            splitContainer1.Location = new Point(5, 25);
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // 
            // 
            splitContainer1.Panel1.Controls.Add(flowPanel);
            // 
            // 
            // 
            splitContainer1.Panel2.Controls.Add(textBoxMessage);
            splitContainer1.Panel2.Controls.Add(buttonSend);
            splitContainer1.Size = new Size(534, 451);
            splitContainer1.SplitterDistance = 393;
            splitContainer1.TabIndex = 2;
            // 
            // flowPanel
            // 
            flowPanel.AllowDrop = true;
            flowPanel.AutoScroll = true;
            flowPanel.AutoSize = true;
            flowPanel.BackColor = Color.Transparent;
            flowPanel.BorderStyle = BorderStyle.Fixed3D;
            flowPanel.Dock = DockStyle.Fill;
            flowPanel.FlowDirection = FlowDirection.TopDown;
            flowPanel.Location = new Point(0, 0);
            flowPanel.Name = "flowPanel";
            flowPanel.Size = new Size(534, 393);
            flowPanel.TabIndex = 2;
            flowPanel.WrapContents = false;
            // 
            // textBoxMessage
            // 
            textBoxMessage.Dock = DockStyle.Fill;
            textBoxMessage.Location = new Point(0, 0);
            textBoxMessage.Multiline = true;
            textBoxMessage.Name = "textBoxMessage";
            textBoxMessage.Size = new Size(477, 53);
            textBoxMessage.TabIndex = 0;
            // 
            // buttonSend
            // 
            buttonSend.Dock = DockStyle.Right;
            buttonSend.Location = new Point(477, 0);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new Size(57, 53);
            buttonSend.TabIndex = 1;
            buttonSend.Values.DropDownArrowColor = Color.Empty;
            buttonSend.Values.Text = "Send";
            buttonSend.Click += ButtonSend_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.Font = new Font("Segoe UI", 9F);
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripButtonTerminate, toolStripSeparator1, toolStripButtonAttachFile, toolStripButtonVoiceCall, toolStripButtonVoiceCallEnd, toolStripSeparator2, toolStripButtonExport, toolStripButtonProperties });
            toolStrip1.Location = new Point(5, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(534, 25);
            toolStrip1.TabIndex = 4;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonTerminate
            // 
            toolStripButtonTerminate.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButtonTerminate.Image = (Image)resources.GetObject("toolStripButtonTerminate.Image");
            toolStripButtonTerminate.ImageTransparentColor = Color.Magenta;
            toolStripButtonTerminate.Name = "toolStripButtonTerminate";
            toolStripButtonTerminate.Size = new Size(23, 22);
            toolStripButtonTerminate.Text = "Terminate";
            toolStripButtonTerminate.ToolTipText = "Terminate";
            toolStripButtonTerminate.Click += ToolStripButtonTerminate_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // toolStripButtonAttachFile
            // 
            toolStripButtonAttachFile.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButtonAttachFile.Image = (Image)resources.GetObject("toolStripButtonAttachFile.Image");
            toolStripButtonAttachFile.ImageTransparentColor = Color.Magenta;
            toolStripButtonAttachFile.Name = "toolStripButtonAttachFile";
            toolStripButtonAttachFile.Size = new Size(23, 22);
            toolStripButtonAttachFile.Text = "Attach File";
            toolStripButtonAttachFile.Click += ToolStripButtonAttachFile_Click;
            // 
            // toolStripButtonVoiceCall
            // 
            toolStripButtonVoiceCall.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButtonVoiceCall.Image = (Image)resources.GetObject("toolStripButtonVoiceCall.Image");
            toolStripButtonVoiceCall.ImageTransparentColor = Color.Magenta;
            toolStripButtonVoiceCall.Name = "toolStripButtonVoiceCall";
            toolStripButtonVoiceCall.Size = new Size(23, 22);
            toolStripButtonVoiceCall.Text = "Voice Call";
            toolStripButtonVoiceCall.Click += ToolStripButtonVoiceCall_Click;
            // 
            // toolStripButtonVoiceCallEnd
            // 
            toolStripButtonVoiceCallEnd.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButtonVoiceCallEnd.Enabled = false;
            toolStripButtonVoiceCallEnd.Image = (Image)resources.GetObject("toolStripButtonVoiceCallEnd.Image");
            toolStripButtonVoiceCallEnd.ImageTransparentColor = Color.Magenta;
            toolStripButtonVoiceCallEnd.Name = "toolStripButtonVoiceCallEnd";
            toolStripButtonVoiceCallEnd.Size = new Size(23, 22);
            toolStripButtonVoiceCallEnd.Text = "End Voice Call";
            toolStripButtonVoiceCallEnd.Click += ToolStripButtonVoiceCallEnd_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 25);
            // 
            // toolStripButtonExport
            // 
            toolStripButtonExport.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButtonExport.Image = (Image)resources.GetObject("toolStripButtonExport.Image");
            toolStripButtonExport.ImageTransparentColor = Color.Magenta;
            toolStripButtonExport.Name = "toolStripButtonExport";
            toolStripButtonExport.Size = new Size(23, 22);
            toolStripButtonExport.Text = "Export";
            toolStripButtonExport.Click += ToolStripButtonExport_Click;
            // 
            // toolStripButtonProperties
            // 
            toolStripButtonProperties.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButtonProperties.Image = (Image)resources.GetObject("toolStripButtonProperties.Image");
            toolStripButtonProperties.ImageTransparentColor = Color.Magenta;
            toolStripButtonProperties.Name = "toolStripButtonProperties";
            toolStripButtonProperties.Size = new Size(23, 22);
            toolStripButtonProperties.Text = "Properties";
            toolStripButtonProperties.Click += ToolStripButtonProperties_Click;
            // 
            // FormMessage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(544, 481);
            Controls.Add(splitContainer1);
            Controls.Add(toolStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(400, 400);
            Name = "FormMessage";
            Opacity = 0.95D;
            Padding = new Padding(5, 0, 5, 5);
            SizeGripStyle = SizeGripStyle.Show;
            StartPosition = FormStartPosition.Manual;
            Text = "Talkster";
            (splitContainer1.Panel1).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            (splitContainer1.Panel2).EndInit();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private KryptonSplitContainer splitContainer1;
        private KryptonTextBox textBoxMessage;
        private KryptonButton buttonSend;
        private FlowLayoutPanel flowPanel;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButtonAttachFile;
        private ToolStripButton toolStripButtonTerminate;
        private ToolStripButton toolStripButtonProperties;
        private ToolStripButton toolStripButtonVoiceCall;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton toolStripButtonExport;
        private ToolStripButton toolStripButtonVoiceCallEnd;
    }
}