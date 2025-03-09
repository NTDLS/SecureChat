using Concentus;
using Concentus.Enums;
using NAudio.CoreAudioApi;
using NAudio.Wave;

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
        public const int CaptureSampleRate = 48000; //We either capture at 48Khz (preferred) or resample to it.

        private readonly SupportedWaveFormat[] _inputFormatPriorities = {
            SupportedWaveFormat.WAVE_FORMAT_48M16, //48 kHz, Mono, 16-bit
            SupportedWaveFormat.WAVE_FORMAT_44M16, //44.1 kHz, Mono, 16-bit
            //SupportedWaveFormat.WAVE_FORMAT_44S16, //44.1 kHz, Stereo, 16-bit
            //SupportedWaveFormat.WAVE_FORMAT_4S16, //44.1 kHz, Stereo, 16-bit
            //SupportedWaveFormat.WAVE_FORMAT_48S16, //48 kHz, Stereo, 16-bit
            SupportedWaveFormat.WAVE_FORMAT_96M16, //96 kHz, Mono, 16-bit
            //SupportedWaveFormat.WAVE_FORMAT_96S16, //96 kHz, Stereo, 16-bit
            SupportedWaveFormat.WAVE_FORMAT_2M16, //22.05 kHz, Mono, 16-bit
            //SupportedWaveFormat.WAVE_FORMAT_2S16, //22.05 kHz, Stereo, 16-bit
            SupportedWaveFormat.WAVE_FORMAT_1M16, //11.025 kHz, Mono, 16-bit
            //SupportedWaveFormat.WAVE_FORMAT_1S16 //11.025 kHz, Stereo, 16-bit
        };

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

        public void Stop()
        {
            _keepRunning = false;

            while (_IsCaptureRunning || _IsPlaybackRunning)
            {
                Thread.Sleep(10);
            }
        }

        public int StartCapture()
        {
            _keepRunning = true;

            new Thread(() => //Audio capture thread.
            {
                int? bestSupportedSampleRate = null;

                var inputDeviceCapabilities = WaveInEvent.GetCapabilities(_inputDeviceIndex);
                foreach (var inputFormat in _inputFormatPriorities)
                {
                    if (inputDeviceCapabilities.SupportsWaveFormat(inputFormat))
                    {
                        bestSupportedSampleRate = inputFormat switch
                        {
                            SupportedWaveFormat.WAVE_FORMAT_48M16 => 48000, //48 kHz, Mono, 16-bit (preferred due to Opus codec)
                            SupportedWaveFormat.WAVE_FORMAT_44M16 => 44100, //44.1 kHz, Mono, 16-bit
                            SupportedWaveFormat.WAVE_FORMAT_96M16 => 96000, //96 kHz, Mono, 16-bit
                            SupportedWaveFormat.WAVE_FORMAT_2M16 => 22050, //22.05 kHz, Mono, 16-bit
                            SupportedWaveFormat.WAVE_FORMAT_1M16 => 11025, //11.025 kHz, Mono, 16-bit
                            _ => throw new NotSupportedException(),
                        };
                        break;
                    }
                }

                if (bestSupportedSampleRate == null)
                {
                    throw new Exception("No acceptable capture rates are supported.");
                }

                WaveFormat? captureResampleFormat = null;
                if (bestSupportedSampleRate != CaptureSampleRate)
                {
                    //Opus only supports 8/12/16/24/48 Khz, so if the mic does not support 48Khz then we will resample the input.
                    captureResampleFormat = new WaveFormat(CaptureSampleRate, 16, 1);
                }

                var waveIn = new WaveInEvent
                {
                    WaveFormat = new WaveFormat(bestSupportedSampleRate.Value, 16, 1),
                    BufferMilliseconds = 10, //Should be equal or less than the Opus Codec ms per frameSize.
                    DeviceNumber = _inputDeviceIndex
                };

                var encoder = OpusCodecFactory.CreateEncoder(CaptureSampleRate, 1, OpusApplication.OPUS_APPLICATION_VOIP);
                encoder.Bitrate = _bitRate;
                encoder.ForceMode = OpusMode.MODE_SILK_ONLY;
                encoder.SignalType = OpusSignal.OPUS_SIGNAL_VOICE;
                encoder.Complexity = 5;
                encoder.ForceChannels = 1;

                int frameSize = CaptureSampleRate / 50; //20ms frame size (1000/50) = 20. (should be greater than WaveInEvent.BufferMilliseconds).
                var partialBuffer = new List<short>(); //Buffer for leftover PCM data
                var encodedFrameBytes = new byte[1275]; //Max Opus packet size

                waveIn.DataAvailable += (sender, e) =>
                {
                    if (Mute) return;

                    short[] pcmShorts;

                    if (captureResampleFormat != null)
                    {
                        var resampledInput = ResampleForTransmission(e, waveIn.WaveFormat, captureResampleFormat);
                        pcmShorts = BytesToShorts(resampledInput, resampledInput.Length);
                    }
                    else
                    {
                        pcmShorts = BytesToShorts(e.Buffer, e.BytesRecorded);
                    }

                    if (OnInputSample != null)
                    {
                        var volume = CalculateMonoVolumeLevel(e.Buffer, e.BytesRecorded);
                        OnInputSample.Invoke(volume);
                    }

                    if (partialBuffer.Count > 0)
                    {
                        //Prepend leftover PCM from previously remaining buffer to the new PCM data
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
                        //Store leftover samples to be prepended to the next buffer.
                        partialBuffer.AddRange(pcmShorts.Skip(i));
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
                Thread.Sleep(1); //This loop is just for debugging.
            }

            return CaptureSampleRate;
        }

        public void StartPlayback(int originalCaptureSampleRate)
        {
            new Thread(() => //Audio playback thread.
            {
                using var enumerator = new MMDeviceEnumerator();
                var outputDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToList();

                var waveOut = new WasapiOut(outputDevices[_outputDeviceIndex], AudioClientShareMode.Shared, true, 50);
                var outputBufferStream = new BufferedWaveProvider(waveOut.OutputWaveFormat)
                {
                    BufferLength = 64 * 1024,
                    DiscardOnBufferOverflow = true
                };

                waveOut.Init(outputBufferStream);
                waveOut.Play();
                _IsPlaybackRunning = true;

                var decoder = OpusCodecFactory.CreateDecoder(originalCaptureSampleRate, 1);
                var transmissionWaveFormat = new WaveFormat(originalCaptureSampleRate, 1);
                var decodedPcm = new short[originalCaptureSampleRate];
                int frameSize = CaptureSampleRate / 50; //20ms frame size (1000/50) = 20.

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

        private static int CalculateBufferSize(WaveFormat inputFormat, int milliseconds = 50)
        {
            //Compute buffer size based on sample rate & frame size.
            int bytesPerSample = inputFormat.BitsPerSample / 8 * inputFormat.Channels;
            int bytesPerMs = inputFormat.SampleRate * bytesPerSample / 1000;
            return bytesPerMs * milliseconds;
        }

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
                if (outputBuffer.BufferedDuration.TotalMilliseconds < 500) //Prevents overflow by dropping extra data.
                {
                    outputBuffer.AddSamples(buffer, 0, bytesRead);
                }
            }
        }

        /// <summary>
        // Calculate volume level from raw PCM data
        /// </summary>
        private static float CalculateMonoVolumeLevel(byte[] bytes, int byteCount)
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
    }
}
