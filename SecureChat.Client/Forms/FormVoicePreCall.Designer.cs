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
            radioButtonSampleRate48000 = new RadioButton();
            radioButtonSampleRate24000 = new RadioButton();
            radioButtonSampleRate16000 = new RadioButton();
            radioButtonSampleRate12000 = new RadioButton();
            radioButtonSampleRate8000 = new RadioButton();
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
            groupBoxSampleRate.Controls.Add(radioButtonSampleRate48000);
            groupBoxSampleRate.Controls.Add(radioButtonSampleRate24000);
            groupBoxSampleRate.Controls.Add(radioButtonSampleRate16000);
            groupBoxSampleRate.Controls.Add(radioButtonSampleRate12000);
            groupBoxSampleRate.Controls.Add(radioButtonSampleRate8000);
            groupBoxSampleRate.Location = new Point(14, 115);
            groupBoxSampleRate.Name = "groupBoxSampleRate";
            groupBoxSampleRate.Size = new Size(222, 152);
            groupBoxSampleRate.TabIndex = 7;
            groupBoxSampleRate.TabStop = false;
            groupBoxSampleRate.Text = "Quality (sample rate)";
            // 
            // radioButtonSampleRate48000
            // 
            radioButtonSampleRate48000.AutoSize = true;
            radioButtonSampleRate48000.Location = new Point(11, 120);
            radioButtonSampleRate48000.Name = "radioButtonSampleRate48000";
            radioButtonSampleRate48000.Size = new Size(122, 19);
            radioButtonSampleRate48000.TabIndex = 6;
            radioButtonSampleRate48000.TabStop = true;
            radioButtonSampleRate48000.Text = "CD quality (48Khz)";
            radioButtonSampleRate48000.UseVisualStyleBackColor = true;
            // 
            // radioButtonSampleRate24000
            // 
            radioButtonSampleRate24000.AutoSize = true;
            radioButtonSampleRate24000.Location = new Point(11, 95);
            radioButtonSampleRate24000.Name = "radioButtonSampleRate24000";
            radioButtonSampleRate24000.Size = new Size(114, 19);
            radioButtonSampleRate24000.TabIndex = 5;
            radioButtonSampleRate24000.TabStop = true;
            radioButtonSampleRate24000.Text = "FM radio (24Khz)";
            radioButtonSampleRate24000.UseVisualStyleBackColor = true;
            // 
            // radioButtonSampleRate16000
            // 
            radioButtonSampleRate16000.AutoSize = true;
            radioButtonSampleRate16000.Location = new Point(11, 70);
            radioButtonSampleRate16000.Name = "radioButtonSampleRate16000";
            radioButtonSampleRate16000.Size = new Size(123, 19);
            radioButtonSampleRate16000.TabIndex = 4;
            radioButtonSampleRate16000.TabStop = true;
            radioButtonSampleRate16000.Text = "Mid-range (16Khz)";
            radioButtonSampleRate16000.UseVisualStyleBackColor = true;
            // 
            // radioButtonSampleRate12000
            // 
            radioButtonSampleRate12000.AutoSize = true;
            radioButtonSampleRate12000.Location = new Point(11, 45);
            radioButtonSampleRate12000.Name = "radioButtonSampleRate12000";
            radioButtonSampleRate12000.Size = new Size(170, 19);
            radioButtonSampleRate12000.TabIndex = 3;
            radioButtonSampleRate12000.TabStop = true;
            radioButtonSampleRate12000.Text = "Low-quality speech (12Khz)";
            radioButtonSampleRate12000.UseVisualStyleBackColor = true;
            // 
            // radioButtonSampleRate8000
            // 
            radioButtonSampleRate8000.AutoSize = true;
            radioButtonSampleRate8000.Location = new Point(11, 20);
            radioButtonSampleRate8000.Name = "radioButtonSampleRate8000";
            radioButtonSampleRate8000.Size = new Size(155, 19);
            radioButtonSampleRate8000.TabIndex = 2;
            radioButtonSampleRate8000.TabStop = true;
            radioButtonSampleRate8000.Text = "Telephone quality (8Khz)";
            radioButtonSampleRate8000.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            buttonOk.Location = new Point(313, 244);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(75, 23);
            buttonOk.TabIndex = 10;
            buttonOk.Text = "Ok";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += ButtonOk_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(394, 244);
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
            ClientSize = new Size(481, 277);
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
        private RadioButton radioButtonSampleRate48000;
        private RadioButton radioButtonSampleRate24000;
        private RadioButton radioButtonSampleRate16000;
        private RadioButton radioButtonSampleRate12000;
        private RadioButton radioButtonSampleRate8000;
        private Button buttonOk;
        private Button buttonCancel;
        private NAudio.Gui.VolumeMeter volumeMeterInput;
        private TrackBar trackBarGain;
    }
}