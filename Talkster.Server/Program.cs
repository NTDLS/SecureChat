using Microsoft.Extensions.Configuration;
using Serilog;
using Topshelf;

namespace Talkster.Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            HostFactory.Run(x =>
            {
                x.StartAutomatically();

                x.EnableServiceRecovery(rc =>
                {
                    rc.RestartService(1);
                });

                x.Service<ChatService>(s =>
                {
                    s.ConstructUsing(hostSettings => new ChatService(configuration));
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("Server for end-to-end encryption chat.");
                x.SetDisplayName("Talkster Server");
                x.SetServiceName("TalksterServer");
            });
        }
    }
}
