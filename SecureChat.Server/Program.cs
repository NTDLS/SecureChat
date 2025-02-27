using Microsoft.Extensions.Configuration;
using NTDLS.SqliteDapperWrapper;
using Serilog;

namespace SecureChat.Server
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

            var sqliteConnection = configuration.GetValue<string>("AppSettings:SQLiteConnection");

            var factory = new ManagedDataStorageFactory($"Data Source={sqliteConnection}");

            HostFactory.Run(x =>
            {
                x.StartAutomatically();

                x.EnableServiceRecovery(rc =>
                {
                    rc.RestartService(1);
                });

                x.Service<QueuingService>(s =>
                {
                    s.ConstructUsing(hostSettings => new QueuingService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("A high-performance and reliable persistent message queue designed for efficient inter-process communication, task queuing, load balancing, and data buffering over TCP/IP.");
                x.SetDisplayName("CatMQ Message Queuing");
                x.SetServiceName("CatMQService");
            });
        }
    }
}
