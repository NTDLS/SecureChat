﻿using NAudio.CoreAudioApi;
using NAudio.Wave;
using NTDLS.WinFormsHelpers;
using SecureChat.Client.Audio;

namespace SecureChat.Client.Forms
{
    public partial class FormVoicePreCall : Form
    {
        private int? _selectedInputDeviceIndex = null;
        private int? _selectedIOutputDeviceIndex = null;
        private AudioPump? _audioPump = null;

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
                PropUpAudio();
            }
        }

        private int GetSelectedBitRate()
        {
            if (radioButtonBitRateLow.Checked)
                return 16 * 1000;
            else if (radioButtonBitRateStandard.Checked)
                return 32 * 1000;
            else if (radioButtonBitRateBalanced.Checked)
                return 64 * 1000;
            else if (radioButtonBitRateHighFidelity.Checked)
                return 96 * 1000;

            return 32 * 1000;
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

                int bitRate = GetSelectedBitRate();

                _audioPump = new AudioPump(_selectedInputDeviceIndex.Value, _selectedIOutputDeviceIndex.Value, bitRate);

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
            this.InvokeClose(DialogResult.OK);
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.InvokeClose(DialogResult.Cancel);
        }
    }
}
