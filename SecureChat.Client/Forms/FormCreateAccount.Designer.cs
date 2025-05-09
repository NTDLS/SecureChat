using Krypton.Toolkit;

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
            buttonCreate = new KryptonButton();
            buttonCancel = new KryptonButton();
            labelUsername = new KryptonLabel();
            labelDisplayName = new KryptonLabel();
            labelPassword = new KryptonLabel();
            textBoxUsername = new KryptonTextBox();
            textBoxDisplayName = new KryptonTextBox();
            textBoxPassword = new KryptonTextBox();
            textBoxConfirmPassword = new KryptonTextBox();
            labelConfirmPassword = new KryptonLabel();
            SuspendLayout();
            // 
            // buttonCreate
            // 
            buttonCreate.Location = new Point(98, 222);
            buttonCreate.Name = "buttonCreate";
            buttonCreate.Size = new Size(75, 23);
            buttonCreate.TabIndex = 4;
            buttonCreate.Values.DropDownArrowColor = Color.Empty;
            buttonCreate.Values.Text = "Create";
            buttonCreate.Click += ButtonCreate_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(179, 222);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 5;
            buttonCancel.Values.DropDownArrowColor = Color.Empty;
            buttonCancel.Values.Text = "Cancel";
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // labelUsername
            // 
            labelUsername.Location = new Point(8, 20);
            labelUsername.Name = "labelUsername";
            labelUsername.Size = new Size(65, 20);
            labelUsername.TabIndex = 2;
            labelUsername.Values.Text = "Username";
            // 
            // labelDisplayName
            // 
            labelDisplayName.Location = new Point(8, 65);
            labelDisplayName.Name = "labelDisplayName";
            labelDisplayName.Size = new Size(86, 20);
            labelDisplayName.TabIndex = 3;
            labelDisplayName.Values.Text = "Display Name";
            // 
            // labelPassword
            // 
            labelPassword.Location = new Point(8, 126);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(62, 20);
            labelPassword.TabIndex = 4;
            labelPassword.Values.Text = "Password";
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
            labelConfirmPassword.Location = new Point(8, 171);
            labelConfirmPassword.Name = "labelConfirmPassword";
            labelConfirmPassword.Size = new Size(109, 20);
            labelConfirmPassword.TabIndex = 9;
            labelConfirmPassword.Values.Text = "Confirm Password";
            // 
            // FormCreateAccount
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(271, 255);
            Controls.Add(textBoxConfirmPassword);
            Controls.Add(textBoxPassword);
            Controls.Add(textBoxDisplayName);
            Controls.Add(textBoxUsername);
            Controls.Add(buttonCancel);
            Controls.Add(buttonCreate);
            Controls.Add(labelConfirmPassword);
            Controls.Add(labelPassword);
            Controls.Add(labelDisplayName);
            Controls.Add(labelUsername);
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

        private KryptonButton buttonCreate;
        private KryptonButton buttonCancel;
        private KryptonLabel labelUsername;
        private KryptonLabel labelDisplayName;
        private KryptonLabel labelPassword;
        private KryptonTextBox textBoxUsername;
        private KryptonTextBox textBoxDisplayName;
        private KryptonTextBox textBoxPassword;
        private KryptonTextBox textBoxConfirmPassword;
        private KryptonLabel labelConfirmPassword;
    }
}