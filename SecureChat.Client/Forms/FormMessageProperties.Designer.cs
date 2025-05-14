using Krypton.Toolkit;

namespace SecureChat.Client.Forms
{
    partial class FormMessageProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMessageProperties));
            labelAccountId = new KryptonLabel();
            textBoxAccountId = new KryptonTextBox();
            labelDisplayName = new KryptonLabel();
            textBoxDisplayName = new KryptonTextBox();
            buttonClose = new KryptonButton();
            textBoxPublicRsaKey = new KryptonTextBox();
            labelPublicRsaKey = new KryptonLabel();
            textBoxSessionId = new KryptonTextBox();
            LebelSessionId = new KryptonLabel();
            textBoxSharedSecret = new KryptonTextBox();
            labelSharedSecret = new KryptonLabel();
            textBoxPrivateRsaKey = new KryptonTextBox();
            labelPrivateRsaKey = new KryptonLabel();
            labelSharedSecretLength = new KryptonLabel();
            labelPrivateRsaKeyLength = new KryptonLabel();
            labelPublicRsaKeyLength = new KryptonLabel();
            SuspendLayout();
            // 
            // labelAccountId
            // 
            labelAccountId.Location = new Point(12, 57);
            labelAccountId.Name = "labelAccountId";
            labelAccountId.Size = new Size(69, 20);
            labelAccountId.TabIndex = 0;
            labelAccountId.Values.Text = "Account Id";
            // 
            // textBoxAccountId
            // 
            textBoxAccountId.Location = new Point(16, 79);
            textBoxAccountId.Name = "textBoxAccountId";
            textBoxAccountId.ReadOnly = true;
            textBoxAccountId.Size = new Size(288, 23);
            textBoxAccountId.TabIndex = 1;
            // 
            // labelDisplayName
            // 
            labelDisplayName.Location = new Point(12, 8);
            labelDisplayName.Name = "labelDisplayName";
            labelDisplayName.Size = new Size(86, 20);
            labelDisplayName.TabIndex = 2;
            labelDisplayName.Values.Text = "Display Name";
            // 
            // textBoxDisplayName
            // 
            textBoxDisplayName.Location = new Point(16, 30);
            textBoxDisplayName.Name = "textBoxDisplayName";
            textBoxDisplayName.ReadOnly = true;
            textBoxDisplayName.Size = new Size(288, 23);
            textBoxDisplayName.TabIndex = 0;
            // 
            // buttonClose
            // 
            buttonClose.Location = new Point(229, 304);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(75, 23);
            buttonClose.TabIndex = 5;
            buttonClose.Values.DropDownArrowColor = Color.Empty;
            buttonClose.Values.Text = "Close";
            buttonClose.Click += buttonClose_Click;
            // 
            // textBoxPublicRsaKey
            // 
            textBoxPublicRsaKey.Location = new Point(16, 177);
            textBoxPublicRsaKey.Name = "textBoxPublicRsaKey";
            textBoxPublicRsaKey.ReadOnly = true;
            textBoxPublicRsaKey.Size = new Size(288, 23);
            textBoxPublicRsaKey.TabIndex = 3;
            // 
            // labelPublicRsaKey
            // 
            labelPublicRsaKey.Location = new Point(12, 155);
            labelPublicRsaKey.Name = "labelPublicRsaKey";
            labelPublicRsaKey.Size = new Size(103, 20);
            labelPublicRsaKey.TabIndex = 9;
            labelPublicRsaKey.Values.Text = "Public Key (hash)";
            // 
            // textBoxSessionId
            // 
            textBoxSessionId.Location = new Point(16, 128);
            textBoxSessionId.Name = "textBoxSessionId";
            textBoxSessionId.ReadOnly = true;
            textBoxSessionId.Size = new Size(288, 23);
            textBoxSessionId.TabIndex = 2;
            // 
            // LebelSessionId
            // 
            LebelSessionId.Location = new Point(12, 106);
            LebelSessionId.Name = "LebelSessionId";
            LebelSessionId.Size = new Size(65, 20);
            LebelSessionId.TabIndex = 7;
            LebelSessionId.Values.Text = "Session Id";
            // 
            // textBoxSharedSecret
            // 
            textBoxSharedSecret.Location = new Point(16, 275);
            textBoxSharedSecret.Name = "textBoxSharedSecret";
            textBoxSharedSecret.ReadOnly = true;
            textBoxSharedSecret.Size = new Size(288, 23);
            textBoxSharedSecret.TabIndex = 5;
            // 
            // labelSharedSecret
            // 
            labelSharedSecret.Location = new Point(12, 253);
            labelSharedSecret.Name = "labelSharedSecret";
            labelSharedSecret.Size = new Size(122, 20);
            labelSharedSecret.TabIndex = 13;
            labelSharedSecret.Values.Text = "Shared Secret (hash)";
            // 
            // textBoxPrivateRsaKey
            // 
            textBoxPrivateRsaKey.Location = new Point(16, 226);
            textBoxPrivateRsaKey.Name = "textBoxPrivateRsaKey";
            textBoxPrivateRsaKey.ReadOnly = true;
            textBoxPrivateRsaKey.Size = new Size(288, 23);
            textBoxPrivateRsaKey.TabIndex = 4;
            // 
            // labelPrivateRsaKey
            // 
            labelPrivateRsaKey.Location = new Point(12, 204);
            labelPrivateRsaKey.Name = "labelPrivateRsaKey";
            labelPrivateRsaKey.Size = new Size(107, 20);
            labelPrivateRsaKey.TabIndex = 11;
            labelPrivateRsaKey.Values.Text = "Private Key (hash)";
            // 
            // labelSharedSecretLength
            // 
            labelSharedSecretLength.Location = new Point(140, 253);
            labelSharedSecretLength.Name = "labelSharedSecretLength";
            labelSharedSecretLength.Size = new Size(50, 20);
            labelSharedSecretLength.TabIndex = 14;
            labelSharedSecretLength.Values.Text = "000bits";
            // 
            // labelPrivateRsaKeyLength
            // 
            labelPrivateRsaKeyLength.Location = new Point(140, 204);
            labelPrivateRsaKeyLength.Name = "labelPrivateRsaKeyLength";
            labelPrivateRsaKeyLength.Size = new Size(50, 20);
            labelPrivateRsaKeyLength.TabIndex = 15;
            labelPrivateRsaKeyLength.Values.Text = "000bits";
            // 
            // labelPublicRsaKeyLength
            // 
            labelPublicRsaKeyLength.Location = new Point(140, 155);
            labelPublicRsaKeyLength.Name = "labelPublicRsaKeyLength";
            labelPublicRsaKeyLength.Size = new Size(50, 20);
            labelPublicRsaKeyLength.TabIndex = 16;
            labelPublicRsaKeyLength.Values.Text = "000bits";
            // 
            // FormMessageProperties
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(324, 340);
            Controls.Add(textBoxSharedSecret);
            Controls.Add(textBoxPrivateRsaKey);
            Controls.Add(textBoxPublicRsaKey);
            Controls.Add(textBoxSessionId);
            Controls.Add(textBoxDisplayName);
            Controls.Add(textBoxAccountId);
            Controls.Add(labelSharedSecret);
            Controls.Add(labelPrivateRsaKey);
            Controls.Add(labelPublicRsaKey);
            Controls.Add(LebelSessionId);
            Controls.Add(buttonClose);
            Controls.Add(labelDisplayName);
            Controls.Add(labelAccountId);
            Controls.Add(labelPublicRsaKeyLength);
            Controls.Add(labelPrivateRsaKeyLength);
            Controls.Add(labelSharedSecretLength);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormMessageProperties";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Secure Chat";
            ResumeLayout(false);
            PerformLayout();
        }


        #endregion

        private KryptonLabel labelAccountId;
        private KryptonTextBox textBoxAccountId;
        private KryptonLabel labelDisplayName;
        private KryptonTextBox textBoxDisplayName;
        private KryptonButton buttonClose;
        private KryptonTextBox textBoxPublicRsaKey;
        private KryptonLabel labelPublicRsaKey;
        private KryptonTextBox textBoxSessionId;
        private KryptonLabel LebelSessionId;
        private KryptonTextBox textBoxSharedSecret;
        private KryptonLabel labelSharedSecret;
        private KryptonTextBox textBoxPrivateRsaKey;
        private KryptonLabel labelPrivateRsaKey;
        private KryptonLabel labelSharedSecretLength;
        private KryptonLabel labelPrivateRsaKeyLength;
        private KryptonLabel labelPublicRsaKeyLength;
    }
}