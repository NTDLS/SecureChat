using CSCore.CoreAudioAPI;
using CSCore;
using CSCore.SoundIn;
using CSCore.SoundOut;
using CSCore.Streams;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace SecureChat.Client.Audio
{
    internal class AudioPump
    {
        WasapiCapture? capture;
        WasapiOut? playback;
        SoundInSource? soundInSource;
        WriteableBufferingSource? bufferSource;

        private readonly string _outputDeviceId;
        private readonly string _inputDeviceId;

        private Thread? _thread;

        public AudioPump(string inputDeviceId, string outputDeviceId)
        {
            _inputDeviceId = inputDeviceId;
            _outputDeviceId = outputDeviceId;
        }

        public void Start()
        {
            using var deviceEnumerator = new MMDeviceEnumerator();
            var inputDevice = deviceEnumerator.EnumAudioEndpoints(DataFlow.Capture, DeviceState.Active)
                .FirstOrDefault(o => o.DeviceID == _inputDeviceId);

            var outputDevice = deviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active)
                .FirstOrDefault(o => o.DeviceID == _outputDeviceId);

            _thread = new Thread(() =>
            {
                capture = new WasapiCapture(/*specify some params*/);
                capture.Device = inputDevice;
                capture.Initialize();

                soundInSource = new SoundInSource(capture) { FillWithZeros = false };
                bufferSource = new WriteableBufferingSource(soundInSource.WaveFormat);

                soundInSource.DataAvailable += (s, e) =>
                {
                    bufferSource.Write(e.Data, 0, e.ByteCount);
                };

                playback = new WasapiOut();
                playback.Device = outputDevice;
                playback.Initialize(bufferSource);


                capture.Start();
                playback.Play();

                while (true)
                {
                    Thread.Sleep(1000);
                }

                
                // Cleanup resources
                //playback.Stop();
                //capture.Stop();
                //playback.Dispose();
                //capture.Dispose();
            });

            _thread.Start();
        }

        /*
                private WaveInEvent? waveIn;
                private WaveOutEvent? waveOut;
                private BufferedWaveProvider? bufferStream;
                private readonly int _outputDeviceIndex;
                private readonly int _inputDeviceIndex;
                private Thread? _thread;

                public bool Mute { get; set; } = false;
                public double Volume { get; set; } = 0.25;
                public int BitRate { get; private set; } = 44100;

                public delegate void AmplitudeReportEventHandler(float amplitude);
                public event AmplitudeReportEventHandler? OnAmplitudeReport;

                public AudioPump(int inputDeviceIndex, int outputDeviceIndex, int bitRate = 44100)
                {
                    BitRate = bitRate;
                    _outputDeviceIndex = outputDeviceIndex;
                    _inputDeviceIndex = inputDeviceIndex;
                }

                public void Start()
                {
                    _stopped = false;

                    _thread = new Thread(() =>
                    {
                        try
                        {
                            waveIn = new WaveInEvent
                            {
                                WaveFormat = new WaveFormat(BitRate, 16, 1), // 44.1kHz, 16-bit, Mono
                                BufferMilliseconds = 50,
                                DeviceNumber = _inputDeviceIndex
                            };

                            bufferStream = new BufferedWaveProvider(waveIn.WaveFormat)
                            {
                                DiscardOnBufferOverflow = true
                            };

                            waveOut = new WaveOutEvent()
                            {
                                DeviceNumber = _outputDeviceIndex,
                                DesiredLatency = 100
                            };

                            waveIn.DataAvailable += (sender, e) =>
                            {
                                bufferStream.AddSamples(e.Buffer, 0, e.BytesRecorded);
                            };


                            waveOut.Init(bufferStream);
                            waveIn.StartRecording();
                            waveOut.Play();


                            while (_keepRunning)
                            {
                                Thread.Sleep(100);
                            }

                            while (_playbackRunning || _recordingRunning || _buffering)
                            {
                                Thread.Sleep(100);
                            }

                            Exceptions.Ignore(() => waveIn.Dispose());
                            Exceptions.Ignore(() => waveOut.Dispose());

                            _stopped = true;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    });

                    _thread.Start();
                }

                bool _stopped = true;
                bool _keepRunning = true;
                bool _recordingRunning = false;
                bool _playbackRunning = false;
                bool _buffering = false;

                public void Stop()
                {
                    Exceptions.Ignore(() => waveIn?.StopRecording());
                    Exceptions.Ignore(() => waveOut?.Stop());

                    _keepRunning = false;

                    while (!_stopped)
                    {
                        Thread.Sleep(100);
                    }

                    _thread = null;
                }

                /// <summary>
                // Calculate volume level from raw PCM data
                /// </summary>
                private static float CalculateVolumeLevel(byte[] buffer, int bytesRead)
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
        */
    }

}
