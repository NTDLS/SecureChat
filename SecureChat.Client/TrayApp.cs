using SecureChat.Library;
using static SecureChat.Library.Constants;

namespace SecureChat.Client
{
    class TrayApp : ApplicationContext
    {
        private NotifyIcon _trayIcon;

        public TrayApp()
        {

            _trayIcon = new NotifyIcon
            {
                Icon = Icon.ExtractIcon(Application.ExecutablePath, 0, 16),
                Text = Constants.AppName,
                Visible = true,
                ContextMenuStrip = new ContextMenuStrip()
            };

            //_trayIcon.BalloonTipTitle = "Notification";
            //_trayIcon.BalloonTipText = "Hello from the system tray!";
            //_trayIcon.ShowBalloonTip(3000);

            SetTrayIcon(OnlineStatus.Online);

            _trayIcon.ContextMenuStrip.Items.Add("Exit", null, OnExit);
        }


        static Icon LoadIconFromResources(byte[] iconData)
        {
            using var stream = new MemoryStream(iconData);
            return new Icon(stream);
        }

        void SetTrayIcon(OnlineStatus state)
        {
            _trayIcon.Icon = state switch
            {
                OnlineStatus.Online => LoadIconFromResources(Properties.Resources.Online16),
                OnlineStatus.Offline => LoadIconFromResources(Properties.Resources.Offline16),
                _ => throw new NotImplementedException()
            };
        }

        private void OnExit(object? sender, EventArgs e)
        {
            _trayIcon.Visible = false;
            Application.Exit();
        }
    }
}
