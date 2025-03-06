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
            groupBoxBitrate = new GroupBox();
            radioButtonBitRate44100 = new RadioButton();
            radioButtonBitRate32000 = new RadioButton();
            radioButtonBitRate22050 = new RadioButton();
            radioButtonBitRate11025 = new RadioButton();
            radioButtonBitRate8000 = new RadioButton();
            buttonOk = new Button();
            buttonCancel = new Button();
            trackBarGain = new TrackBar();
            progressBarVolume = new ProgressBar();
            groupBoxBitrate.SuspendLayout();
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
            comboBoxAudioOutputDevice.TabIndex = 0;
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
            comboBoxAudioInputDevice.TabIndex = 2;
            // 
            // labelMicrophoneGain
            // 
            labelMicrophoneGain.AutoSize = true;
            labelMicrophoneGain.Location = new Point(240, 112);
            labelMicrophoneGain.Name = "labelMicrophoneGain";
            labelMicrophoneGain.Size = new Size(98, 15);
            labelMicrophoneGain.TabIndex = 6;
            labelMicrophoneGain.Text = "Microphone gain";
            // 
            // groupBoxBitrate
            // 
            groupBoxBitrate.Controls.Add(radioButtonBitRate44100);
            groupBoxBitrate.Controls.Add(radioButtonBitRate32000);
            groupBoxBitrate.Controls.Add(radioButtonBitRate22050);
            groupBoxBitrate.Controls.Add(radioButtonBitRate11025);
            groupBoxBitrate.Controls.Add(radioButtonBitRate8000);
            groupBoxBitrate.Location = new Point(14, 115);
            groupBoxBitrate.Name = "groupBoxBitrate";
            groupBoxBitrate.Size = new Size(222, 148);
            groupBoxBitrate.TabIndex = 7;
            groupBoxBitrate.TabStop = false;
            groupBoxBitrate.Text = "Quality";
            // 
            // radioButtonBitRate44100
            // 
            radioButtonBitRate44100.AutoSize = true;
            radioButtonBitRate44100.Location = new Point(11, 120);
            radioButtonBitRate44100.Name = "radioButtonBitRate44100";
            radioButtonBitRate44100.Size = new Size(121, 19);
            radioButtonBitRate44100.TabIndex = 4;
            radioButtonBitRate44100.TabStop = true;
            radioButtonBitRate44100.Text = "CD quality (44100)";
            radioButtonBitRate44100.UseVisualStyleBackColor = true;
            // 
            // radioButtonBitRate32000
            // 
            radioButtonBitRate32000.AutoSize = true;
            radioButtonBitRate32000.Location = new Point(11, 95);
            radioButtonBitRate32000.Name = "radioButtonBitRate32000";
            radioButtonBitRate32000.Size = new Size(113, 19);
            radioButtonBitRate32000.TabIndex = 3;
            radioButtonBitRate32000.TabStop = true;
            radioButtonBitRate32000.Text = "FM radio (32000)";
            radioButtonBitRate32000.UseVisualStyleBackColor = true;
            // 
            // radioButtonBitRate22050
            // 
            radioButtonBitRate22050.AutoSize = true;
            radioButtonBitRate22050.Location = new Point(11, 70);
            radioButtonBitRate22050.Name = "radioButtonBitRate22050";
            radioButtonBitRate22050.Size = new Size(122, 19);
            radioButtonBitRate22050.TabIndex = 2;
            radioButtonBitRate22050.TabStop = true;
            radioButtonBitRate22050.Text = "Mid-range (22050)";
            radioButtonBitRate22050.UseVisualStyleBackColor = true;
            // 
            // radioButtonBitRate11025
            // 
            radioButtonBitRate11025.AutoSize = true;
            radioButtonBitRate11025.Location = new Point(11, 45);
            radioButtonBitRate11025.Name = "radioButtonBitRate11025";
            radioButtonBitRate11025.Size = new Size(169, 19);
            radioButtonBitRate11025.TabIndex = 1;
            radioButtonBitRate11025.TabStop = true;
            radioButtonBitRate11025.Text = "Low-quality speech (11025)";
            radioButtonBitRate11025.UseVisualStyleBackColor = true;
            // 
            // radioButtonBitRate8000
            // 
            radioButtonBitRate8000.AutoSize = true;
            radioButtonBitRate8000.Location = new Point(11, 20);
            radioButtonBitRate8000.Name = "radioButtonBitRate8000";
            radioButtonBitRate8000.Size = new Size(154, 19);
            radioButtonBitRate8000.TabIndex = 0;
            radioButtonBitRate8000.TabStop = true;
            radioButtonBitRate8000.Text = "Telephone quality (8000)";
            radioButtonBitRate8000.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            buttonOk.Location = new Point(313, 240);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(75, 23);
            buttonOk.TabIndex = 8;
            buttonOk.Text = "Ok";
            buttonOk.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(394, 240);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 9;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            // 
            // trackBarGain
            // 
            trackBarGain.Location = new Point(242, 130);
            trackBarGain.Maximum = 20;
            trackBarGain.Minimum = 1;
            trackBarGain.Name = "trackBarGain";
            trackBarGain.Size = new Size(227, 45);
            trackBarGain.TabIndex = 10;
            trackBarGain.Value = 10;
            // 
            // progressBarVolume
            // 
            progressBarVolume.Location = new Point(254, 188);
            progressBarVolume.Maximum = 10;
            progressBarVolume.Name = "progressBarVolume";
            progressBarVolume.Size = new Size(215, 23);
            progressBarVolume.Step = 1;
            progressBarVolume.TabIndex = 11;
            // 
            // FormVoicePreCall
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(481, 274);
            Controls.Add(progressBarVolume);
            Controls.Add(trackBarGain);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOk);
            Controls.Add(groupBoxBitrate);
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
            groupBoxBitrate.ResumeLayout(false);
            groupBoxBitrate.PerformLayout();
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
        private GroupBox groupBoxBitrate;
        private RadioButton radioButtonBitRate44100;
        private RadioButton radioButtonBitRate32000;
        private RadioButton radioButtonBitRate22050;
        private RadioButton radioButtonBitRate11025;
        private RadioButton radioButtonBitRate8000;
        private Button buttonOk;
        private Button buttonCancel;
        private TrackBar trackBarGain;
        private ProgressBar progressBarVolume;
    }
}