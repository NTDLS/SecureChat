using Krypton.Toolkit;

namespace SecureChat.Client.Forms
{
    partial class FormVoicePreCall
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormVoicePreCall));
            comboBoxAudioOutputDevice = new KryptonComboBox();
            labelAudioOutputDevice = new KryptonLabel();
            labelAudioInputDevice = new KryptonLabel();
            comboBoxAudioInputDevice = new KryptonComboBox();
            radioButtonBitRateHighFidelity = new KryptonRadioButton();
            radioButtonBitRateBalanced = new KryptonRadioButton();
            radioButtonBitRateStandard = new KryptonRadioButton();
            radioButtonBitRateLow = new KryptonRadioButton();
            buttonOk = new KryptonButton();
            buttonCancel = new KryptonButton();
            volumeMeterInput = new NAudio.Gui.VolumeMeter();
            kryptonPanel1 = new KryptonPanel();
            kryptonLabel1 = new KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)comboBoxAudioOutputDevice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)comboBoxAudioInputDevice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)kryptonPanel1).BeginInit();
            kryptonPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // comboBoxAudioOutputDevice
            // 
            comboBoxAudioOutputDevice.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxAudioOutputDevice.DropDownWidth = 457;
            comboBoxAudioOutputDevice.FormattingEnabled = true;
            comboBoxAudioOutputDevice.Location = new Point(12, 86);
            comboBoxAudioOutputDevice.Name = "comboBoxAudioOutputDevice";
            comboBoxAudioOutputDevice.Size = new Size(457, 22);
            comboBoxAudioOutputDevice.TabIndex = 1;
            // 
            // labelAudioOutputDevice
            // 
            labelAudioOutputDevice.Location = new Point(12, 66);
            labelAudioOutputDevice.Name = "labelAudioOutputDevice";
            labelAudioOutputDevice.Size = new Size(121, 20);
            labelAudioOutputDevice.TabIndex = 1;
            labelAudioOutputDevice.Values.Text = "Audio output device";
            // 
            // labelAudioInputDevice
            // 
            labelAudioInputDevice.Location = new Point(12, 18);
            labelAudioInputDevice.Name = "labelAudioInputDevice";
            labelAudioInputDevice.Size = new Size(113, 20);
            labelAudioInputDevice.TabIndex = 3;
            labelAudioInputDevice.Values.Text = "Audio input device";
            // 
            // comboBoxAudioInputDevice
            // 
            comboBoxAudioInputDevice.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxAudioInputDevice.DropDownWidth = 457;
            comboBoxAudioInputDevice.FormattingEnabled = true;
            comboBoxAudioInputDevice.Location = new Point(12, 38);
            comboBoxAudioInputDevice.Name = "comboBoxAudioInputDevice";
            comboBoxAudioInputDevice.Size = new Size(457, 22);
            comboBoxAudioInputDevice.TabIndex = 0;
            // 
            // radioButtonBitRateHighFidelity
            // 
            radioButtonBitRateHighFidelity.Location = new Point(0, 78);
            radioButtonBitRateHighFidelity.Name = "radioButtonBitRateHighFidelity";
            radioButtonBitRateHighFidelity.Size = new Size(144, 20);
            radioButtonBitRateHighFidelity.TabIndex = 5;
            radioButtonBitRateHighFidelity.Values.Text = "High-fidelity (96 kbps)";
            // 
            // radioButtonBitRateBalanced
            // 
            radioButtonBitRateBalanced.Location = new Point(0, 53);
            radioButtonBitRateBalanced.Name = "radioButtonBitRateBalanced";
            radioButtonBitRateBalanced.Size = new Size(125, 20);
            radioButtonBitRateBalanced.TabIndex = 4;
            radioButtonBitRateBalanced.Values.Text = "Balanced (64 kbps)";
            // 
            // radioButtonBitRateStandard
            // 
            radioButtonBitRateStandard.Checked = true;
            radioButtonBitRateStandard.Location = new Point(0, 28);
            radioButtonBitRateStandard.Name = "radioButtonBitRateStandard";
            radioButtonBitRateStandard.Size = new Size(125, 20);
            radioButtonBitRateStandard.TabIndex = 3;
            radioButtonBitRateStandard.Values.Text = "Standard (32 kbps)";
            // 
            // radioButtonBitRateLow
            // 
            radioButtonBitRateLow.Location = new Point(0, 3);
            radioButtonBitRateLow.Name = "radioButtonBitRateLow";
            radioButtonBitRateLow.Size = new Size(98, 20);
            radioButtonBitRateLow.TabIndex = 2;
            radioButtonBitRateLow.Values.Text = "Low (16 kbps)";
            // 
            // buttonOk
            // 
            buttonOk.Location = new Point(309, 226);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(75, 23);
            buttonOk.TabIndex = 10;
            buttonOk.Values.DropDownArrowColor = Color.Empty;
            buttonOk.Values.Text = "Ok";
            buttonOk.Click += ButtonOk_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(390, 226);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 11;
            buttonCancel.Values.DropDownArrowColor = Color.Empty;
            buttonCancel.Values.Text = "Cancel";
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // volumeMeterInput
            // 
            volumeMeterInput.Amplitude = 0F;
            volumeMeterInput.ForeColor = Color.SpringGreen;
            volumeMeterInput.Location = new Point(171, 144);
            volumeMeterInput.MaxDb = 18F;
            volumeMeterInput.MinDb = -60F;
            volumeMeterInput.Name = "volumeMeterInput";
            volumeMeterInput.Size = new Size(13, 105);
            volumeMeterInput.TabIndex = 9;
            volumeMeterInput.Text = "volumeMeterInput";
            // 
            // kryptonPanel1
            // 
            kryptonPanel1.Controls.Add(radioButtonBitRateLow);
            kryptonPanel1.Controls.Add(radioButtonBitRateHighFidelity);
            kryptonPanel1.Controls.Add(radioButtonBitRateStandard);
            kryptonPanel1.Controls.Add(radioButtonBitRateBalanced);
            kryptonPanel1.Location = new Point(12, 144);
            kryptonPanel1.Name = "kryptonPanel1";
            kryptonPanel1.Size = new Size(153, 105);
            kryptonPanel1.TabIndex = 13;
            // 
            // kryptonLabel1
            // 
            kryptonLabel1.Location = new Point(12, 123);
            kryptonLabel1.Name = "kryptonLabel1";
            kryptonLabel1.Size = new Size(128, 20);
            kryptonLabel1.TabIndex = 14;
            kryptonLabel1.Values.Text = "Quality (Sample Rate)";
            // 
            // FormVoicePreCall
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(477, 258);
            Controls.Add(kryptonPanel1);
            Controls.Add(volumeMeterInput);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOk);
            Controls.Add(comboBoxAudioInputDevice);
            Controls.Add(comboBoxAudioOutputDevice);
            Controls.Add(labelAudioInputDevice);
            Controls.Add(labelAudioOutputDevice);
            Controls.Add(kryptonLabel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormVoicePreCall";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Secure Chat";
            Load += FormVoicePreCall_Load;
            ((System.ComponentModel.ISupportInitialize)comboBoxAudioOutputDevice).EndInit();
            ((System.ComponentModel.ISupportInitialize)comboBoxAudioInputDevice).EndInit();
            ((System.ComponentModel.ISupportInitialize)kryptonPanel1).EndInit();
            kryptonPanel1.ResumeLayout(false);
            kryptonPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private KryptonComboBox comboBoxAudioOutputDevice;
        private KryptonLabel labelAudioOutputDevice;
        private KryptonLabel labelAudioInputDevice;
        private KryptonComboBox comboBoxAudioInputDevice;
        private KryptonRadioButton radioButtonBitRateHighFidelity;
        private KryptonRadioButton radioButtonBitRateBalanced;
        private KryptonRadioButton radioButtonBitRateStandard;
        private KryptonRadioButton radioButtonBitRateLow;
        private KryptonButton buttonOk;
        private KryptonButton buttonCancel;
        private NAudio.Gui.VolumeMeter volumeMeterInput;
        private KryptonPanel kryptonPanel1;
        private KryptonLabel kryptonLabel1;
    }
}