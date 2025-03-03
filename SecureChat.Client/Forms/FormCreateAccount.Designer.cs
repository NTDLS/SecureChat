namespace SecureChat.Client.Forms
{
    partial class FormCreateAccount
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCreateAccount));
            buttonCreate = new Button();
            buttonCancel = new Button();
            labelUsername = new Label();
            labelDisplayName = new Label();
            labelPassword = new Label();
            textBoxUsername = new TextBox();
            textBoxDisplayName = new TextBox();
            textBoxPassword = new TextBox();
            textBoxConfirmPassword = new TextBox();
            labelConfirmPassword = new Label();
            SuspendLayout();
            // 
            // buttonCreate
            // 
            buttonCreate.Location = new Point(98, 221);
            buttonCreate.Name = "buttonCreate";
            buttonCreate.Size = new Size(75, 23);
            buttonCreate.TabIndex = 4;
            buttonCreate.Text = "Create";
            buttonCreate.UseVisualStyleBackColor = true;
            buttonCreate.Click += ButtonCreate_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(179, 221);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 5;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // labelUsername
            // 
            labelUsername.AutoSize = true;
            labelUsername.Location = new Point(12, 22);
            labelUsername.Name = "labelUsername";
            labelUsername.Size = new Size(60, 15);
            labelUsername.TabIndex = 2;
            labelUsername.Text = "Username";
            // 
            // labelDisplayName
            // 
            labelDisplayName.AutoSize = true;
            labelDisplayName.Location = new Point(12, 67);
            labelDisplayName.Name = "labelDisplayName";
            labelDisplayName.Size = new Size(80, 15);
            labelDisplayName.TabIndex = 3;
            labelDisplayName.Text = "Display Name";
            // 
            // labelPassword
            // 
            labelPassword.AutoSize = true;
            labelPassword.Location = new Point(12, 128);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(57, 15);
            labelPassword.TabIndex = 4;
            labelPassword.Text = "Password";
            // 
            // textBoxUsername
            // 
            textBoxUsername.Location = new Point(12, 40);
            textBoxUsername.Name = "textBoxUsername";
            textBoxUsername.Size = new Size(242, 23);
            textBoxUsername.TabIndex = 0;
            // 
            // textBoxDisplayName
            // 
            textBoxDisplayName.Location = new Point(12, 85);
            textBoxDisplayName.Name = "textBoxDisplayName";
            textBoxDisplayName.Size = new Size(242, 23);
            textBoxDisplayName.TabIndex = 1;
            // 
            // textBoxPassword
            // 
            textBoxPassword.Location = new Point(12, 146);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.PasswordChar = '*';
            textBoxPassword.Size = new Size(242, 23);
            textBoxPassword.TabIndex = 2;
            // 
            // textBoxConfirmPassword
            // 
            textBoxConfirmPassword.Location = new Point(12, 191);
            textBoxConfirmPassword.Name = "textBoxConfirmPassword";
            textBoxConfirmPassword.PasswordChar = '*';
            textBoxConfirmPassword.Size = new Size(242, 23);
            textBoxConfirmPassword.TabIndex = 3;
            // 
            // labelConfirmPassword
            // 
            labelConfirmPassword.AutoSize = true;
            labelConfirmPassword.Location = new Point(12, 173);
            labelConfirmPassword.Name = "labelConfirmPassword";
            labelConfirmPassword.Size = new Size(104, 15);
            labelConfirmPassword.TabIndex = 9;
            labelConfirmPassword.Text = "Confirm Password";
            // 
            // FormCreateAccount
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(269, 256);
            Controls.Add(labelConfirmPassword);
            Controls.Add(textBoxConfirmPassword);
            Controls.Add(textBoxPassword);
            Controls.Add(textBoxDisplayName);
            Controls.Add(textBoxUsername);
            Controls.Add(labelPassword);
            Controls.Add(labelDisplayName);
            Controls.Add(labelUsername);
            Controls.Add(buttonCancel);
            Controls.Add(buttonCreate);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormCreateAccount";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Secure Chat";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonCreate;
        private Button buttonCancel;
        private Label labelUsername;
        private Label labelDisplayName;
        private Label labelPassword;
        private TextBox textBoxUsername;
        private TextBox textBoxDisplayName;
        private TextBox textBoxPassword;
        private TextBox textBoxConfirmPassword;
        private Label labelConfirmPassword;
    }
}