using NAudio.CoreAudioApi;
using NAudio.Wave;
using SecureChat.Client.Audio;

namespace SecureChat.Client.Forms
{
    public partial class FormVoicePreCall : Form
    {
        private int? _selectedInputDeviceIndex = null;
        private int? _selectedIOutputDeviceIndex = null;
        private AudioPump? _audioPump = null;
        private int? _sampleRate = null;

        public FormVoicePreCall()
        {
            InitializeComponent();
        }

        private void FormVoicePreCall_Load(object sender, EventArgs e)
        {
            comboBoxAudioInputDevice.SelectedIndexChanged += ComboBoxAudioInputDevice_SelectedIndexChanged;
            comboBoxAudioOutputDevice.SelectedIndexChanged += ComboBoxAudioOutputDevice_SelectedIndexChanged;

            var enumerator = new MMDeviceEnumerator();

            var inputDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToList();
            for (int device = 0; device < WaveInEvent.DeviceCount; device++)
            {
                var capabilities = WaveInEvent.GetCapabilities(device);
                var mmDevice = inputDevices.FirstOrDefault(o => o.FriendlyName.StartsWith(capabilities.ProductName));
                if (mmDevice != null)
                {
                    comboBoxAudioInputDevice.Items.Add(new AudioDeviceComboItem(mmDevice.FriendlyName, device));
                }
            }

            var outputDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            for (int device = 0; device < outputDevices.Count; device++)
            {
                comboBoxAudioOutputDevice.Items.Add(new AudioDeviceComboItem(outputDevices[device].FriendlyName, device));
            }

            FormClosing += (sender, e) =>
            {
                _audioPump?.Stop();
                _audioPump = null;
            };

            trackBarGain.ValueChanged += TrackBarGain_ValueChanged;
            trackBarGain.Value = 25;

            radioButtonSampleRate8000.CheckedChanged += RadioButtonBitRate_CheckedChanged;
            radioButtonSampleRate11025.CheckedChanged += RadioButtonBitRate_CheckedChanged;
            radioButtonSampleRate22050.CheckedChanged += RadioButtonBitRate_CheckedChanged;
            radioButtonSampleRate32000.CheckedChanged += RadioButtonBitRate_CheckedChanged;
            radioButtonSampleRate44100.CheckedChanged += RadioButtonBitRate_CheckedChanged;
            radioButtonSampleRateNative.CheckedChanged += RadioButtonBitRate_CheckedChanged;

            SetSelectedSampleRate(22050);
        }

        private void TrackBarGain_ValueChanged(object? sender, EventArgs e)
        {
            if (_audioPump != null)
            {
                _audioPump.Gain = ((float)trackBarGain.Value) / 10.0f;
            }
        }

        private void RadioButtonBitRate_CheckedChanged(object? sender, EventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.Checked)
            {
                _sampleRate = GetSelectedSampleRate();
                PropUpAudio();
            }
        }

        private int? GetSelectedSampleRate()
        {
            if (radioButtonSampleRate8000.Checked)
                return 8000;
            else if (radioButtonSampleRate11025.Checked)
                return 11025;
            else if (radioButtonSampleRate22050.Checked)
                return 22050;
            else if (radioButtonSampleRate32000.Checked)
                return 32000;
            else if (radioButtonSampleRate44100.Checked)
                return 44100;
            else if (radioButtonSampleRateNative.Checked)
                return null;

            return 44100;
        }

        private void SetSelectedSampleRate(int? sampleRate)
        {
            switch (sampleRate)
            {
                case 8000:
                    radioButtonSampleRate8000.Checked = true;
                    break;
                case 11025:
                    radioButtonSampleRate11025.Checked = true;
                    break;
                case 22050:
                    radioButtonSampleRate22050.Checked = true;
                    break;
                case 32000:
                    radioButtonSampleRate32000.Checked = true;
                    break;
                case 44100:
                    radioButtonSampleRate44100.Checked = true;
                    break;
                case null:
                    radioButtonSampleRateNative.Checked = true;
                    break;
            }
        }

        private void ComboBoxAudioInputDevice_SelectedIndexChanged(object? sender, EventArgs e)
        {
            _selectedInputDeviceIndex = (comboBoxAudioInputDevice.SelectedItem as AudioDeviceComboItem)?.DeviceIndex;
            PropUpAudio();
        }

        private void ComboBoxAudioOutputDevice_SelectedIndexChanged(object? sender, EventArgs e)
        {
            _selectedIOutputDeviceIndex = (comboBoxAudioOutputDevice.SelectedItem as AudioDeviceComboItem)?.DeviceIndex;
            PropUpAudio();
        }

        private void PropUpAudio()
        {
            if (_selectedInputDeviceIndex != null && _selectedIOutputDeviceIndex != null)
            {
                _audioPump?.Stop();
                _audioPump = null;

                _audioPump = new AudioPump(_selectedInputDeviceIndex.Value, _selectedIOutputDeviceIndex.Value, _sampleRate);
                _audioPump.Gain = ((float)trackBarGain.Value) / 10.0f;

                _audioPump.OnInputSample += (volume) =>
                {
                    BeginInvoke(new Action(() =>
                    {
                        volumeMeterInput.Amplitude = volume;
                    }));
                };

                _audioPump.Start();
            }
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {

        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
