using CSCore.CoreAudioAPI;
using SecureChat.Client.Audio;

namespace SecureChat.Client.Forms
{
    public partial class FormVoicePreCall : Form
    {
        private string? _selectedInputDeviceId = null;
        private string? _selectedIOutputDeviceId = null;
        private AudioPump? _audioPump = null;
        private int _bitRate = 22050;

        public FormVoicePreCall()
        {
            InitializeComponent();
        }

        private void FormVoicePreCall_Load(object sender, EventArgs e)
        {
            comboBoxAudioInputDevice.SelectedIndexChanged += ComboBoxAudioInputDevice_SelectedIndexChanged;
            comboBoxAudioOutputDevice.SelectedIndexChanged += ComboBoxAudioOutputDevice_SelectedIndexChanged;

            using var deviceEnumerator = new MMDeviceEnumerator();

            var inputDevices = deviceEnumerator.EnumAudioEndpoints(DataFlow.Capture, DeviceState.Active);
            foreach(var inputDevice in inputDevices) 
            {
                comboBoxAudioInputDevice.Items.Add(new AudioDeviceComboItem(inputDevice.FriendlyName, inputDevice.DeviceID));
            }

            var outputDevices = deviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active);
            foreach (var outputDevice in outputDevices)
            {
                comboBoxAudioOutputDevice.Items.Add(new AudioDeviceComboItem(outputDevice.FriendlyName, outputDevice.DeviceID));
            }

            FormClosing += (sender, e) =>
            {
                //_audioPump?.Stop();
                _audioPump = null;
            };

            //volumeSliderVolume.VolumeChanged += VolumeSliderVolume_VolumeChanged;
            //volumeSliderVolume.Volume = 0.025f;

            radioButtonBitRate8000.CheckedChanged += RadioButtonBitRate_CheckedChanged;
            radioButtonBitRate11025.CheckedChanged += RadioButtonBitRate_CheckedChanged;
            radioButtonBitRate22050.CheckedChanged += RadioButtonBitRate_CheckedChanged;
            radioButtonBitRate32000.CheckedChanged += RadioButtonBitRate_CheckedChanged;
            radioButtonBitRate44100.CheckedChanged += RadioButtonBitRate_CheckedChanged;
            SetSelectedBitrate(_bitRate);
        }

        private void RadioButtonBitRate_CheckedChanged(object? sender, EventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.Checked)
            {
                _bitRate = GetSelectedBitrate();
                PropUpAudio();
            }
        }

        private int GetSelectedBitrate()
        {
            if (radioButtonBitRate8000.Checked)
                return 8000;
            else if (radioButtonBitRate11025.Checked)
                return 11025;
            else if (radioButtonBitRate22050.Checked)
                return 22050;
            else if (radioButtonBitRate32000.Checked)
                return 32000;
            else if (radioButtonBitRate44100.Checked)
                return 44100;

            return 44100;
        }

        private void SetSelectedBitrate(int bitrate)
        {
            switch (bitrate)
            {
                case 8000:
                    radioButtonBitRate8000.Checked = true;
                    break;
                case 11025:
                    radioButtonBitRate11025.Checked = true;
                    break;
                case 22050:
                    radioButtonBitRate22050.Checked = true;
                    break;
                case 32000:
                    radioButtonBitRate32000.Checked = true;
                    break;
                case 44100:
                    radioButtonBitRate44100.Checked = true;
                    break;
            }
        }

        private void VolumeSliderVolume_VolumeChanged(object? sender, EventArgs e)
        {
            if (_audioPump != null)
            {
                //_audioPump.Volume = volumeSliderVolume.Volume;
            }
        }

        private void ComboBoxAudioInputDevice_SelectedIndexChanged(object? sender, EventArgs e)
        {
            _selectedInputDeviceId = (comboBoxAudioInputDevice.SelectedItem as AudioDeviceComboItem)?.DeviceId;
            PropUpAudio();
        }

        private void ComboBoxAudioOutputDevice_SelectedIndexChanged(object? sender, EventArgs e)
        {
            _selectedIOutputDeviceId = (comboBoxAudioOutputDevice.SelectedItem as AudioDeviceComboItem)?.DeviceId;
            PropUpAudio();
        }

        private void PropUpAudio()
        {
            if (_selectedInputDeviceId != null && _selectedIOutputDeviceId != null)
            {
                //_audioPump?.Stop();
                _audioPump = null;

                _audioPump = new AudioPump(_selectedInputDeviceId, _selectedIOutputDeviceId);
                //_audioPump.Volume = volumeSliderVolume.Volume;
                //_audioPump.OnAmplitudeReport += (level) =>
                {
                    BeginInvoke(new Action(() =>
                    {
                        //volumeMeter.Amplitude = level * 100;
                    }));
                };

                _audioPump.Start();
            }
        }
    }
}
