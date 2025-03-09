using Concentus.Structs;
using Concentus.Enums;

using NAudio.CoreAudioApi;
using NAudio.Wave;
using NTDLS.Helpers;
using System.Xml.Linq;
using Concentus;
using System;
using System.Text;
using System.Threading.Channels;
using System.Drawing.Imaging;

namespace SecureChat.Client.Audio
{
    internal class AudioPump
    {
        public delegate void VolumeSampleEventHandler(float volume);
        public event VolumeSampleEventHandler? OnInputSample;

        public delegate void FrameProducedEventHandler(byte[] bytes);
        public event FrameProducedEventHandler? OnFrameProduced;

        private readonly int _outputDeviceIndex;
        private readonly int _inputDeviceIndex;

        private bool _IsRunning = false;
        private bool _keepRunning = false;

        public bool Mute { get; set; } = false;
        public float Gain { get; set; } = 1.0f;
        public int SampleRate { get; private set; }
        public int FrameSize { get; private set; }

        public AudioPump(int inputDeviceIndex, int outputDeviceIndex, int sampleRate)
        {
            SampleRate = sampleRate;
            _inputDeviceIndex = inputDeviceIndex;
            _outputDeviceIndex = outputDeviceIndex;
        }

        private readonly Queue<byte[]> _playbackBuffer = new();

        public void IngestFrame(byte[] bytes)
        {
            lock (_playbackBuffer)
            {
                _playbackBuffer.Enqueue(bytes);
            }
        }

        public void Start()
        {
            _keepRunning = true;

            int transmissionSampleRate = 0; //Should come from client.
            int transmissionChannelCount = 0; //Should come from client.

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

                transmissionSampleRate = nativeInputDeviceWaveFormat.SampleRate; //Should come from client.
                transmissionChannelCount = 1; //Should come from client.

                var waveIn = new WaveInEvent
                {
                    WaveFormat = new WaveFormat(transmissionSampleRate, 16, transmissionChannelCount),
                    //If the mic will allow 16bit mono, then lets just roll with it.
                    //WaveFormat = new WaveFormat(inputWaveFormat.SampleRate, inputWaveFormat.BitsPerSample, inputWaveFormat.Channels),
                    BufferMilliseconds = 20,
                    DeviceNumber = _inputDeviceIndex
                };

                FrameSize = (transmissionSampleRate * transmissionChannelCount) / 50; // 20ms frame size
                //byte[] frameBuffer = new byte[FrameSize];

                Console.WriteLine($"Input: {waveIn.WaveFormat.SampleRate} Hz, {waveIn.WaveFormat.BitsPerSample}-bit, {waveIn.WaveFormat.Channels}ch");

                var encoder = OpusCodecFactory.CreateEncoder(transmissionSampleRate, transmissionChannelCount, OpusApplication.OPUS_APPLICATION_VOIP);
                encoder.Bitrate = (96000);
                encoder.ForceMode = (OpusMode.MODE_SILK_ONLY);
                encoder.SignalType = (OpusSignal.OPUS_SIGNAL_VOICE);
                encoder.Complexity = (5);

                var partialBuffer = new List<short>(); // Buffer for leftover PCM data
                var encodedFrameBytes = new byte[1275]; // Max Opus packet size

                waveIn.DataAvailable += (sender, e) =>
                {
                    if (Mute) return;

                    var pcmShorts = BytesToShorts(e.Buffer, 0, e.BytesRecorded);

                    // Prepend leftover PCM from previous buffer to the new PCM data
                    if (partialBuffer.Count > 0)
                    {
                        pcmShorts = partialBuffer.Concat(pcmShorts).ToArray();
                        partialBuffer.Clear();
                    }

                    // Process PCM in frameSize-sample chunks
                    int i = 0;
                    for (; i + FrameSize <= pcmShorts.Length; i += FrameSize)
                    {
                        int encodedFrameByteCount = encoder.Encode(
                            new ReadOnlySpan<short>(pcmShorts, i, FrameSize),
                            FrameSize, new Span<byte>(encodedFrameBytes), encodedFrameBytes.Length);

                        lock (_playbackBuffer)
                        {
                            _playbackBuffer.Enqueue(encodedFrameBytes.Take(encodedFrameByteCount).ToArray());
                        }

                        //OnFrameProduced?.Invoke(encodedFrameBytes.Take(encodedFrameByteCount).ToArray());
                    }

                    // Store leftover samples (PREPEND to next buffer)
                    if (i < pcmShorts.Length)
                    {
                        partialBuffer.AddRange(pcmShorts.Skip(i));
                    }
                };

                waveIn.StartRecording();
                _IsRunning = true;

                while (_keepRunning)
                {
                    Thread.Sleep(100);
                }

                waveIn?.StopRecording();
                waveIn?.Dispose();

                _IsRunning = false;
            }).Start();


