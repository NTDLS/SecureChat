using Concentus;
using Concentus.Enums;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NTDLS.Helpers;

namespace SecureChat.Client.Audio
{
    internal class AudioPump
    {
        #region UNUSED

        public delegate void VolumeSampleEventHandler(float volume);
        public event VolumeSampleEventHandler? OnInputSample;
        public float Gain { get; set; } = 1.0f;

        #endregion

        public delegate void FrameProducedEventHandler(byte[] bytes);
        public event FrameProducedEventHandler? OnFrameProduced;

        private readonly int _outputDeviceIndex;
        private readonly int _inputDeviceIndex;
        private bool _IsCaptureRunning = false;
        private bool _IsPlaybackRunning = false;
        private bool _keepRunning = false;
        public int _bitRate;

        private readonly AutoResetEvent _ingestionEvent = new AutoResetEvent(false);

        public bool Mute { get; set; } = false;
        public int CaptureSampleRate { get; private set; }

        public AudioPump(int inputDeviceIndex, int outputDeviceIndex, int bitRate)
        {
            _bitRate = bitRate;
            _inputDeviceIndex = inputDeviceIndex;
            _outputDeviceIndex = outputDeviceIndex;
        }

        private readonly Queue<byte[]> _playbackBuffer = new();

        public void IngestFrame(byte[] bytes)
        {
            lock (_playbackBuffer)
            {
                _playbackBuffer.Enqueue(bytes);
                _ingestionEvent.Set();
            }
        }

        public int StartCapture()
        {
            _keepRunning = true;

            new Thread(() => //Audio capture thread.
            {
                var enumerator = new MMDeviceEnumerator();

                WaveFormat? nativeInputDeviceWaveFormat = null;
                var inputDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToList();
                for (int device = 0; device < WaveInEvent.DeviceCount; device++)
                {
                    if (_inputDeviceIndex == device)
                    {
                        var capabilities = WaveInEvent.GetCapabilities(device);
                        var mmDevice = inputDevices.FirstOrDefault(o => o.FriendlyName.StartsWith(capabilities.ProductName));
                        if (mmDevice != null)
                        {
                            nativeInputDeviceWaveFormat = mmDevice.AudioClient.MixFormat;
                        }
                    }
                }

                nativeInputDeviceWaveFormat.EnsureNotNull();

                CaptureSampleRate = nativeInputDeviceWaveFormat.SampleRate; //Should come from client.

                var waveIn = new WaveInEvent
                {
                    WaveFormat = new WaveFormat(CaptureSampleRate, 16, 1),
                    //If the mic will allow 16bit mono, then lets just roll with it.
                    //WaveFormat = new WaveFormat(inputWaveFormat.SampleRate, inputWaveFormat.BitsPerSample, inputWaveFormat.Channels),
                    BufferMilliseconds = 10, //Should be equal or less than the Opus Codec ms per frameSize.
                    DeviceNumber = _inputDeviceIndex
                };

                //Console.WriteLine($"Input: {waveIn.WaveFormat.SampleRate} Hz, {waveIn.WaveFormat.BitsPerSample}-bit, {waveIn.WaveFormat.Channels}ch");

                var encoder = OpusCodecFactory.CreateEncoder(CaptureSampleRate, 1, OpusApplication.OPUS_APPLICATION_VOIP);
                encoder.Bitrate = _bitRate;
                encoder.ForceMode = OpusMode.MODE_SILK_ONLY;
                encoder.SignalType = OpusSignal.OPUS_SIGNAL_VOICE;
                encoder.Complexity = 5;
                encoder.ForceChannels = 1;

                int frameSize = CaptureSampleRate / 50; // 20ms frame size (1000/50) = 20. (should be greater than WaveInEvent.BufferMilliseconds).
                var partialBuffer = new List<short>(); // Buffer for leftover PCM data
                var encodedFrameBytes = new byte[1275]; // Max Opus packet size

                waveIn.DataAvailable += (sender, e) =>
                {
                    if (Mute) return;

                    var pcmShorts = BytesToShorts(e.Buffer, e.BytesRecorded);

                    if (OnInputSample != null)
                    {
                        var volume = CalculateVolumeLevelWithGain(e.Buffer, e.BytesRecorded);
                        OnInputSample.Invoke(volume);
                    }

                    if (partialBuffer.Count > 0)
                    {
                        // Prepend leftover PCM from previous buffer to the new PCM data
                        pcmShorts = partialBuffer.Concat(pcmShorts).ToArray();
                        partialBuffer.Clear();
                    }

                    int i = 0;
                    for (; i + frameSize <= pcmShorts.Length; i += frameSize)
                    {
                        int encodedFrameByteCount = encoder.Encode(
                            new ReadOnlySpan<short>(pcmShorts, i, frameSize),
                            frameSize, new Span<byte>(encodedFrameBytes), encodedFrameBytes.Length);

                        OnFrameProduced?.Invoke(encodedFrameBytes.Take(encodedFrameByteCount).ToArray());
                    }

                    if (i < pcmShorts.Length)
                    {
                        partialBuffer.AddRange(pcmShorts.Skip(i)); // Store leftover samples to be prepended to the next buffer.
                    }
                };

                waveIn.StartRecording();
                _IsCaptureRunning = true;

                while (_keepRunning)
                {
                    Thread.Sleep(100);
                }

                waveIn?.StopRecording();
                waveIn?.Dispose();

                _IsCaptureRunning = false;
            }).Start();

            while (!_IsCaptureRunning)
            {
                Thread.Sleep(1); // This loop is just for debugging.
            }

            return CaptureSampleRate;
        }

