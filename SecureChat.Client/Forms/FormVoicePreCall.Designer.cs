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
            labelMicrophoneGain = new Label();
            groupBoxSampleRate = new GroupBox();
            radioButtonBitRateExtraHighQuality = new RadioButton();
            radioButtonBitRateHighQuality = new RadioButton();
            radioButtonBitRateMediumQuality = new RadioButton();
            radioButtonBitRateLowQuality = new RadioButton();
            buttonOk = new Button();
            buttonCancel = new Button();
            volumeMeterInput = new NAudio.Gui.VolumeMeter();
            trackBarGain = new TrackBar();
            groupBoxSampleRate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarGain).BeginInit();
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
            // labelMicrophoneGain
            // 
            labelMicrophoneGain.AutoSize = true;
            labelMicrophoneGain.Location = new Point(248, 112);
            labelMicrophoneGain.Name = "labelMicrophoneGain";
            labelMicrophoneGain.Size = new Size(99, 15);
            labelMicrophoneGain.TabIndex = 6;
            labelMicrophoneGain.Text = "Microphone Gain";
            // 
            // groupBoxSampleRate
            // 
            groupBoxSampleRate.Controls.Add(radioButtonBitRateExtraHighQuality);
            groupBoxSampleRate.Controls.Add(radioButtonBitRateHighQuality);
            groupBoxSampleRate.Controls.Add(radioButtonBitRateMediumQuality);
            groupBoxSampleRate.Controls.Add(radioButtonBitRateLowQuality);
            groupBoxSampleRate.Location = new Point(14, 115);
            groupBoxSampleRate.Name = "groupBoxSampleRate";
            groupBoxSampleRate.Size = new Size(222, 124);
            groupBoxSampleRate.TabIndex = 7;
            groupBoxSampleRate.TabStop = false;
            groupBoxSampleRate.Text = "Quality (sample rate)";
            // 
            // radioButtonBitRateExtraHighQuality
            // 
            radioButtonBitRateExtraHighQuality.AutoSize = true;
            radioButtonBitRateExtraHighQuality.Location = new Point(11, 95);
            radioButtonBitRateExtraHighQuality.Name = "radioButtonBitRateExtraHighQuality";
            radioButtonBitRateExtraHighQuality.Size = new Size(169, 19);
            radioButtonBitRateExtraHighQuality.TabIndex = 5;
            radioButtonBitRateExtraHighQuality.TabStop = true;
            radioButtonBitRateExtraHighQuality.Text = "Extra-high quality (96 kbps)";
            radioButtonBitRateExtraHighQuality.UseVisualStyleBackColor = true;
            // 
            // radioButtonBitRateHighQuality
            // 
            radioButtonBitRateHighQuality.AutoSize = true;
            radioButtonBitRateHighQuality.Location = new Point(11, 70);
            radioButtonBitRateHighQuality.Name = "radioButtonBitRateHighQuality";
            radioButtonBitRateHighQuality.Size = new Size(141, 19);
            radioButtonBitRateHighQuality.TabIndex = 4;
            radioButtonBitRateHighQuality.TabStop = true;
            radioButtonBitRateHighQuality.Text = "High quality (64 kbps)";
            radioButtonBitRateHighQuality.UseVisualStyleBackColor = true;
            // 
            // radioButtonBitRateMediumQuality
            // 
            radioButtonBitRateMediumQuality.AutoSize = true;
            radioButtonBitRateMediumQuality.Location = new Point(11, 45);
            radioButtonBitRateMediumQuality.Name = "radioButtonBitRateMediumQuality";
            radioButtonBitRateMediumQuality.Size = new Size(160, 19);
            radioButtonBitRateMediumQuality.TabIndex = 3;
            radioButtonBitRateMediumQuality.TabStop = true;
            radioButtonBitRateMediumQuality.Text = "Medium quality (32 kbps)";
            radioButtonBitRateMediumQuality.UseVisualStyleBackColor = true;
            // 
            // radioButtonBitRateLowQuality
            // 
            radioButtonBitRateLowQuality.AutoSize = true;
            radioButtonBitRateLowQuality.Location = new Point(11, 20);
            radioButtonBitRateLowQuality.Name = "radioButtonBitRateLowQuality";
            radioButtonBitRateLowQuality.Size = new Size(137, 19);
            radioButtonBitRateLowQuality.TabIndex = 2;
            radioButtonBitRateLowQuality.TabStop = true;
            radioButtonBitRateLowQuality.Text = "Low quality (16 kbps)";
            radioButtonBitRateLowQuality.UseVisualStyleBackColor = true;
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
            volumeMeterInput.Location = new Point(248, 180);
            volumeMeterInput.MaxDb = 18F;
            volumeMeterInput.MinDb = -60F;
            volumeMeterInput.Name = "volumeMeterInput";
            volumeMeterInput.Orientation = Orientation.Horizontal;
            volumeMeterInput.Size = new Size(221, 24);
            volumeMeterInput.TabIndex = 9;
            volumeMeterInput.Text = "volumeMeterInput";
            // 
            // trackBarGain
            // 
            trackBarGain.Location = new Point(248, 130);
            trackBarGain.Maximum = 50;
            trackBarGain.Name = "trackBarGain";
            trackBarGain.Size = new Size(221, 45);
            trackBarGain.TabIndex = 8;
            // 
            // FormVoicePreCall
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(481, 249);
            Controls.Add(trackBarGain);
            Controls.Add(volumeMeterInput);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOk);
            Controls.Add(groupBoxSampleRate);
            Controls.Add(labelMicrophoneGain);
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
            ((System.ComponentModel.ISupportInitialize)trackBarGain).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboBoxAudioOutputDevice;
        private Label labelAudioOutputDevice;
        private Label labelAudioInputDevice;
        private ComboBox comboBoxAudioInputDevice;
        private Label labelMicrophoneGain;
        private GroupBox groupBoxSampleRate;
        private RadioButton radioButtonBitRateExtraHighQuality;
        private RadioButton radioButtonBitRateHighQuality;
        private RadioButton radioButtonBitRateMediumQuality;
        private RadioButton radioButtonBitRateLowQuality;
        private Button buttonOk;
        private Button buttonCancel;
        private NAudio.Gui.VolumeMeter volumeMeterInput;
        private TrackBar trackBarGain;
    }
}