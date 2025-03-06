using Microsoft.Extensions.Configuration;
using SecureChat.Client.Forms;
using SecureChat.Client.Models;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using CSCore;
using CSCore.SoundIn;
using CSCore.SoundOut;
using CSCore.Streams;
using CSCore.CoreAudioAPI;

namespace SecureChat.Client
{
    static class VoiceChat
    {         
        [STAThread]
        static void Main()
        {
   
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormVoicePreCall());

            return;

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            //Create a default persisted state if one does not exist.
            Settings.Save();

            Application.Run(new TrayApp());

        }

        /*
        static WasapiCapture? capture;
        static WasapiOut? playback;
        static SoundInSource? soundInSource;
        static WriteableBufferingSource? bufferSource;

        static void Main()
        {
            using var deviceEnumerator = new MMDeviceEnumerator();
            var devices = deviceEnumerator.EnumAudioEndpoints(DataFlow.Capture, DeviceState.Active);

            capture = new WasapiCapture();
            capture.WaveFormat = new WaveFormat(48000, 16, 2); // 48kHz, 16-bit, Stereo
            capture.Device = devices[0];
            
            capture.Initialize();

            soundInSource = new SoundInSource(capture) { FillWithZeros = false };
            bufferSource = new WriteableBufferingSource(soundInSource.WaveFormat);

            soundInSource.DataAvailable += (s, e) =>
            {
                bufferSource.Write(e.Data, 0, e.ByteCount);
            };

            playback = new WasapiOut();
            playback.Initialize(bufferSource);


            capture.Start();
            playback.Play();

            Console.WriteLine("Capturing and playing audio. Press ENTER to stop...");
            Console.ReadLine();

            // Cleanup resources
            playback.Stop();
            capture.Stop();
            playback.Dispose();
            capture.Dispose();
        }
        */
    }
}