        public void StartPlayback(int originalCaptureSampleRate)
        {
            new Thread(() => //Audio playback thread.
            {
                var enumerator = new MMDeviceEnumerator();
                var outputDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToList();

                var waveOut = new WasapiOut(outputDevices[_outputDeviceIndex], AudioClientShareMode.Shared, true, 50);
                var outputBufferStream = new BufferedWaveProvider(waveOut.OutputWaveFormat)
                {
                    BufferLength = 64 * 1024,
                    DiscardOnBufferOverflow = true
                };

                //Console.WriteLine($"Output: {waveOut.OutputWaveFormat.SampleRate} Hz, {waveOut.OutputWaveFormat.BitsPerSample}-bit, {waveOut.OutputWaveFormat.Channels}ch");

                var decoder = OpusCodecFactory.CreateDecoder(originalCaptureSampleRate, 1);

                waveOut.Init(outputBufferStream);
                waveOut.Play();
                _IsPlaybackRunning = true;

                var transmissionWaveFormat = new WaveFormat(originalCaptureSampleRate, 1);
                var decodedPcm = new short[originalCaptureSampleRate];
                int frameSize = CaptureSampleRate / 50; // 20ms frame size (1000/50) = 20.

                while (_keepRunning)
                {
                    byte[]? bytes;
                    bool gotData = false;

                    lock (_playbackBuffer)
                    {
                        gotData = _playbackBuffer.TryDequeue(out bytes);
                    }

                    if (gotData && bytes != null)
                    {
                        int decodedSamples = decoder.Decode(new ReadOnlySpan<byte>(bytes), new Span<short>(decodedPcm), frameSize, false);
                        var pcmBytes = ShortsToBytes(decodedPcm, decodedSamples);
                        ResampleForOutput(pcmBytes, transmissionWaveFormat, waveOut.OutputWaveFormat, outputBufferStream);
                    }
                    else
                    {
                        _ingestionEvent.WaitOne(1);
                    }
                }

                waveOut?.Stop();

                while (waveOut?.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(10);
                }

                waveOut?.Dispose();

                _IsPlaybackRunning = false;
            }).Start();
        }

        public static short[] BytesToShorts(byte[] input)
            => BytesToShorts(input, input.Length);

        public static short[] BytesToShorts(byte[] input, int length)
        {
            short[] processedValues = new short[length / 2];
            for (int c = 0; c < processedValues.Length; c++)
            {
                processedValues[c] = (short)(((int)input[(c * 2)]) << 0);
                processedValues[c] += (short)(((int)input[(c * 2) + 1]) << 8);
            }

            return processedValues;
        }

        public static byte[] ShortsToBytes(short[] input)
            => ShortsToBytes(input, input.Length);

