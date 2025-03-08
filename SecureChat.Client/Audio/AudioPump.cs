using Concentus.Structs;
using Concentus.Enums;

using NAudio.CoreAudioApi;
using NAudio.Wave;
using NTDLS.Helpers;
using System.Xml.Linq;
using Concentus;
using System;

namespace SecureChat.Client.Audio
{
    internal class AudioPump
    {
        public delegate void VolumeSampleEventHandler(float volume);
        public event VolumeSampleEventHandler? OnInputSample;

        public delegate void FrameProducedEventHandler(byte[] bytes, int byteCount);
        public event FrameProducedEventHandler? OnFrameProduced;

        private readonly int _outputDeviceIndex;
        private readonly int _inputDeviceIndex;

        private bool _IsRunning = false;
        private bool _keepRunning = false;

        public bool Mute { get; set; } = false;
        public float Gain { get; set; } = 1.0f;
        public int SampleRate { get; private set; }

        public AudioPump(int inputDeviceIndex, int outputDeviceIndex, int sampleRate)
        {
            SampleRate = sampleRate;
            _inputDeviceIndex = inputDeviceIndex;
            _outputDeviceIndex = outputDeviceIndex;
        }

        public void Start()
        {
            _keepRunning = true;

            new Thread(() => //Audio capture thread.
            {
                var enumerator = new MMDeviceEnumerator();

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
                    WaveFormat = new WaveFormat(inputWaveFormat.SampleRate, 16, 1),
                    //If the mic will allow 16bit mono, then lets just roll with it.
                    //WaveFormat = new WaveFormat(inputWaveFormat.SampleRate, inputWaveFormat.BitsPerSample, inputWaveFormat.Channels),
                    BufferMilliseconds = 75,
                    DeviceNumber = _inputDeviceIndex
                };

                Console.WriteLine($"Input: {waveIn.WaveFormat.SampleRate} Hz, {waveIn.WaveFormat.BitsPerSample}-bit, {waveIn.WaveFormat.Channels}ch");

                var encoder = OpusCodecFactory.CreateEncoder(waveIn.WaveFormat.SampleRate, waveIn.WaveFormat.Channels, OpusApplication.OPUS_APPLICATION_VOIP);
                encoder.ForceChannels = 1;
                encoder.Bitrate = SampleRate;

                var pcmBuffer = new List<short>(); // Buffer for leftover PCM data

                waveIn.DataAvailable += (sender, e) =>
                {
                    if (Mute) return;

                    var pcmShorts = ConvertPcmBytesToShorts(e.Buffer, e.BytesRecorded);

                    // Prepend leftover PCM from previous buffer to the new PCM data
                    if (pcmBuffer.Count > 0)
                    {
                        pcmShorts = pcmBuffer.Concat(pcmShorts).ToArray();
                        pcmBuffer.Clear();
                    }

                    int frameSize = waveIn.WaveFormat.SampleRate / 50; // 20ms frame size

                    // Process PCM in frameSize-sample chunks
                    byte[] opusData = new byte[1275]; // Max Opus packet size
                    int i = 0;
                    for (; i + frameSize <= pcmShorts.Length; i += frameSize)
                    {
                        int encodedBytes = encoder.Encode(new ReadOnlySpan<short>(pcmShorts, i, frameSize), frameSize, new Span<byte>(opusData), opusData.Length);

                        OnFrameProduced?.Invoke(opusData, encodedBytes);

                        Console.WriteLine($"Encoded {encodedBytes} bytes at {encoder.Bitrate}bps for frame {i / 960}");

                        // Transmit encoded data...
                    }

                    // Store leftover samples (PREPEND to next buffer)
                    if (i < pcmShorts.Length)
                    {
                        pcmBuffer.AddRange(pcmShorts.Skip(i));
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
                    var enumerator = new MMDeviceEnumerator();
                    var outputDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToList();

                    var waveOut = new WasapiOut(outputDevices[_outputDeviceIndex], AudioClientShareMode.Shared, true, 50);
                    var outputBufferStream = new BufferedWaveProvider(waveOut.OutputWaveFormat)
                    {
                        BufferLength = 64 * 1024,
                        DiscardOnBufferOverflow = true
                    };

                    Console.WriteLine($"Output: {waveOut.OutputWaveFormat.SampleRate} Hz, {waveOut.OutputWaveFormat.BitsPerSample}-bit, {waveOut.OutputWaveFormat.Channels}ch");

                    /*
                    var encoder = OpusCodecFactory.CreateEncoder(waveIn.WaveFormat.SampleRate, waveIn.WaveFormat.Channels, OpusApplication.OPUS_APPLICATION_VOIP);
                    encoder.ForceChannels = 1;
                    encoder.Bitrate = SampleRate;
                    */

                    waveOut.Init(outputBufferStream);
                    waveOut.Play();
                    _IsRunning = true;

                    while (_keepRunning)
                    {
                        Thread.Sleep(100);
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

        static short[] ConvertPcmBytesToShorts(byte[] pcmBytes, int length)
        {
            short[] pcmShorts = new short[length / 2]; // 2 bytes per sample

            for (int i = 0; i < pcmShorts.Length; i++)
            {
                pcmShorts[i] = (short)(pcmBytes[i * 2] | (pcmBytes[i * 2 + 1] << 8));
            }

            return pcmShorts;
        }

        static byte[] ConvertShortsToPcmBytes(short[] pcmShorts)
        {
            byte[] pcmBytes = new byte[pcmShorts.Length * 2];

            for (int i = 0; i < pcmShorts.Length; i++)
            {
                pcmBytes[i * 2] = (byte)(pcmShorts[i] & 0xFF);
                pcmBytes[i * 2 + 1] = (byte)((pcmShorts[i] >> 8) & 0xFF);
            }

            return pcmBytes;
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
