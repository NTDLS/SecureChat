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
        public float Gain { get; set; } = 1.0f;
        public int? SampleRate { get; private set; }

        public AudioPump(int inputDeviceIndex, int outputDeviceIndex, int? sampleRate)
        {
            SampleRate = sampleRate;
            _inputDeviceIndex = inputDeviceIndex;
            _outputDeviceIndex = outputDeviceIndex;
        }

        public void Start()
        {
            _keepRunning = true;

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

                    var transmissionWaveFormat = new WaveFormat(SampleRate ?? inputWaveFormat.SampleRate, 16, 1);

                    var waveIn = new WaveInEvent
                    {
                        WaveFormat = new WaveFormat(inputWaveFormat.SampleRate, 16, 1),
                        //If the mic will allow 16bit mono, then lets just roll with it.
                        //WaveFormat = new WaveFormat(inputWaveFormat.SampleRate, inputWaveFormat.BitsPerSample, inputWaveFormat.Channels),
                        BufferMilliseconds = 75,
                        DeviceNumber = _inputDeviceIndex
                    };

                    var waveOut = new WasapiOut(outputDevices[_outputDeviceIndex], AudioClientShareMode.Shared, true, 50);
                    //waveOut.Volume = 1.0f;

                    var outputBufferStream = new BufferedWaveProvider(waveOut.OutputWaveFormat)
                    {
                        BufferLength = 64 * 1024,
                        DiscardOnBufferOverflow = true
                    };

                    Console.WriteLine($"Input: {waveIn.WaveFormat.SampleRate} Hz, {waveIn.WaveFormat.BitsPerSample}-bit, {waveIn.WaveFormat.Channels}ch");
                    Console.WriteLine($"Transmission: {transmissionWaveFormat.SampleRate} Hz, {transmissionWaveFormat.BitsPerSample}-bit, {transmissionWaveFormat.Channels}ch");
                    Console.WriteLine($"Output: {waveOut.OutputWaveFormat.SampleRate} Hz, {waveOut.OutputWaveFormat.BitsPerSample}-bit, {waveOut.OutputWaveFormat.Channels}ch");

                    bool transcode = true;

                    waveIn.DataAvailable += (sender, e) =>
                    {
                        if (Mute) return;

                        if (transcode)
                        {
                            var transmissionBytes = ResampleForTransmission(e, waveIn.WaveFormat, transmissionWaveFormat, Gain);
                            ResampleForOutput(transmissionBytes, transmissionWaveFormat, waveOut.OutputWaveFormat, outputBufferStream);
                        }
                        else
                        {
                            ResampleForOutput(e.Buffer, e.BytesRecorded, waveIn.WaveFormat, waveOut.OutputWaveFormat, outputBufferStream);
                        }
                    };

                    waveOut.Init(outputBufferStream);
                    waveIn.StartRecording();
                    waveOut.Play();
                    _IsRunning = true;

                    while (_keepRunning)
                    {
                        Thread.Sleep(100);
                    }

                    waveIn?.StopRecording();
                    waveOut?.Stop();

                    while (waveOut?.PlaybackState == PlaybackState.Playing)
                    {
                        Thread.Sleep(10);
                    }

                    waveIn?.Dispose();
                    waveOut?.Dispose();

                    _IsRunning = false;
                }).Start();
        }

        public static byte[] ResampleForTransmission(WaveInEventArgs recorded, WaveFormat inputFormat, WaveFormat outputFormat, float gain)
        {
            using var inputStream = new RawSourceWaveStream(new MemoryStream(recorded.Buffer, 0, recorded.BytesRecorded, writable: false), inputFormat);
            using var resampler = new MediaFoundationResampler(inputStream, outputFormat);

            var volumeProvider = new VolumeWaveProvider16(resampler)
            {
                Volume = gain
            };

            var resampledBuffer = new byte[inputFormat.AverageBytesPerSecond];
            using var outputStream = new MemoryStream();

            int bytesRead;
            while ((bytesRead = volumeProvider.Read(resampledBuffer, 0, resampledBuffer.Length)) > 0)
            {
                outputStream.Write(resampledBuffer, 0, bytesRead);
            }

            return outputStream.ToArray();
        }

        public static byte[] ResampleForTransmission(WaveInEventArgs recorded, WaveFormat inputFormat, WaveFormat outputFormat)
        {
            using var inputStream = new RawSourceWaveStream(new MemoryStream(recorded.Buffer, 0, recorded.BytesRecorded, writable: false), inputFormat);
            using var resampler = new MediaFoundationResampler(inputStream, outputFormat);

            // Compute buffer size based on sample rate & frame size.
            int bytesPerSample = outputFormat.BitsPerSample / 8 * outputFormat.Channels;
            int bytesPerMs = outputFormat.SampleRate * bytesPerSample / 1000;
            int bufferSize = bytesPerMs * 50; // 50ms buffer
            var buffer = new byte[bufferSize];

            using var outputStream = new MemoryStream();

            int bytesRead;
            while ((bytesRead = resampler.Read(buffer, 0, buffer.Length)) > 0)
            {
                outputStream.Write(buffer, 0, bytesRead);
            }

            return outputStream.ToArray();
        }

        public static void ResampleForOutput(byte[] inputBytes, WaveFormat inputFormat, WaveFormat outputFormat, BufferedWaveProvider outputBuffer)
            => ResampleForOutput(inputBytes, inputBytes.Length, inputFormat, outputFormat, outputBuffer);

        public static void ResampleForOutput(byte[] inputBytes, int byteCount, WaveFormat inputFormat, WaveFormat outputFormat, BufferedWaveProvider outputBuffer)
        {
            using var inputStream = new RawSourceWaveStream(new MemoryStream(inputBytes, 0, byteCount, writable: false), inputFormat);
            using var resampler = new MediaFoundationResampler(inputStream, outputFormat);

            // Compute buffer size based on sample rate & frame size.
            int bytesPerSample = outputFormat.BitsPerSample / 8 * outputFormat.Channels;
            int bytesPerMs = outputFormat.SampleRate * bytesPerSample / 1000;
            int bufferSize = bytesPerMs * 50; // 50ms buffer
            var buffer = new byte[bufferSize];

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
