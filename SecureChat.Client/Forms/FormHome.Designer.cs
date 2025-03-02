using SecureChat.Client.Controls;

namespace SecureChat.Client.Forms
{
    partial class FormHome
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHome));
            treeViewAcquaintances = new DoubleBufferedTreeView();
            SuspendLayout();
            // 
            // treeViewAcquaintances
            // 
            treeViewAcquaintances.Dock = DockStyle.Fill;
            treeViewAcquaintances.HotTracking = true;
            treeViewAcquaintances.Location = new Point(10, 10);
            treeViewAcquaintances.Name = "treeViewAcquaintances";
            treeViewAcquaintances.ShowRootLines = false;
            treeViewAcquaintances.Size = new Size(265, 488);
            treeViewAcquaintances.TabIndex = 0;
            // 
            // FormHome
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(285, 508);
            Controls.Add(treeViewAcquaintances);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FormHome";
            Opacity = 0.95D;
            Padding = new Padding(10);
            SizeGripStyle = SizeGripStyle.Show;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Secure Chat";
            ResumeLayout(false);
        }

        #endregion

        private DoubleBufferedTreeView treeViewAcquaintances;
    }
}