using NAudio.CoreAudioApi;
using NAudio.Wave;
using NTDLS.Helpers;
using System.IO;

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
                        BufferMilliseconds = 100,
                        DeviceNumber = _inputDeviceIndex
                    };

                    var transmissionWaveFormat = new WaveFormat(SampleRate, 16, 1);

                    var bufferStream = new BufferedWaveProvider(waveIn.WaveFormat)
                    {
                        DiscardOnBufferOverflow = true
                    };

                    var waveOut = new WasapiOut(outputDevices[_outputDeviceIndex], AudioClientShareMode.Shared, false, 100);

                    Console.WriteLine($"Input: {waveIn.WaveFormat.SampleRate} Hz, {waveIn.WaveFormat.BitsPerSample}-bit, {waveIn.WaveFormat.Channels}ch");
                    Console.WriteLine($"Transmission: {transmissionWaveFormat.SampleRate} Hz, {transmissionWaveFormat.BitsPerSample}-bit, {transmissionWaveFormat.Channels}ch");
                    Console.WriteLine($"Output: {waveOut.OutputWaveFormat.SampleRate} Hz, {waveOut.OutputWaveFormat.BitsPerSample}-bit, {waveOut.OutputWaveFormat.Channels}ch");

                    waveIn.DataAvailable += (sender, e) =>
                    {
                        if (Mute)
                        {
                            return;
                        }

                        var transmissionBytes = ResampleAudioBytes(e.Buffer, e.BytesRecorded, waveIn.WaveFormat, transmissionWaveFormat);

                        //Mock sending transmissionBytes to remote peer.

                        var outputBytes = ResampleAudioBytes(transmissionBytes, transmissionBytes.Length, transmissionWaveFormat, waveOut.OutputWaveFormat);

                        bufferStream.AddSamples(outputBytes, 0, outputBytes.Length);
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

        public static byte[] ResampleAudioBytes(byte[] inputBytes, int inputByteCount, WaveFormat inputFormat, WaveFormat outputFormat)
        {
            using var inputStream = new RawSourceWaveStream(new MemoryStream(inputBytes, 0, inputByteCount), inputFormat);
            using var resampler = new ResamplerDmoStream(inputStream, outputFormat);
            using var outputStream = new MemoryStream();

            var buffer = new byte[outputFormat.AverageBytesPerSecond];

            int bytesRead;
            while ((bytesRead = resampler.Read(buffer, 0, buffer.Length)) > 0)
            {
                outputStream.Write(buffer, 0, bytesRead);
            }

            return outputStream.ToArray();
        }

        public void Stop()
        {
            _keepRunning = false;

            while (_IsRunning)
            {
                Thread.Sleep(10);
            }
        }

        /*
        static void GetInputDevices()
        {
            var enumerator = new MMDeviceEnumerator();

            Console.WriteLine("Available Input Devices:");
            var inputDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToList();
            for (int device = 0; device < WaveInEvent.DeviceCount; device++)
            {
                var capabilities = WaveInEvent.GetCapabilities(device);
                var mmDevice = inputDevices.FirstOrDefault(o => o.FriendlyName.StartsWith(capabilities.ProductName));
                if (mmDevice != null)
                {
                    Console.WriteLine($"{device}: {mmDevice.FriendlyName}");
                }
            }

            Console.WriteLine("Available Output Devices:");
            var outputDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            for (int device = 0; device < outputDevices.Count; device++)
            {
                Console.WriteLine($"{device}: {outputDevices[device].FriendlyName}");
            }
        }         
         */

        /// <summary>
        // Calculate volume level from raw PCM data
        /// </summary>
        private static float CalculateVolumeLevelRMS(byte[] buffer, int bytesRead)
        {
            // Compute RMS level of audio to display on meter
            int sampleCount = bytesRead / 2; // 16-bit audio (2 bytes per sample)
            double sum = 0;

            for (int i = 0; i < bytesRead; i += 2)
            {
                short sample = (short)(buffer[i] | (buffer[i + 1] << 8)); // Convert bytes to 16-bit sample
                sum += sample * sample;
            }

            double rms = Math.Sqrt(sum / sampleCount);
            float normalized = (float)(rms / 32768.0); // Normalize to range 0.0 - 1.0
            return Math.Min(1.0f, normalized); // Ensure value is within range
        }

        private static float CalculateVolumeLevelWithGain(byte[] buffer, int bytesRead, float gain = 100.0f)
        {
            short maxSample = 0;
            for (int i = 0; i < bytesRead; i += 2)
            {
                short sample = (short)(buffer[i] | (buffer[i + 1] << 8));
                maxSample = Math.Max(maxSample, Math.Abs(sample));
            }

            float normalized = maxSample / 32768.0f;
            return Math.Min(1.0f, normalized * gain);
        }
    }
}
