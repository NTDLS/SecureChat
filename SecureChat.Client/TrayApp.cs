using SecureChat.Client.Forms;
using SecureChat.Library;
using static SecureChat.Library.Constants;

namespace SecureChat.Client
{
    class TrayApp : ApplicationContext
    {
        private readonly NotifyIcon _trayIcon;

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

            //SetTrayIcon(ScOnlineStatus.Online);

            _trayIcon.ContextMenuStrip.Items.Add("Exit", null, OnExit);

            using var formLogin = new FormLogin();
            formLogin.ShowDialog();
        }

        static Icon LoadIconFromResources(byte[] iconData)
        {
            using var stream = new MemoryStream(iconData);
            return new Icon(stream);
        }

        void SetTrayIcon(ScOnlineStatus state)
        {
            _trayIcon.Icon = state switch
            {
                ScOnlineStatus.Online => LoadIconFromResources(Properties.Resources.Online16),
                ScOnlineStatus.Offline => LoadIconFromResources(Properties.Resources.Offline16),
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
