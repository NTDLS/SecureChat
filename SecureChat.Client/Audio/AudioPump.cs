using Concentus;
using Concentus.Enums;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System.Collections.Concurrent;

namespace SecureChat.Client.Audio
{
    internal class AudioPump
    {
        public delegate void VolumeSampleEventHandler(float volume);
        public event VolumeSampleEventHandler? OnInputSample;

        public delegate void FrameProducedEventHandler(byte[] bytes, int byteCount);
        public event FrameProducedEventHandler? OnFrameProduced;

        private bool _IsCaptureRunning = false;
        private bool _IsPlaybackRunning = false;
        private bool _keepRunning = false;
        private readonly int _bitRate;
        private readonly int _inputDeviceIndex;
        private readonly int _outputDeviceIndex;
        private readonly ConcurrentQueue<byte[]> _playbackBuffer = new();
        private readonly AutoResetEvent _ingestionEvent = new(false);

        public bool Mute { get; set; } = false;
        public const int CaptureSampleRate = 48000; //We either capture at 48Khz (preferred) or resample to it.

        private readonly Dictionary<SupportedWaveFormat, WaveFormat> _inputFormatPriorities = new()
        {
            {SupportedWaveFormat.WAVE_FORMAT_48M16, new WaveFormat(48000, 16, 1)}, //48 kHz, Mono, 16-bit (strongly preferred due to Opus codec)
            {SupportedWaveFormat.WAVE_FORMAT_48S16, new WaveFormat(48000, 16, 2)}, //48 kHz, Stereo, 16-bit
            {SupportedWaveFormat.WAVE_FORMAT_44M16, new WaveFormat(44100, 16, 1)}, //44.1 kHz, Mono, 16-bit
            {SupportedWaveFormat.WAVE_FORMAT_44S16, new WaveFormat(44100, 16, 2)}, //44.1 kHz, Stereo, 16-bit
            {SupportedWaveFormat.WAVE_FORMAT_96M16, new WaveFormat(96000, 16, 1)}, //96 kHz, Mono, 16-bit
            {SupportedWaveFormat.WAVE_FORMAT_96S16, new WaveFormat(96000, 16, 2)}, //96 kHz, Stereo, 16-bit
            {SupportedWaveFormat.WAVE_FORMAT_2M16, new WaveFormat(22050, 16, 1)}, //22.05 kHz, Mono, 16-bit
            {SupportedWaveFormat.WAVE_FORMAT_2S16, new WaveFormat(22050, 16, 2)}, //22.05 kHz, Stereo, 16-bit
            {SupportedWaveFormat.WAVE_FORMAT_1M16, new WaveFormat(11025, 16, 1)}, //11.025 kHz, Mono, 16-bit
            {SupportedWaveFormat.WAVE_FORMAT_1S16, new WaveFormat(11025, 16, 2)}, //11.025 kHz, Stereo, 16-bit
        };

        public AudioPump(int inputDeviceIndex, int outputDeviceIndex, int bitRate)
        {
            _bitRate = bitRate;
            _inputDeviceIndex = inputDeviceIndex;
            _outputDeviceIndex = outputDeviceIndex;
        }

        public void IngestFrame(byte[] bytes, int byteCount)
        {
            if (byteCount <= 0) return;
            _playbackBuffer.Enqueue(bytes.Take(byteCount).ToArray());
            _ingestionEvent.Set();
        }

        public void Stop()
        {
            _keepRunning = false;

            while (_IsCaptureRunning || _IsPlaybackRunning)
            {
                Thread.Sleep(10);
            }
        }

        public void StartCapture()
        {
            _keepRunning = true;

            new Thread(() => //Audio capture thread.
            {
                WaveFormat? bestSupportedInputFormat = null;

                var inputDeviceCapabilities = WaveInEvent.GetCapabilities(_inputDeviceIndex);
                foreach (var inputFormat in _inputFormatPriorities)
                {
                    if (inputDeviceCapabilities.SupportsWaveFormat(inputFormat.Key))
                    {
                        bestSupportedInputFormat = inputFormat.Value;
                        break;
                    }
                }

                if (bestSupportedInputFormat == null)
                {
                    throw new Exception("No acceptable capture rates are supported.");
                }

                WaveFormat? captureResampleFormat = null;
                if (bestSupportedInputFormat.SampleRate != CaptureSampleRate || bestSupportedInputFormat.Channels != 1)
                {
                    //Opus only supports 8/12/16/24/48 Khz, so if the mic does not support 48Khz mono then we will resample the input.
                    captureResampleFormat = new WaveFormat(CaptureSampleRate, 16, 1);
                }

                var waveIn = new WaveInEvent
                {
                    WaveFormat = bestSupportedInputFormat,
                    BufferMilliseconds = 18, //Should be equal or less than the Opus Codec ms per frameSize.
                    DeviceNumber = _inputDeviceIndex
                };

                var encoder = OpusCodecFactory.CreateEncoder(CaptureSampleRate, 1, OpusApplication.OPUS_APPLICATION_VOIP);
                encoder.Bitrate = _bitRate;
                encoder.ForceMode = OpusMode.MODE_SILK_ONLY;
                encoder.SignalType = OpusSignal.OPUS_SIGNAL_VOICE;
                encoder.Complexity = 9;

                int frameSize = (CaptureSampleRate / 1000) * 20; //should be greater than WaveInEvent.BufferMilliseconds).
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
                            pcmShorts.AsSpan(i, frameSize), frameSize, encodedFrameBytes.AsSpan(), encodedFrameBytes.Length);

                        OnFrameProduced?.Invoke(encodedFrameBytes, encodedFrameByteCount);
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
        }

        public void StartPlayback()
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

                var decoder = OpusCodecFactory.CreateDecoder(CaptureSampleRate, 1);
                var transmissionWaveFormat = new WaveFormat(CaptureSampleRate, 1);
                var decodedPcm = new short[CaptureSampleRate];
                int frameSize = CaptureSampleRate / 50; //20ms frame size (1000/50) = 20.

                while (_keepRunning)
                {
                    if (_playbackBuffer.TryDequeue(out var bytes) && bytes != null)
                    {
                        int decodedSamples = decoder.Decode(bytes.AsSpan(), decodedPcm.AsSpan(), frameSize);
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
