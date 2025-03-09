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
        private int _bitRate = 32 * 1000;

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

            radioButtonBitRateLowQuality.CheckedChanged += RadioButtonBitRate_CheckedChanged;
            radioButtonBitRateMediumQuality.CheckedChanged += RadioButtonBitRate_CheckedChanged;
            radioButtonBitRateHighQuality.CheckedChanged += RadioButtonBitRate_CheckedChanged;
            radioButtonBitRateExtraHighQuality.CheckedChanged += RadioButtonBitRate_CheckedChanged;

            SetSelectedBitRate(_bitRate);
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
                _bitRate = GetSelectedBitRate();
                PropUpAudio();
            }
        }

        private int GetSelectedBitRate()
        {
            if (radioButtonBitRateLowQuality.Checked)
                return 16 * 1000;
            else if (radioButtonBitRateMediumQuality.Checked)
                return 32 * 1000;
            else if (radioButtonBitRateHighQuality.Checked)
                return 64 * 1000;
            else if (radioButtonBitRateExtraHighQuality.Checked)
                return 96 * 1000;

            return 32 * 1000;
        }

        private void SetSelectedBitRate(int bitRate)
        {
            switch (bitRate)
            {
                case 16 * 1000:
                    radioButtonBitRateLowQuality.Checked = true;
                    break;
                case 32 * 1000:
                    radioButtonBitRateMediumQuality.Checked = true;
                    break;
                case 64 * 1000:
                    radioButtonBitRateHighQuality.Checked = true;
                    break;
                case 96 * 1000:
                    radioButtonBitRateExtraHighQuality.Checked = true;
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

                _audioPump = new AudioPump(_selectedInputDeviceIndex.Value, _selectedIOutputDeviceIndex.Value, _bitRate);

                _audioPump.OnInputSample += (volume) =>
                {
                    BeginInvoke(new Action(() =>
                    {
                        volumeMeterInput.Amplitude = volume;
                    }));
                };

                _audioPump.OnFrameProduced += (byte[] bytes) =>
                {
                    _audioPump.IngestFrame(bytes);
                };

                int captureSampleRate = _audioPump.StartCapture();
                _audioPump.StartPlayback(captureSampleRate);
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
