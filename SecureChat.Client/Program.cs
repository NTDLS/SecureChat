namespace SecureChat.Client
{
    static class VoiceChat
    {

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TrayApp()); // No visible form!
        }

        /*
        static void Main()
        {
            using var formLogin = new FormLogin();
            formLogin.ShowDialog();
        }
        */

        /*
         using NAudio.CoreAudioApi;
         using NAudio.Wave;

        private static WaveInEvent? waveIn;
        private static WasapiOut? waveOut;
        private static BufferedWaveProvider? bufferStream;
        private static ResamplerDmoStream? resampler;

        static void Main()
        { 
        Console.WriteLine("\nAvailable Playback Devices:");
        var enumerator = new MMDeviceEnumerator();
        var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

        int index = 0;
        foreach (var device in devices)
        {
            Console.WriteLine($"[{index}] {device.FriendlyName}");
            index++;
        }

        Console.WriteLine("Available Input Devices:");
        for (int device = 0; device < WaveInEvent.DeviceCount; device++)
        {
            var capabilities = WaveInEvent.GetCapabilities(device);
            Console.WriteLine($"Input Device {device}: {capabilities.ProductName}");
        }


        Console.Write("\nSelect playback device index: ");
        int selectedIndex = int.Parse(Console.ReadLine());
        var selectedDevice = devices[selectedIndex];

        Console.Write("\nSelect input device index: ");
        int inputDeviceIndex = int.Parse(Console.ReadLine());

        Console.WriteLine($"Using Input: {WaveInEvent.GetCapabilities(inputDeviceIndex).ProductName}");
        Console.WriteLine($"Using Output: {selectedDevice.FriendlyName}");

        StartVoiceChat(selectedDevice, inputDeviceIndex);

        Console.WriteLine("Press any key to stop...");
        Console.ReadKey();

        StopVoiceChat();
    }

    static void StartVoiceChat(MMDevice outputDevice, int inputDeviceIndex)
    {
        waveIn = new WaveInEvent
        {
            WaveFormat = new WaveFormat(44100, 16, 1), // 44.1kHz, 16-bit, Mono
            BufferMilliseconds = 50,
            DeviceNumber = inputDeviceIndex
        };

        bufferStream = new BufferedWaveProvider(waveIn.WaveFormat)
        {
            DiscardOnBufferOverflow = true
        };

        waveOut = new WasapiOut(outputDevice, AudioClientShareMode.Shared, false, 50);

        var sampler = new ResamplerDmoStream(bufferStream, waveOut.OutputWaveFormat);

        waveIn.DataAvailable += (sender, e) =>
        {
            bufferStream.AddSamples(e.Buffer, 0, e.BytesRecorded);
        };

        waveOut.Init(sampler);
        waveIn.StartRecording();
        waveOut.Play();
    }

    static void StopVoiceChat()
    {
        waveIn?.StopRecording();
        waveOut?.Stop();
        waveIn?.Dispose();
        waveOut?.Dispose();
        resampler?.Dispose();
    }

         */
    }
}
