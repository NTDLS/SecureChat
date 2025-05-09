using Krypton.Toolkit;

namespace SecureChat.Client.Controls
{
    public class DoubleBufferedTreeView : KryptonTreeView
    {
        public DoubleBufferedTreeView()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }
    }
}
