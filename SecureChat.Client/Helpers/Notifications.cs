using SecureChat.Client.Models;
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
                using var stream = new MemoryStream(Resources.ContactOnline);
                if (stream != null)
                {
                    using var player = new SoundPlayer(stream);
                    player.Play();
                }
            }
            if (Settings.Instance.AlertToastWhenContactComesOnline && LocalSession.Current != null)
            {
                LocalSession.Current.Tray.ShowBalloon("", $"{contactName} is now online.");
            }
        }

        public static void MessageReceived(string contactName)
        {
            if (Settings.Instance.PlaySoundWhenMessageReceived)
            {
                using var stream = new MemoryStream(Resources.MessageReceived);
                if (stream != null)
                {
                    using var player = new SoundPlayer(stream);
                    player.Play();
                }
            }
            if (Settings.Instance.AlertToastWhenMessageReceived && LocalSession.Current != null)
            {
                LocalSession.Current.Tray.ShowBalloon("", $"{contactName} sent you a message.");
            }
        }

        public static void IncomingCall(string contactName)
        {
            if (Settings.Instance.PlaySoundWhenMessageReceived)
            {
                using var stream = new MemoryStream(Resources.IncomingCall);
                if (stream != null)
                {
                    using var player = new SoundPlayer(stream);
                    player.Play();
                }
            }
            if (Settings.Instance.AlertToastWhenMessageReceived && LocalSession.Current != null)
            {
                LocalSession.Current.Tray.ShowBalloon("", $"{contactName} is calling you.");
            }
        }
    }
}
