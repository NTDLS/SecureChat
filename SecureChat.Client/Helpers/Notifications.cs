using SecureChat.Client.Properties;
using System.Media;

namespace SecureChat.Client.Helpers
{
    public static class Notifications
    {
        public static void ContactOnline(string contactName)
        {
            if (Settings.Instance.PlaySoundWhenContactComesOnline)
            {
                using var player = new SoundPlayer(Resources.AudioContactOnline);
                player.Play();
            }
            if (Settings.Instance.AlertToastWhenContactComesOnline && ServerConnection.Current != null)
            {
                ServerConnection.Current.Tray.ShowBalloon("", $"{contactName} is now online.");
            }
        }

        public static void MessageReceived(string contactName)
        {
            if (Settings.Instance.PlaySoundWhenMessageReceived)
            {
                using var player = new SoundPlayer(Resources.AudioMessageReceived);
                player.Play();
            }
            if (Settings.Instance.AlertToastWhenMessageReceived && ServerConnection.Current != null)
            {
                ServerConnection.Current.Tray.ShowBalloon("", $"{contactName} sent you a message.");
            }
        }

        public static void IncomingCall(string contactName)
        {
            if (Settings.Instance.PlaySoundWhenMessageReceived)
            {
                using var player = new SoundPlayer(Resources.AudioIncommingCall);
                player.Play();
            }
            if (Settings.Instance.AlertToastWhenMessageReceived && ServerConnection.Current != null)
            {
                ServerConnection.Current.Tray.ShowBalloon("", $"{contactName} is calling you.");
            }
        }
    }
}
