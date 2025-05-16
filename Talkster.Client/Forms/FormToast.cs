using Krypton.Toolkit;
using System.Drawing.Drawing2D;
using static Talkster.Client.Helpers.Notifications;

namespace Talkster.Client.Forms
{
    public partial class FormToast : Form
    {
        public delegate void ToastClickActionParameterized(object? param);
        public delegate void ToastClickAction();

        private int _duration = 0;
        private System.Windows.Forms.Timer _timer = new();
        private DateTime _startTimeUTC;
        private readonly int _cornerRadius = 10;

        private ToastClickActionParameterized? _parameterizedAction;
        private ToastClickAction? _action;
        private object? _actionParameter;

        public FormToast()
        {
            InitializeComponent();

            TopMost = true;
            StartPosition = FormStartPosition.Manual;
            Opacity = 0;
            Padding = new Padding(10);

            _timer.Tick += Timer_Tick;
            _timer.Interval = 10;

            Click += FormToast_Click;
            labelBody.Click += FormToast_Click;
            labelHeader.Click += FormToast_Click;
            pictureBoxIcon.Click += FormToast_Click;
        }

        public void InvokePopup(ToastStyle style, string headerText, string bodyText,
            ToastClickActionParameterized? action, object actionParameter, int duration = 3000, ToastPosition position = ToastPosition.BottomRight)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => InvokePopup(style, headerText, bodyText, action, actionParameter, duration, position)));
                return;
            }

            if (Visible)
            {
                return; //If the toast is already visible, just exit.
            }

            _parameterizedAction = action;
            _actionParameter = actionParameter;
            _action = null;

            Popup(style, headerText, bodyText, duration, position);
        }

        public void InvokePopup(ToastStyle style, string headerText, string bodyText,
            ToastClickAction? action, int duration = 3000, ToastPosition position = ToastPosition.BottomRight)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => InvokePopup(style, headerText, bodyText, action, duration, position)));
                return;
            }

            if (Visible)
            {
                return; //If the toast is already visible, just exit.
            }

            _parameterizedAction = null;
            _actionParameter = null;
            _action = action;

            Popup(style, headerText, bodyText, duration, position);
        }

        private void Popup(ToastStyle style, string headerText, string bodyText, int duration = 3000, ToastPosition position = ToastPosition.BottomRight)
        {
            BackColor = KryptonManager.CurrentGlobalPalette.GetBackColor1(PaletteBackStyle.PanelClient, PaletteState.Normal);
            Opacity = 0;

            _duration = duration;

            labelHeader.ForeColor = KryptonManager.CurrentGlobalPalette.GetContentShortTextColor1(PaletteContentStyle.LabelTitlePanel, PaletteState.Normal);
            labelHeader.BackColor = Color.Transparent;
            labelHeader.Text = headerText;

            labelBody.ForeColor = labelHeader.ForeColor;
            labelBody.BackColor = Color.Transparent;
            labelBody.Text = bodyText;

            switch (style)
            {
                case ToastStyle.Error:
                    pictureBoxIcon.Image = Properties.Resources.ToastError32;
                    break;
                case ToastStyle.Success:
                    pictureBoxIcon.Image = Properties.Resources.ToastSuccess32;
                    break;
                case ToastStyle.Warning:
                    pictureBoxIcon.Image = Properties.Resources.ToastWarning32;
                    break;
                default:
                    pictureBoxIcon.Image = Properties.Resources.AppLogo32;
                    break;
            }

            var screen = GetCurrentScreen();
            switch (position)
            {
                case ToastPosition.TopLeft:
                    Location = new Point(screen.WorkingArea.Left + 10, screen.WorkingArea.Top + 10);
                    break;
                case ToastPosition.TopRight:
                    Location = new Point(screen.WorkingArea.Right - Width - 10, screen.WorkingArea.Top + 10);
                    break;
                case ToastPosition.TopCenter:
                    Location = new Point(screen.WorkingArea.Left + (screen.WorkingArea.Width / 2) - (Width / 2), screen.WorkingArea.Top + 10);
                    break;
                case ToastPosition.BottomLeft:
                    Location = new Point(screen.WorkingArea.Left + 10, screen.WorkingArea.Bottom - Height - 10);
                    break;
                case ToastPosition.BottomRight:
                    Location = new Point(screen.WorkingArea.Right - Width - 10, screen.WorkingArea.Bottom - Height - 10);
                    break;
                case ToastPosition.BottomCenter:
                    Location = new Point(screen.WorkingArea.Left + (screen.WorkingArea.Width / 2) - (Width / 2), screen.WorkingArea.Bottom - Height - 10);
                    break;
            }

            Show();

            _startTimeUTC = DateTime.UtcNow;
            _timer.Start();
            _timer.Enabled = true;
        }

        private void FormToast_Click(object? sender, EventArgs e)
        {
            //Trigger the fade-out immediately.
            _startTimeUTC = DateTime.UtcNow.AddMilliseconds(-_duration);

            _action?.Invoke();
            _parameterizedAction?.Invoke(_actionParameter);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            var bounds = new Rectangle(0, 0, this.Width, this.Height);
            var path = GetRoundedRectPath(bounds, _cornerRadius);
            this.Region = new Region(path);
        }

        private GraphicsPath GetRoundedRectPath(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            var path = new GraphicsPath();
            path.StartFigure();

            // Top left arc
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            // Top right arc
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            // Bottom right arc
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            // Bottom left arc
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);

            path.CloseFigure();
            return path;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (Visible == false)
            {
                _timer.Stop();
                return;
            }

            if ((DateTime.UtcNow - _startTimeUTC).TotalMilliseconds > _duration)
            {
                if (Opacity == 0)
                {
                    _timer.Stop();
                    Hide();
                }
                else
                {
                    Opacity -= 0.1f;
                }
            }
            else if (Opacity < 1.0f)
            {
                Opacity += 0.1f;
            }
            else
            {
                //Just showing the dialog, waiting on the fade-out to start.
            }
        }

        private Screen GetCurrentScreen()
        {
            foreach (var screen in Screen.AllScreens)
            {
                if (screen.Bounds.Contains(Location))
                {
                    return screen;
                }
            }

            return Screen.AllScreens.First();
        }
    }
}
