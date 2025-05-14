using Krypton.Toolkit;

namespace SecureChat.Client.Forms
{
    partial class FormProfile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProfile));
            labelWarning = new KryptonLabel();
            labelDisplayName = new KryptonLabel();
            labelTagline = new KryptonLabel();
            textBoxDisplayName = new KryptonTextBox();
            textBoxTagline = new KryptonTextBox();
            buttonSave = new KryptonButton();
            buttonCancel = new KryptonButton();
            textBoxBiography = new KryptonTextBox();
            labelBiography = new KryptonLabel();
            SuspendLayout();
            // 
            // labelWarning
            // 
            labelWarning.LabelStyle = LabelStyle.BoldControl;
            labelWarning.Location = new Point(23, 12);
            labelWarning.Name = "labelWarning";
            labelWarning.Size = new Size(244, 20);
            labelWarning.TabIndex = 0;
            labelWarning.Values.Text = "This profile is viewable to your contacts.";
            // 
            // labelDisplayName
            // 
            labelDisplayName.Location = new Point(8, 38);
            labelDisplayName.Name = "labelDisplayName";
            labelDisplayName.Size = new Size(86, 20);
            labelDisplayName.TabIndex = 1;
            labelDisplayName.Values.Text = "Display Name";
            // 
            // labelTagline
            // 
            labelTagline.Location = new Point(8, 84);
            labelTagline.Name = "labelTagline";
            labelTagline.Size = new Size(50, 20);
            labelTagline.TabIndex = 2;
            labelTagline.Values.Text = "Tagline";
            // 
            // textBoxDisplayName
            // 
            textBoxDisplayName.Location = new Point(12, 58);
            textBoxDisplayName.Name = "textBoxDisplayName";
            textBoxDisplayName.Size = new Size(273, 23);
            textBoxDisplayName.TabIndex = 5;
            // 
            // textBoxTagline
            // 
            textBoxTagline.Location = new Point(12, 104);
            textBoxTagline.Name = "textBoxTagline";
            textBoxTagline.Size = new Size(273, 23);
            textBoxTagline.TabIndex = 6;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(129, 243);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(75, 23);
            buttonSave.TabIndex = 7;
            buttonSave.Values.DropDownArrowColor = Color.Empty;
            buttonSave.Values.Text = "Save";
            buttonSave.Click += ButtonSave_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(210, 243);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 8;
            buttonCancel.Values.DropDownArrowColor = Color.Empty;
            buttonCancel.Values.Text = "Cancel";
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // textBoxBiography
            // 
            textBoxBiography.Location = new Point(12, 150);
            textBoxBiography.Multiline = true;
            textBoxBiography.Name = "textBoxBiography";
            textBoxBiography.Size = new Size(273, 86);
            textBoxBiography.TabIndex = 10;
            // 
            // labelBiography
            // 
            labelBiography.Location = new Point(8, 130);
            labelBiography.Name = "labelBiography";
            labelBiography.Size = new Size(66, 20);
            labelBiography.TabIndex = 9;
            labelBiography.Values.Text = "Biography";
            // 
            // FormProfile
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(307, 282);
            Controls.Add(textBoxBiography);
            Controls.Add(buttonCancel);
            Controls.Add(buttonSave);
            Controls.Add(textBoxTagline);
            Controls.Add(textBoxDisplayName);
            Controls.Add(labelWarning);
            Controls.Add(labelBiography);
            Controls.Add(labelTagline);
            Controls.Add(labelDisplayName);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormProfile";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Secure Chat";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private KryptonLabel labelWarning;
        private KryptonLabel labelDisplayName;
        private KryptonLabel labelTagline;
        private KryptonTextBox textBoxDisplayName;
        private KryptonTextBox textBoxTagline;
        private KryptonButton buttonSave;
        private KryptonButton buttonCancel;
        private KryptonTextBox textBoxBiography;
        private KryptonLabel labelBiography;
    }
}