using Microsoft.Extensions.Configuration;
using SecureChat.Library;
using Serilog;
using System.Text;
using Topshelf;

namespace SecureChat.Server
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // Generate RSA Key Pair
            var keyPair = Crypto.GeneratePublicPrivateKeyPair();

            // Encrypt the data
            byte[] encryptedData = Crypto.RsaEncryptBytes(Encoding.UTF8.GetBytes("Hello, world!"), keyPair.PublicRsaKey);

            // Decrypt the data
            byte[] decryptedData = Crypto.RsaDecryptBytes(encryptedData, keyPair.PrivateRsaKey);
            Console.WriteLine(Encoding.UTF8.GetString(decryptedData)); // Should print "Hello, world!"


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
                x.SetDisplayName("Secure Chat Server");
                x.SetServiceName("SecureChatServer");
            });
        }
    }
}
