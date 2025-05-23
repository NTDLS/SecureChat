﻿using System.Media;
using Talkster.Client.Forms;
using Talkster.Client.Properties;
using Talkster.Library;
using static Talkster.Client.Forms.FormToast;

namespace Talkster.Client.Helpers
{
    public static class Notifications
    {
        public enum ToastPosition
        {
            TopLeft,
            TopRight,
            TopCenter,
            BottomLeft,
            BottomRight,
            BottomCenter
        }

        public enum ToastStyle
        {
            None,
            Error,
            Success,
            Warning
        }

        public static void ContactOnline(string contactName)
        {
            if (Settings.Instance.PlaySoundWhenContactComesOnline)
            {
                using var player = new SoundPlayer(Resources.AudioContactOnline);
                player.Play();
            }
            if (Settings.Instance.AlertToastWhenContactComesOnline && ServerConnection.Current != null)
            {
                ToastPlain(ScConstants.AppName, $"{contactName} is now online.");
            }
        }

        public static void MessageReceived(string contactName, Form formToActive)
        {
            if (Settings.Instance.PlaySoundWhenMessageReceived)
            {
                using var player = new SoundPlayer(Resources.AudioMessageReceived);
                player.Play();
            }
            if (Settings.Instance.AlertToastWhenMessageReceived && ServerConnection.Current != null)
            {
                ToastPlain(ScConstants.AppName, $"{contactName} sent you a message.", () =>
                    {
                        formToActive.Invoke(() =>
                        {
                            formToActive.Activate();
                            formToActive.BringToFront();
                            if (formToActive.WindowState != FormWindowState.Maximized)
                            {
                                formToActive.WindowState = FormWindowState.Normal;
                            }
                            formToActive.Focus();
                        });
                    });
            }
        }

        public static void IncomingCall(string contactName, Form formToActive)
        {
            if (Settings.Instance.PlaySoundWhenMessageReceived)
            {
                using var player = new SoundPlayer(Resources.AudioIncommingCall);
                player.Play();
            }
            if (Settings.Instance.AlertToastWhenMessageReceived && ServerConnection.Current != null)
            {
                ToastPlain(ScConstants.AppName, $"{contactName} is calling you.", () =>
                    {
                        formToActive.Invoke(() =>
                        {
                            formToActive.Activate();
                            formToActive.BringToFront();
                            if (formToActive.WindowState != FormWindowState.Maximized)
                            {
                                formToActive.WindowState = FormWindowState.Normal;
                            }
                            formToActive.Focus();
                        });
                    });
            }
        }

        #region Toast.

        private static FormToast? _notificationForm;
        private static Thread? _thread;

        public static void InitializeToast()
        {
            if (_notificationForm == null && _thread == null)
            {
                _thread = new Thread(() =>
                {
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.EnableVisualStyles();
                    SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());

                    _notificationForm = new FormToast();
                    _ = _notificationForm.Handle;
                    Application.Run();
                });

                _thread.SetApartmentState(ApartmentState.STA);
                _thread.IsBackground = true;
                _thread.Start();

                while (_notificationForm == null)
                {
                    Thread.Sleep(10);
                }
            }
        }

        public static void ToastSuccess(string headerText, string bodyText, ToastClickActionParameterized action, object actionParameter, int? duration = null, ToastPosition position = ToastPosition.BottomRight)
            => _notificationForm?.InvokePopup(ToastStyle.Success, headerText, bodyText, action, actionParameter, duration ?? Settings.Instance.ToastTimeoutSeconds * 1000, position);
        public static void ToastSuccess(string headerText, string bodyText, ToastClickAction action, int? duration = null, ToastPosition position = ToastPosition.BottomRight)
            => _notificationForm?.InvokePopup(ToastStyle.Success, headerText, bodyText, action, duration ?? Settings.Instance.ToastTimeoutSeconds * 1000, position);
        public static void ToastSuccess(string headerText, string bodyText, int? duration = null, ToastPosition position = ToastPosition.BottomRight)
            => _notificationForm?.InvokePopup(ToastStyle.Success, headerText, bodyText, null, duration ?? Settings.Instance.ToastTimeoutSeconds * 1000, position);

        public static void ToastWarning(string headerText, string bodyText, ToastClickActionParameterized action, object actionParameter, int? duration = null, ToastPosition position = ToastPosition.BottomRight)
            => _notificationForm?.InvokePopup(ToastStyle.Warning, headerText, bodyText, action, actionParameter, duration ?? Settings.Instance.ToastTimeoutSeconds * 1000, position);
        public static void ToastWarning(string headerText, string bodyText, ToastClickAction action, int? duration = null, ToastPosition position = ToastPosition.BottomRight)
            => _notificationForm?.InvokePopup(ToastStyle.Warning, headerText, bodyText, action, duration ?? Settings.Instance.ToastTimeoutSeconds * 1000, position);
        public static void ToastWarning(string headerText, string bodyText, int? duration = null, ToastPosition position = ToastPosition.BottomRight)
            => _notificationForm?.InvokePopup(ToastStyle.Warning, headerText, bodyText, null, duration ?? Settings.Instance.ToastTimeoutSeconds * 1000, position);

        public static void ToastError(string headerText, string bodyText, ToastClickActionParameterized action, object actionParameter, int? duration = null, ToastPosition position = ToastPosition.BottomRight)
            => _notificationForm?.InvokePopup(ToastStyle.Error, headerText, bodyText, action, actionParameter, duration ?? Settings.Instance.ToastTimeoutSeconds * 1000, position);
        public static void ToastError(string headerText, string bodyText, ToastClickAction action, int? duration = null, ToastPosition position = ToastPosition.BottomRight)
            => _notificationForm?.InvokePopup(ToastStyle.Error, headerText, bodyText, action, duration ?? Settings.Instance.ToastTimeoutSeconds * 1000, position);
        public static void ToastError(string headerText, string bodyText, int? duration = null, ToastPosition position = ToastPosition.BottomRight)
            => _notificationForm?.InvokePopup(ToastStyle.Error, headerText, bodyText, null, duration ?? Settings.Instance.ToastTimeoutSeconds * 1000, position);

        public static void ToastPlain(string headerText, string bodyText, ToastClickActionParameterized action, object actionParameter, int? duration = null, ToastPosition position = ToastPosition.BottomRight)
            => _notificationForm?.InvokePopup(ToastStyle.None, headerText, bodyText, action, actionParameter, duration ?? Settings.Instance.ToastTimeoutSeconds * 1000, position);
        public static void ToastPlain(string headerText, string bodyText, ToastClickAction action, int? duration = null, ToastPosition position = ToastPosition.BottomRight)
            => _notificationForm?.InvokePopup(ToastStyle.None, headerText, bodyText, action, duration ?? Settings.Instance.ToastTimeoutSeconds * 1000, position);
        public static void ToastPlain(string headerText, string bodyText, int? duration = null, ToastPosition position = ToastPosition.BottomRight)
            => _notificationForm?.InvokePopup(ToastStyle.None, headerText, bodyText, null, duration ?? Settings.Instance.ToastTimeoutSeconds * 1000, position);

        #endregion
    }
}