        public static byte[] ShortsToBytes(short[] input, int length)
        {
            byte[] processedValues = new byte[length * 2];
            for (int c = 0; c < length; c++)
            {
                processedValues[c * 2] = (byte)(input[c] & 0xFF);
                processedValues[c * 2 + 1] = (byte)((input[c] >> 8) & 0xFF);
            }

            return processedValues;
        }

        /*
        public byte[] ResampleForTransmission(WaveInEventArgs recorded, WaveFormat inputFormat, WaveFormat outputFormat, float gain)
        {
            using var inputStream = new RawSourceWaveStream(new MemoryStream(recorded.Buffer, 0, recorded.BytesRecorded, writable: false), inputFormat);
            using var resampler = new MediaFoundationResampler(inputStream, outputFormat);

            var volumeProvider = new VolumeWaveProvider16(resampler)
            {
                Volume = gain
            };

            var bufferSize = CalculateBufferSize(inputFormat);
            var buffer = new byte[bufferSize];
            using var outputStream = new MemoryStream();

            int bytesRead;
            while ((bytesRead = volumeProvider.Read(buffer, 0, buffer.Length)) > 0)
            {
                outputStream.Write(buffer, 0, bytesRead);
            }

            var output = outputStream.ToArray();
            OnInputSample?.Invoke(CalculateVolumeLevelWithGain(output));
            return output;
        }
        */

        private static int CalculateBufferSize(WaveFormat inputFormat, int milliseconds = 50)
        {
            // Compute buffer size based on sample rate & frame size.
            int bytesPerSample = inputFormat.BitsPerSample / 8 * inputFormat.Channels;
            int bytesPerMs = inputFormat.SampleRate * bytesPerSample / 1000;
            return bytesPerMs * milliseconds;
        }

        /*
        public static byte[] ResampleForTransmission(WaveInEventArgs recorded, WaveFormat inputFormat, WaveFormat outputFormat)
        {
            using var inputStream = new RawSourceWaveStream(new MemoryStream(recorded.Buffer, 0, recorded.BytesRecorded, writable: false), inputFormat);
            using var resampler = new MediaFoundationResampler(inputStream, outputFormat);

            var bufferSize = CalculateBufferSize(inputFormat);
            var buffer = new byte[bufferSize];

            using var outputStream = new MemoryStream();

            int bytesRead;
            while ((bytesRead = resampler.Read(buffer, 0, buffer.Length)) > 0)
            {
                outputStream.Write(buffer, 0, bytesRead);
            }

            return outputStream.ToArray();
        }
        */

        /// <summary>
        /// Resamples and writes to the audio output device.
        /// </summary>
        public static void ResampleForOutput(byte[] inputBytes, WaveFormat inputFormat, WaveFormat outputFormat, BufferedWaveProvider outputBuffer)
            => ResampleForOutput(inputBytes, inputBytes.Length, inputFormat, outputFormat, outputBuffer);

        /// <summary>
        /// Resamples and writes to the audio output device.
        /// </summary>
        public static void ResampleForOutput(byte[] inputBytes, int byteCount, WaveFormat inputFormat, WaveFormat outputFormat, BufferedWaveProvider outputBuffer)
        {
            using var inputStream = new RawSourceWaveStream(new MemoryStream(inputBytes, 0, byteCount, writable: false), inputFormat);
            using var resampler = new MediaFoundationResampler(inputStream, outputFormat);

            var bufferSize = CalculateBufferSize(inputFormat);
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

            while (_IsCaptureRunning || _IsPlaybackRunning)
            {
                Thread.Sleep(10);
            }
        }

        /*
        /// <summary>
        // Calculate RMS volume level from raw PCM data
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
        */

        /// <summary>
        // Calculate volume level from raw PCM data
        /// </summary>
        private static float CalculateVolumeLevelWithGain(byte[] bytes, int byteCount)
        {
            short maxSample = 0;
            for (int i = 0; i < byteCount; i += 2)
            {
                short sample = (short)(bytes[i] | (bytes[i + 1] << 8));
                maxSample = Math.Max(maxSample, Math.Abs(sample));
            }

            float normalized = maxSample / 32768.0f;
            return Math.Min(1.0f, normalized);
        }

    }
}
