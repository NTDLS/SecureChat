using NAudio.CoreAudioApi;
using NAudio.Wave;
using NTDLS.Helpers;

namespace SecureChat.Client.Audio
{
    internal class AudioPump
    {
        public delegate void VolumeSampleEventHandler(float volume);
        public event VolumeSampleEventHandler? OnVolumeSample;

        private readonly int _outputDeviceIndex;
        private readonly int _inputDeviceIndex;

        private bool _IsRunning = false;
        private bool _keepRunning = false;

        public bool Mute { get; set; } = false;
        public float Gain { get; set; } = 0.5f;
        public int SampleRate { get; private set; } = 22050;

        public AudioPump(int inputDeviceIndex, int outputDeviceIndex, int sampleRate = 22050)
        {
            SampleRate = sampleRate;
            _inputDeviceIndex = inputDeviceIndex;
            _outputDeviceIndex = outputDeviceIndex;
        }

        const int ResampleBufferSize = 1024;

        public void Start()
        {
            _keepRunning = true;

            bool isRecordingRunning = false;
            bool isPlaybackRunning = false;
            bool isBuffering = false;

            new Thread(() =>
                {
                    var enumerator = new MMDeviceEnumerator();
                    var outputDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToList();

                    WaveFormat? inputWaveFormat = null;
                    var inputDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToList();
                    for (int device = 0; device < WaveInEvent.DeviceCount; device++)
                    {
                        if (_inputDeviceIndex == device)
                        {
                            var capabilities = WaveInEvent.GetCapabilities(device);
                            var mmDevice = inputDevices.FirstOrDefault(o => o.FriendlyName.StartsWith(capabilities.ProductName));
                            if (mmDevice != null)
                            {
                                inputWaveFormat = mmDevice.AudioClient.MixFormat;
                            }
                        }
                    }

                    inputWaveFormat.EnsureNotNull();

                    var waveIn = new WaveInEvent
                    {
                        //Example: 44.1kHz, 16-bit, Mono
                        WaveFormat = new WaveFormat(inputWaveFormat.SampleRate, inputWaveFormat.BitsPerSample, inputWaveFormat.Channels),
                        BufferMilliseconds = 75,
                        DeviceNumber = _inputDeviceIndex
                    };

                    var transmissionWaveFormat = new WaveFormat(SampleRate, 16, 1);

                    var waveOut = new WasapiOut(outputDevices[_outputDeviceIndex], AudioClientShareMode.Shared, false, 50);

                    var bufferStream = new BufferedWaveProvider(waveOut.OutputWaveFormat)
                    {
                        BufferLength = 64 * 1024,
                        DiscardOnBufferOverflow = true
                    };

                    Console.WriteLine($"Input: {waveIn.WaveFormat.SampleRate} Hz, {waveIn.WaveFormat.BitsPerSample}-bit, {waveIn.WaveFormat.Channels}ch");
                    Console.WriteLine($"Transmission: {transmissionWaveFormat.SampleRate} Hz, {transmissionWaveFormat.BitsPerSample}-bit, {transmissionWaveFormat.Channels}ch");
                    Console.WriteLine($"Output: {waveOut.OutputWaveFormat.SampleRate} Hz, {waveOut.OutputWaveFormat.BitsPerSample}-bit, {waveOut.OutputWaveFormat.Channels}ch");

                    waveIn.DataAvailable += (sender, e) =>
                    {
                        if (Mute)
                        {
                            return;
                        }

                        var transmissionBytes = ResampleForTransmission(e, waveIn.WaveFormat, transmissionWaveFormat);

                        if (OnVolumeSample != null)
                        {
                            var volume = CalculateVolumeLevelWithGain(transmissionBytes);
                            OnVolumeSample.Invoke(volume);
                        }

                        //Mock sending transmissionBytes to remote peer.

                        ResampleForOutput(transmissionBytes, transmissionWaveFormat, waveOut.OutputWaveFormat, bufferStream);
                    };

                    waveOut.Init(bufferStream);
                    waveIn.StartRecording();
                    isRecordingRunning = true;
                    waveOut.Play();
                    isPlaybackRunning = true;
                    _IsRunning = true;

                    waveIn.RecordingStopped += (object? sender, StoppedEventArgs e) =>
                    {
                        isRecordingRunning = false;
                    };

                    waveOut.PlaybackStopped += (object? sender, StoppedEventArgs e) =>
                    {
                        isPlaybackRunning = false;
                    };

                    while (_keepRunning)
                    {
                        Thread.Sleep(100);
                    }

                    waveIn?.StopRecording();
                    waveOut?.Stop();

                    while (isRecordingRunning || isPlaybackRunning || isBuffering)
                    {
                        Thread.Sleep(10);
                    }

                    waveIn?.Dispose();
                    waveOut?.Dispose();

                    _IsRunning = false;
                }).Start();
        }

