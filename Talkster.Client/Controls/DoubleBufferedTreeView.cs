using Krypton.Toolkit;

namespace Talkster.Client.Controls
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
