﻿using Microsoft.Extensions.Configuration;
using SecureChat.Client.Forms;
using SecureChat.Client.Models;
using Serilog;

namespace SecureChat.Client
{
    static class VoiceChat
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FormVoicePreCall());

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
    }
}
