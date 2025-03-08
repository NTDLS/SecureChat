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
            radioButtonSampleRateNative = new RadioButton();
            radioButtonSampleRate44100 = new RadioButton();
            radioButtonSampleRate32000 = new RadioButton();
            radioButtonSampleRate22050 = new RadioButton();
            radioButtonSampleRate11025 = new RadioButton();
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
            groupBoxSampleRate.Controls.Add(radioButtonSampleRateNative);
            groupBoxSampleRate.Controls.Add(radioButtonSampleRate44100);
            groupBoxSampleRate.Controls.Add(radioButtonSampleRate32000);
            groupBoxSampleRate.Controls.Add(radioButtonSampleRate22050);
            groupBoxSampleRate.Controls.Add(radioButtonSampleRate11025);
            groupBoxSampleRate.Controls.Add(radioButtonSampleRate8000);
            groupBoxSampleRate.Location = new Point(14, 115);
            groupBoxSampleRate.Name = "groupBoxSampleRate";
            groupBoxSampleRate.Size = new Size(222, 174);
            groupBoxSampleRate.TabIndex = 7;
            groupBoxSampleRate.TabStop = false;
            groupBoxSampleRate.Text = "Quality (sample rate)";
            // 
            // radioButtonSampleRateNative
            // 
            radioButtonSampleRateNative.AutoSize = true;
            radioButtonSampleRateNative.Location = new Point(11, 145);
            radioButtonSampleRateNative.Name = "radioButtonSampleRateNative";
            radioButtonSampleRateNative.Size = new Size(159, 19);
            radioButtonSampleRateNative.TabIndex = 7;
            radioButtonSampleRateNative.TabStop = true;
            radioButtonSampleRateNative.Text = "Device native sample rate";
            radioButtonSampleRateNative.UseVisualStyleBackColor = true;
            // 
            // radioButtonSampleRate44100
            // 
            radioButtonSampleRate44100.AutoSize = true;
            radioButtonSampleRate44100.Location = new Point(11, 120);
            radioButtonSampleRate44100.Name = "radioButtonSampleRate44100";
            radioButtonSampleRate44100.Size = new Size(121, 19);
            radioButtonSampleRate44100.TabIndex = 6;
            radioButtonSampleRate44100.TabStop = true;
            radioButtonSampleRate44100.Text = "CD quality (44100)";
            radioButtonSampleRate44100.UseVisualStyleBackColor = true;
            // 
            // radioButtonSampleRate32000
            // 
            radioButtonSampleRate32000.AutoSize = true;
            radioButtonSampleRate32000.Location = new Point(11, 95);
            radioButtonSampleRate32000.Name = "radioButtonSampleRate32000";
            radioButtonSampleRate32000.Size = new Size(113, 19);
            radioButtonSampleRate32000.TabIndex = 5;
            radioButtonSampleRate32000.TabStop = true;
            radioButtonSampleRate32000.Text = "FM radio (32000)";
            radioButtonSampleRate32000.UseVisualStyleBackColor = true;
            // 
            // radioButtonSampleRate22050
            // 
            radioButtonSampleRate22050.AutoSize = true;
            radioButtonSampleRate22050.Location = new Point(11, 70);
            radioButtonSampleRate22050.Name = "radioButtonSampleRate22050";
            radioButtonSampleRate22050.Size = new Size(122, 19);
            radioButtonSampleRate22050.TabIndex = 4;
            radioButtonSampleRate22050.TabStop = true;
            radioButtonSampleRate22050.Text = "Mid-range (22050)";
            radioButtonSampleRate22050.UseVisualStyleBackColor = true;
            // 
            // radioButtonSampleRate11025
            // 
            radioButtonSampleRate11025.AutoSize = true;
            radioButtonSampleRate11025.Location = new Point(11, 45);
            radioButtonSampleRate11025.Name = "radioButtonSampleRate11025";
            radioButtonSampleRate11025.Size = new Size(169, 19);
            radioButtonSampleRate11025.TabIndex = 3;
            radioButtonSampleRate11025.TabStop = true;
            radioButtonSampleRate11025.Text = "Low-quality speech (11025)";
            radioButtonSampleRate11025.UseVisualStyleBackColor = true;
            // 
            // radioButtonSampleRate8000
            // 
            radioButtonSampleRate8000.AutoSize = true;
            radioButtonSampleRate8000.Location = new Point(11, 20);
            radioButtonSampleRate8000.Name = "radioButtonSampleRate8000";
            radioButtonSampleRate8000.Size = new Size(154, 19);
            radioButtonSampleRate8000.TabIndex = 2;
            radioButtonSampleRate8000.TabStop = true;
            radioButtonSampleRate8000.Text = "Telephone quality (8000)";
            radioButtonSampleRate8000.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            buttonOk.Location = new Point(313, 266);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(75, 23);
            buttonOk.TabIndex = 10;
            buttonOk.Text = "Ok";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += ButtonOk_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(394, 266);
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
            ClientSize = new Size(481, 299);
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
        private RadioButton radioButtonSampleRate44100;
        private RadioButton radioButtonSampleRate32000;
        private RadioButton radioButtonSampleRate22050;
        private RadioButton radioButtonSampleRate11025;
        private RadioButton radioButtonSampleRate8000;
        private Button buttonOk;
        private Button buttonCancel;
        private RadioButton radioButtonSampleRateNative;
        private NAudio.Gui.VolumeMeter volumeMeterInput;
        private TrackBar trackBarGain;
    }
}