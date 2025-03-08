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
        private int _sampleRate = 16000;

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
            radioButtonSampleRate12000.CheckedChanged += RadioButtonBitRate_CheckedChanged;
            radioButtonSampleRate16000.CheckedChanged += RadioButtonBitRate_CheckedChanged;
            radioButtonSampleRate24000.CheckedChanged += RadioButtonBitRate_CheckedChanged;
            radioButtonSampleRate48000.CheckedChanged += RadioButtonBitRate_CheckedChanged;

            SetSelectedSampleRate(16000);
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

        private int GetSelectedSampleRate()
        {            //be 8 / 12 / 16 / 24 / 48 Khz

            if (radioButtonSampleRate8000.Checked)
                return 8000;
            else if (radioButtonSampleRate12000.Checked)
                return 12000;
            else if (radioButtonSampleRate16000.Checked)
                return 16000;
            else if (radioButtonSampleRate24000.Checked)
                return 24000;
            else if (radioButtonSampleRate48000.Checked)
                return 48000;

            return 48000;
        }

        private void SetSelectedSampleRate(int sampleRate)
        {
            switch (sampleRate)
            {
                case 8000:
                    radioButtonSampleRate8000.Checked = true;
                    break;
                case 12000:
                    radioButtonSampleRate12000.Checked = true;
                    break;
                case 16000:
                    radioButtonSampleRate16000.Checked = true;
                    break;
                case 24000:
                    radioButtonSampleRate24000.Checked = true;
                    break;
                case 48000:
                    radioButtonSampleRate48000.Checked = true;
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
