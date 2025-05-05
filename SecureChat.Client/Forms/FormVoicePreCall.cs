using NAudio.CoreAudioApi;
using NAudio.Wave;
using NTDLS.WinFormsHelpers;
using SecureChat.Client.Audio;
using SecureChat.Library;

namespace SecureChat.Client.Forms
{
    public partial class FormVoicePreCall : Form
    {
        private AudioPump? _audioPump = null;

        private int? _inputDeviceIndex = null;
        public int InputDeviceIndex { get => _inputDeviceIndex ?? 0; }
        private int? _outputDeviceIndex = null;
        public int OutputDeviceIndex { get => _outputDeviceIndex ?? 0; }
        private int _bitrate = 32 * 1000;
        public int Bitrate { get => _bitrate; }

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

            radioButtonBitRateLow.CheckedChanged += RadioButtonBitRate_CheckedChanged;
            radioButtonBitRateStandard.CheckedChanged += RadioButtonBitRate_CheckedChanged;
            radioButtonBitRateBalanced.CheckedChanged += RadioButtonBitRate_CheckedChanged;
            radioButtonBitRateHighFidelity.CheckedChanged += RadioButtonBitRate_CheckedChanged;

            radioButtonBitRateStandard.Checked = true;
        }

        private void RadioButtonBitRate_CheckedChanged(object? sender, EventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.Checked)
            {
                if (radioButtonBitRateLow.Checked)
                    _bitrate = 16 * 1000;
                else if (radioButtonBitRateStandard.Checked)
                    _bitrate = 32 * 1000;
                else if (radioButtonBitRateBalanced.Checked)
                    _bitrate = 64 * 1000;
                else if (radioButtonBitRateHighFidelity.Checked)
                    _bitrate = 96 * 1000;

                PropUpAudio();
            }
        }

        private void ComboBoxAudioInputDevice_SelectedIndexChanged(object? sender, EventArgs e)
        {
            _inputDeviceIndex = (comboBoxAudioInputDevice.SelectedItem as AudioDeviceComboItem)?.DeviceIndex;
            PropUpAudio();
        }

        private void ComboBoxAudioOutputDevice_SelectedIndexChanged(object? sender, EventArgs e)
        {
            _outputDeviceIndex = (comboBoxAudioOutputDevice.SelectedItem as AudioDeviceComboItem)?.DeviceIndex;
            PropUpAudio();
        }

        private void PropUpAudio()
        {
            if (_inputDeviceIndex != null && _outputDeviceIndex != null)
            {
                _audioPump?.Stop();

                _audioPump = new AudioPump(_inputDeviceIndex.Value, _outputDeviceIndex.Value, _bitrate);

                _audioPump.OnInputSample += (volume) =>
                {
                    BeginInvoke(new Action(() =>
                    {
                        volumeMeterInput.Amplitude = volume;
                    }));
                };

                _audioPump.OnFrameProduced += (byte[] bytes, int byteCount) =>
                {
                    _audioPump.IngestFrame(bytes, byteCount);
                };

                _audioPump.StartCapture();
                _audioPump.StartPlayback();
            }
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            if (_inputDeviceIndex != null && _outputDeviceIndex != null)
            {
                this.InvokeClose(DialogResult.OK);
                return;
            }
            MessageBox.Show("You must select an input and output device.", ScConstants.AppName, MessageBoxButtons.OK);
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.InvokeClose(DialogResult.Cancel);
        }
    }
}
