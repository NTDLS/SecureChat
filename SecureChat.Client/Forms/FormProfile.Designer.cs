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
            labelWarning = new Label();
            labelDisplayName = new Label();
            labelTagline = new Label();
            textBoxDisplayName = new TextBox();
            textBoxTagline = new TextBox();
            buttonSave = new Button();
            buttonCancel = new Button();
            textBoxBiography = new TextBox();
            labelBiography = new Label();
            SuspendLayout();
            // 
            // labelWarning
            // 
            labelWarning.AutoSize = true;
            labelWarning.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelWarning.ForeColor = Color.FromArgb(192, 0, 0);
            labelWarning.Location = new Point(12, 9);
            labelWarning.Name = "labelWarning";
            labelWarning.Size = new Size(273, 20);
            labelWarning.TabIndex = 0;
            labelWarning.Text = "This profile is viewable to your contacts.";
            // 
            // labelDisplayName
            // 
            labelDisplayName.AutoSize = true;
            labelDisplayName.Location = new Point(12, 40);
            labelDisplayName.Name = "labelDisplayName";
            labelDisplayName.Size = new Size(80, 15);
            labelDisplayName.TabIndex = 1;
            labelDisplayName.Text = "Display Name";
            // 
            // labelTagline
            // 
            labelTagline.AutoSize = true;
            labelTagline.Location = new Point(12, 84);
            labelTagline.Name = "labelTagline";
            labelTagline.Size = new Size(45, 15);
            labelTagline.TabIndex = 2;
            labelTagline.Text = "Tagline";
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
            textBoxTagline.Location = new Point(12, 102);
            textBoxTagline.Name = "textBoxTagline";
            textBoxTagline.Size = new Size(273, 23);
            textBoxTagline.TabIndex = 6;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(129, 238);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(75, 23);
            buttonSave.TabIndex = 7;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += ButtonSave_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(210, 238);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 8;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // textBoxBiography
            // 
            textBoxBiography.Location = new Point(12, 146);
            textBoxBiography.Multiline = true;
            textBoxBiography.Name = "textBoxBiography";
            textBoxBiography.Size = new Size(273, 86);
            textBoxBiography.TabIndex = 10;
            // 
            // labelBiography
            // 
            labelBiography.AutoSize = true;
            labelBiography.Location = new Point(12, 128);
            labelBiography.Name = "labelBiography";
            labelBiography.Size = new Size(61, 15);
            labelBiography.TabIndex = 9;
            labelBiography.Text = "Biography";
            // 
            // FormProfile
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(299, 271);
            Controls.Add(textBoxBiography);
            Controls.Add(labelBiography);
            Controls.Add(buttonCancel);
            Controls.Add(buttonSave);
            Controls.Add(textBoxTagline);
            Controls.Add(textBoxDisplayName);
            Controls.Add(labelTagline);
            Controls.Add(labelDisplayName);
            Controls.Add(labelWarning);
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

        private Label labelWarning;
        private Label labelDisplayName;
        private Label labelTagline;
        private TextBox textBoxDisplayName;
        private TextBox textBoxTagline;
        private Button buttonSave;
        private Button buttonCancel;
        private TextBox textBoxBiography;
        private Label labelBiography;
    }
}