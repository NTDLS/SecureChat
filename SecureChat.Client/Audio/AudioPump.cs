using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace SecureChat.Client.Audio
{
    internal class AudioPump
    {
        public delegate void VolumeSampleEventHandler(float volume);
        public event VolumeSampleEventHandler? OnVolumeSample;

        private WaveInEvent? _waveIn;
        private WasapiOut? _waveOut;
        private BufferedWaveProvider? _bufferStream;
        private ResamplerDmoStream? _sampler;

        private readonly int _outputDeviceIndex;
        private readonly int _inputDeviceIndex;

        private Thread? _thread;
        private bool _running = false;
        private bool _keepRunning = false;
        private bool _recordingRunning = false;
        private bool _playbackRunning = false;
        private bool _buffering = false;

        public bool Mute { get; set; } = false;
        public float Gain { get; set; } = 0.5f;
        public int SampleRate { get; private set; }

        public AudioPump(int inputDeviceIndex, int outputDeviceIndex, int sampleRate = 22050)
        {
            SampleRate = sampleRate;
            _inputDeviceIndex = inputDeviceIndex;
            _outputDeviceIndex = outputDeviceIndex;
        }

        public void Start()
        {
            _keepRunning = true;

            _thread = new Thread(() =>
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

                _waveIn = new WaveInEvent
                {
                    WaveFormat = inputWaveFormat, //new WaveFormat(SampleRate, 16, 1), //inputWaveFormat, // 44.1kHz, 16-bit, Mono
                    BufferMilliseconds = 100,
                    DeviceNumber = _inputDeviceIndex
                };

                _bufferStream = new BufferedWaveProvider(_waveIn.WaveFormat)
                {
                    DiscardOnBufferOverflow = true
                };

                _waveOut = new WasapiOut(outputDevices[_outputDeviceIndex], AudioClientShareMode.Shared, false, 100);

                _sampler = new ResamplerDmoStream(_bufferStream, _waveOut.OutputWaveFormat);
                //_sampler = new ResamplerDmoStream(_bufferStream, new WaveFormat(SampleRate, 16, 1));
                //Console.WriteLine($"Input Format: {_waveIn.WaveFormat.SampleRate} Hz, {_waveIn.WaveFormat.BitsPerSample}-bit, {_waveIn.WaveFormat.Channels}ch");
                //Console.WriteLine($"Resampler Format: {_sampler.WaveFormat.SampleRate} Hz, {_sampler.WaveFormat.BitsPerSample}-bit, {_sampler.WaveFormat.Channels}ch");
                //Console.WriteLine($"Output Format: {_waveOut.OutputWaveFormat.SampleRate} Hz, {_waveOut.OutputWaveFormat.BitsPerSample}-bit, {_waveOut.OutputWaveFormat.Channels}ch");

                _waveIn.DataAvailable += (sender, e) =>
                {
                    _buffering = true;
                    if (Mute)
                    {
                        return;
                    }
                    byte[] buffer = e.Buffer;
                    for (int i = 0; i < e.BytesRecorded; i += 2)
                    {
                        var sample = (short)(buffer[i] | (buffer[i + 1] << 8)); // Convert to 16-bit sample.
                        sample = (short)(sample * Gain); // Reduce volume.
                        //Reconstruct byte.
                        buffer[i] = (byte)(sample & 0xFF); //Least significant bit.
                        buffer[i + 1] = (byte)((sample >> 8) & 0xFF); //Most significant bit.
                    }

                    if (OnVolumeSample != null)
                    {
                        //var volume = CalculateVolumeLevelWithGain(buffer, e.BytesRecorded);
                        //OnVolumeSample.Invoke(volume);
                    }

                    _bufferStream.AddSamples(e.Buffer, 0, e.BytesRecorded);
                    _buffering = false;
                };

                _waveOut.Init(_sampler);
                _waveIn.StartRecording();
                _recordingRunning = true;
                _waveOut.Play();
                _playbackRunning = true;
                _running = true;

                _waveIn.RecordingStopped += (object? sender, StoppedEventArgs e) =>
                {
                    _recordingRunning = false;
                };

                _waveOut.PlaybackStopped += (object? sender, StoppedEventArgs e) =>
                {
                    _playbackRunning = false;
                };

                while (_keepRunning)
                {
                    Thread.Sleep(100);
                }

                _waveIn?.StopRecording();
                _waveOut?.Stop();

                while (_recordingRunning || _playbackRunning || _buffering)
                {
                    Thread.Sleep(10);
                }

                _waveIn?.Dispose();
                _waveOut?.Dispose();
                _sampler?.Dispose();

                _running = false;
            });

            _thread.Start();
        }

        public void Stop()
        {

            _keepRunning = false;

            while (_running)
            {
                Thread.Sleep(10);
            }

            _thread = null;
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