            new Thread(() => //Audio playback thread.
            {
                while (transmissionSampleRate == 0 && transmissionChannelCount == 0)
                {
                    Thread.Sleep(1); // This loop is just for debugging.
                }

                var enumerator = new MMDeviceEnumerator();
                var outputDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToList();

                var waveOut = new WasapiOut(outputDevices[_outputDeviceIndex], AudioClientShareMode.Shared, true, 50);
                var outputBufferStream = new BufferedWaveProvider(waveOut.OutputWaveFormat)
                {
                    BufferLength = 64 * 1024,
                    DiscardOnBufferOverflow = true
                };

                Console.WriteLine($"Output: {waveOut.OutputWaveFormat.SampleRate} Hz, {waveOut.OutputWaveFormat.BitsPerSample}-bit, {waveOut.OutputWaveFormat.Channels}ch");

                var decoder = OpusCodecFactory.CreateDecoder(transmissionSampleRate, transmissionChannelCount);

                waveOut.Init(outputBufferStream);
                waveOut.Play();
                _IsRunning = true;

                var transmissionWaveFormat = new WaveFormat(transmissionSampleRate, transmissionChannelCount);

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
                        short[] decodedPcm = new short[transmissionSampleRate * transmissionChannelCount];
                        int decodedSamples = decoder.Decode(new ReadOnlySpan<byte>(bytes), new Span<short>(decodedPcm), FrameSize, false);

                        var pcmBytes = ShortsToBytes(decodedPcm);

                        //Resamples and writes to the audio output device.
                        ResampleForOutput(pcmBytes, transmissionWaveFormat, waveOut.OutputWaveFormat, outputBufferStream);
                    }
                    else
                    {
                        //Thread.Sleep(1);
                    }
                }

                waveOut?.Stop();

                while (waveOut?.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(10);
                }

                waveOut?.Dispose();

                _IsRunning = false;
            }).Start();
        }


        public static short[] BytesToShorts(byte[] input)
        {
            return BytesToShorts(input, 0, input.Length);
        }


        public static short[] BytesToShorts(byte[] input, int offset, int length)
        {
            short[] processedValues = new short[length / 2];
            for (int c = 0; c < processedValues.Length; c++)
            {
                processedValues[c] = (short)(((int)input[(c * 2) + offset]) << 0);
                processedValues[c] += (short)(((int)input[(c * 2) + 1 + offset]) << 8);
            }

            return processedValues;
        }

        public static byte[] ShortsToBytes(short[] input)
        {
            return ShortsToBytes(input, 0, input.Length);
        }

        public static byte[] ShortsToBytes(short[] input, int offset, int length)
        {
            byte[] processedValues = new byte[length * 2];
            for (int c = 0; c < length; c++)
            {
                processedValues[c * 2] = (byte)(input[c + offset] & 0xFF);
                processedValues[c * 2 + 1] = (byte)((input[c + offset] >> 8) & 0xFF);
            }

            return processedValues;
        }

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

        private static int CalculateBufferSize(WaveFormat inputFormat, int milliseconds = 50)
        {
            // Compute buffer size based on sample rate & frame size.
            int bytesPerSample = inputFormat.BitsPerSample / 8 * inputFormat.Channels;
            int bytesPerMs = inputFormat.SampleRate * bytesPerSample / 1000;
            return bytesPerMs * milliseconds;
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

        public static void ResampleForOutput(byte[] inputBytes, WaveFormat inputFormat, WaveFormat outputFormat, BufferedWaveProvider outputBuffer)
            => ResampleForOutput(inputBytes, inputBytes.Length, inputFormat, outputFormat, outputBuffer);

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

        private static float CalculateVolumeLevelWithGain(byte[] buffer)
        {
            short maxSample = 0;
            for (int i = 0; i < buffer.Length; i += 2)
            {
                short sample = (short)(buffer[i] | (buffer[i + 1] << 8));
                maxSample = Math.Max(maxSample, Math.Abs(sample));
            }

            float normalized = maxSample / 32768.0f;
            return Math.Min(1.0f, normalized);
        }
    }
}
