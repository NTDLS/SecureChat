using Krypton.Toolkit;
using Microsoft.Extensions.Configuration;
using Talkster.Client.Helpers;
using Talkster.Library;

namespace Talkster.Client
{
    static class Program
    {
        public static KryptonManager ThemeManager = new KryptonManager();

        public static Logger Log { get; set; } = new Logger();

        [STAThread]
        static void Main()
        {
            var mutex = new Mutex(true, ScConstants.MutexName, out var createdNewMutex);
            if (!createdNewMutex)
            {
                TrayApp.IsOnlyInstance = false;
#if !DEBUG
                MessageBox.Show("Another instance is already running.", ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
#endif
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .Build();

            Settings.Save(); //Create a default persisted state if one does not exist.

            ThemeManager.GlobalPaletteMode = Settings.Instance.Theme;

            Notifications.InitializeToast();

            Application.Run(new TrayApp());

            if (createdNewMutex)
            {
                mutex.ReleaseMutex();
            }
        }
    }
}
