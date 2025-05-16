using Krypton.Toolkit;

namespace Talkster.Client.Forms
{
    partial class FormLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            labelUsername = new KryptonLabel();
            textBoxUsername = new KryptonTextBox();
            labelPassword = new KryptonLabel();
            textBoxPassword = new KryptonTextBox();
            pictureBoxLogo = new KryptonPictureBox();
            buttonLogin = new KryptonButton();
            buttonCancel = new KryptonButton();
            linkLabelCreateAccount = new KryptonLinkLabel();
            checkBoxStayLoggedIn = new KryptonCheckBox();
            buttonSettings = new KryptonButton();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).BeginInit();
            SuspendLayout();
            // 
            // labelUsername
            // 
            labelUsername.Location = new Point(96, 13);
            labelUsername.Name = "labelUsername";
            labelUsername.Size = new Size(65, 20);
            labelUsername.TabIndex = 0;
            labelUsername.Values.Text = "Username";
            // 
            // textBoxUsername
            // 
            textBoxUsername.Location = new Point(100, 31);
            textBoxUsername.Name = "textBoxUsername";
            textBoxUsername.Size = new Size(197, 23);
            textBoxUsername.TabIndex = 0;
            // 
            // labelPassword
            // 
            labelPassword.Location = new Point(96, 61);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(62, 20);
            labelPassword.TabIndex = 2;
            labelPassword.Values.Text = "Password";
            // 
            // textBoxPassword
            // 
            textBoxPassword.Location = new Point(100, 79);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.PasswordChar = '*';
            textBoxPassword.Size = new Size(197, 23);
            textBoxPassword.TabIndex = 1;
            // 
            // pictureBoxLogo
            // 
            pictureBoxLogo.BackgroundImage = (Image)resources.GetObject("pictureBoxLogo.BackgroundImage");
            pictureBoxLogo.BackgroundImageLayout = ImageLayout.Center;
            pictureBoxLogo.Location = new Point(12, 31);
            pictureBoxLogo.Name = "pictureBoxLogo";
            pictureBoxLogo.Size = new Size(73, 70);
            pictureBoxLogo.TabIndex = 4;
            pictureBoxLogo.TabStop = false;
            // 
            // buttonLogin
            // 
            buttonLogin.Location = new Point(132, 162);
            buttonLogin.Name = "buttonLogin";
            buttonLogin.Size = new Size(75, 25);
            buttonLogin.TabIndex = 4;
            buttonLogin.Values.DropDownArrowColor = Color.Empty;
            buttonLogin.Values.Text = "Login";
            buttonLogin.Click += ButtonLogin_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(213, 162);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 25);
            buttonCancel.TabIndex = 5;
            buttonCancel.Values.DropDownArrowColor = Color.Empty;
            buttonCancel.Values.Text = "Cancel";
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // linkLabelCreateAccount
            // 
            linkLabelCreateAccount.Location = new Point(96, 134);
            linkLabelCreateAccount.Name = "linkLabelCreateAccount";
            linkLabelCreateAccount.Size = new Size(205, 20);
            linkLabelCreateAccount.TabIndex = 3;
            linkLabelCreateAccount.Values.Text = "Don't have an account? Create one!";
            linkLabelCreateAccount.LinkClicked += LinkLabelCreateAccount_LinkClicked;
            // 
            // checkBoxStayLoggedIn
            // 
            checkBoxStayLoggedIn.Location = new Point(100, 108);
            checkBoxStayLoggedIn.Name = "checkBoxStayLoggedIn";
            checkBoxStayLoggedIn.Size = new Size(107, 20);
            checkBoxStayLoggedIn.TabIndex = 2;
            checkBoxStayLoggedIn.Values.Text = "Stay logged in?";
            // 
            // buttonSettings
            // 
            buttonSettings.Location = new Point(36, 162);
            buttonSettings.Name = "buttonSettings";
            buttonSettings.Size = new Size(90, 25);
            buttonSettings.TabIndex = 6;
            buttonSettings.Values.DropDownArrowColor = Color.Empty;
            buttonSettings.Values.Text = "Settings";
            buttonSettings.Click += ButtonSettings_Click;
            // 
            // FormLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(325, 195);
            Controls.Add(buttonSettings);
            Controls.Add(checkBoxStayLoggedIn);
            Controls.Add(linkLabelCreateAccount);
            Controls.Add(buttonCancel);
            Controls.Add(buttonLogin);
            Controls.Add(pictureBoxLogo);
            Controls.Add(textBoxPassword);
            Controls.Add(labelPassword);
            Controls.Add(textBoxUsername);
            Controls.Add(labelUsername);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(335, 240);
            Name = "FormLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Talkster";
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }


        #endregion

        private KryptonLabel labelUsername;
        private KryptonTextBox textBoxUsername;
        private KryptonLabel labelPassword;
        private KryptonTextBox textBoxPassword;
        private KryptonPictureBox pictureBoxLogo;
        private KryptonButton buttonLogin;
        private KryptonButton buttonCancel;
        private KryptonLinkLabel linkLabelCreateAccount;
        private KryptonCheckBox checkBoxStayLoggedIn;
        private KryptonButton buttonSettings;
    }
}