        public byte[] ResampleForTransmission(WaveInEventArgs recorded, WaveFormat inputFormat, WaveFormat outputFormat)
        {
            using var inputStream = new RawSourceWaveStream(new MemoryStream(recorded.Buffer, 0, recorded.BytesRecorded, writable: false), inputFormat);
            using var resampler = new ResamplerDmoStream(inputStream, outputFormat);

            var volumeProvider = new VolumeWaveProvider16(resampler)
            {
                Volume = Gain
            };

            var resampledBuffer = new byte[ResampleBufferSize];
            using var outputStream = new MemoryStream();

            int bytesRead;
            while ((bytesRead = volumeProvider.Read(resampledBuffer, 0, resampledBuffer.Length)) > 0)
            {
                outputStream.Write(resampledBuffer, 0, bytesRead);
            }

            return outputStream.ToArray();
        }

        public static void ResampleForOutput(byte[] inputBytes, WaveFormat inputFormat, WaveFormat outputFormat, BufferedWaveProvider outputBuffer)
        {
            using var inputStream = new RawSourceWaveStream(new MemoryStream(inputBytes, 0, inputBytes.Length, writable: false), inputFormat);
            using var resampler = new ResamplerDmoStream(inputStream, outputFormat);
            var buffer = new byte[ResampleBufferSize];

            int bytesRead;
            while ((bytesRead = resampler.Read(buffer, 0, buffer.Length)) > 0)
            {
                if (outputBuffer.BufferedDuration.TotalMilliseconds < 500) // Prevents overflow by dropping extra data.
                {
                    outputBuffer.AddSamples(buffer, 0, bytesRead);
                }
            }
        }

        public void Stop()
        {
            _keepRunning = false;

            while (_IsRunning)
            {
                Thread.Sleep(10);
            }
        }

        /// <summary>
        // Calculate volume level from raw PCM data
        /// </summary>
        private static float CalculateVolumeLevelRMS(byte[] buffer)
        {
            // Compute RMS level of audio to display on meter
            int sampleCount = buffer.Length / 2; // 16-bit audio (2 bytes per sample)
            double sum = 0;

            for (int i = 0; i < buffer.Length; i += 2)
            {
                short sample = (short)(buffer[i] | (buffer[i + 1] << 8)); // Convert bytes to 16-bit sample
                sum += sample * sample;
            }

            double rms = Math.Sqrt(sum / sampleCount);
            float normalized = (float)(rms / 32768.0); // Normalize to range 0.0 - 1.0
            return Math.Min(1.0f, normalized); // Ensure value is within range
        }

        private static float CalculateVolumeLevelWithGain(byte[] buffer, float gain = 100.0f)
        {
            short maxSample = 0;
            for (int i = 0; i < buffer.Length; i += 2)
            {
                short sample = (short)(buffer[i] | (buffer[i + 1] << 8));
                maxSample = Math.Max(maxSample, Math.Abs(sample));
            }

            float normalized = maxSample / 32768.0f;
            return Math.Min(1.0f, normalized * gain);
        }
    }
}
