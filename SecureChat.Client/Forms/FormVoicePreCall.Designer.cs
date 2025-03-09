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
            comboBoxAudioOutputDevice = new ComboBox();
            labelAudioOutputDevice = new Label();
            labelAudioInputDevice = new Label();
            comboBoxAudioInputDevice = new ComboBox();
            groupBoxSampleRate = new GroupBox();
            radioButtonBitRateHighFidelity = new RadioButton();
            radioButtonBitRateBalanced = new RadioButton();
            radioButtonBitRateStandard = new RadioButton();
            radioButtonBitRateLow = new RadioButton();
            buttonOk = new Button();
            buttonCancel = new Button();
            volumeMeterInput = new NAudio.Gui.VolumeMeter();
            groupBoxSampleRate.SuspendLayout();
            SuspendLayout();
            // 
            // comboBoxAudioOutputDevice
            // 
            comboBoxAudioOutputDevice.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxAudioOutputDevice.FormattingEnabled = true;
            comboBoxAudioOutputDevice.Location = new Point(12, 86);
            comboBoxAudioOutputDevice.Name = "comboBoxAudioOutputDevice";
            comboBoxAudioOutputDevice.Size = new Size(457, 23);
            comboBoxAudioOutputDevice.TabIndex = 1;
            // 
            // labelAudioOutputDevice
            // 
            labelAudioOutputDevice.AutoSize = true;
            labelAudioOutputDevice.Location = new Point(12, 68);
            labelAudioOutputDevice.Name = "labelAudioOutputDevice";
            labelAudioOutputDevice.Size = new Size(115, 15);
            labelAudioOutputDevice.TabIndex = 1;
            labelAudioOutputDevice.Text = "Audio output device";
            // 
            // labelAudioInputDevice
            // 
            labelAudioInputDevice.AutoSize = true;
            labelAudioInputDevice.Location = new Point(12, 20);
            labelAudioInputDevice.Name = "labelAudioInputDevice";
            labelAudioInputDevice.Size = new Size(107, 15);
            labelAudioInputDevice.TabIndex = 3;
            labelAudioInputDevice.Text = "Audio input device";
            // 
            // comboBoxAudioInputDevice
            // 
            comboBoxAudioInputDevice.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxAudioInputDevice.FormattingEnabled = true;
            comboBoxAudioInputDevice.Location = new Point(12, 38);
            comboBoxAudioInputDevice.Name = "comboBoxAudioInputDevice";
            comboBoxAudioInputDevice.Size = new Size(457, 23);
            comboBoxAudioInputDevice.TabIndex = 0;
            // 
            // groupBoxSampleRate
            // 
            groupBoxSampleRate.Controls.Add(radioButtonBitRateHighFidelity);
            groupBoxSampleRate.Controls.Add(radioButtonBitRateBalanced);
            groupBoxSampleRate.Controls.Add(radioButtonBitRateStandard);
            groupBoxSampleRate.Controls.Add(radioButtonBitRateLow);
            groupBoxSampleRate.Location = new Point(14, 115);
            groupBoxSampleRate.Name = "groupBoxSampleRate";
            groupBoxSampleRate.Size = new Size(222, 124);
            groupBoxSampleRate.TabIndex = 7;
            groupBoxSampleRate.TabStop = false;
            groupBoxSampleRate.Text = "Quality (sample rate)";
            // 
            // radioButtonBitRateHighFidelity
            // 
            radioButtonBitRateHighFidelity.AutoSize = true;
            radioButtonBitRateHighFidelity.Location = new Point(11, 95);
            radioButtonBitRateHighFidelity.Name = "radioButtonBitRateHighFidelity";
            radioButtonBitRateHighFidelity.Size = new Size(143, 19);
            radioButtonBitRateHighFidelity.TabIndex = 5;
            radioButtonBitRateHighFidelity.Text = "High-fidelity (96 kbps)";
            radioButtonBitRateHighFidelity.UseVisualStyleBackColor = true;
            // 
            // radioButtonBitRateBalanced
            // 
            radioButtonBitRateBalanced.AutoSize = true;
            radioButtonBitRateBalanced.Location = new Point(11, 70);
            radioButtonBitRateBalanced.Name = "radioButtonBitRateBalanced";
            radioButtonBitRateBalanced.Size = new Size(124, 19);
            radioButtonBitRateBalanced.TabIndex = 4;
            radioButtonBitRateBalanced.Text = "Balanced (64 kbps)";
            radioButtonBitRateBalanced.UseVisualStyleBackColor = true;
            // 
            // radioButtonBitRateStandard
            // 
            radioButtonBitRateStandard.AutoSize = true;
            radioButtonBitRateStandard.Checked = true;
            radioButtonBitRateStandard.Location = new Point(11, 45);
            radioButtonBitRateStandard.Name = "radioButtonBitRateStandard";
            radioButtonBitRateStandard.Size = new Size(123, 19);
            radioButtonBitRateStandard.TabIndex = 3;
            radioButtonBitRateStandard.TabStop = true;
            radioButtonBitRateStandard.Text = "Standard (32 kbps)";
            radioButtonBitRateStandard.UseVisualStyleBackColor = true;
            // 
            // radioButtonBitRateLow
            // 
            radioButtonBitRateLow.AutoSize = true;
            radioButtonBitRateLow.Location = new Point(11, 20);
            radioButtonBitRateLow.Name = "radioButtonBitRateLow";
            radioButtonBitRateLow.Size = new Size(98, 19);
            radioButtonBitRateLow.TabIndex = 2;
            radioButtonBitRateLow.Text = "Low (16 kbps)";
            radioButtonBitRateLow.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            buttonOk.Location = new Point(313, 216);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(75, 23);
            buttonOk.TabIndex = 10;
            buttonOk.Text = "Ok";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += ButtonOk_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(394, 216);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 11;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // volumeMeterInput
            // 
            volumeMeterInput.Amplitude = 0F;
            volumeMeterInput.ForeColor = Color.SpringGreen;
            volumeMeterInput.Location = new Point(242, 123);
            volumeMeterInput.MaxDb = 18F;
            volumeMeterInput.MinDb = -60F;
            volumeMeterInput.Name = "volumeMeterInput";
            volumeMeterInput.Size = new Size(10, 115);
            volumeMeterInput.TabIndex = 9;
            volumeMeterInput.Text = "volumeMeterInput";
            // 
            // FormVoicePreCall
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(481, 249);
            Controls.Add(volumeMeterInput);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOk);
            Controls.Add(groupBoxSampleRate);
            Controls.Add(labelAudioInputDevice);
            Controls.Add(comboBoxAudioInputDevice);
            Controls.Add(labelAudioOutputDevice);
            Controls.Add(comboBoxAudioOutputDevice);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormVoicePreCall";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Secure Chat";
            Load += FormVoicePreCall_Load;
            groupBoxSampleRate.ResumeLayout(false);
            groupBoxSampleRate.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboBoxAudioOutputDevice;
        private Label labelAudioOutputDevice;
        private Label labelAudioInputDevice;
        private ComboBox comboBoxAudioInputDevice;
        private GroupBox groupBoxSampleRate;
        private RadioButton radioButtonBitRateHighFidelity;
        private RadioButton radioButtonBitRateBalanced;
        private RadioButton radioButtonBitRateStandard;
        private RadioButton radioButtonBitRateLow;
        private Button buttonOk;
        private Button buttonCancel;
        private NAudio.Gui.VolumeMeter volumeMeterInput;
    }
}