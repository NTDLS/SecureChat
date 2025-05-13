using System;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace SecureChat.Client.Controls
{


    public static class GraphicsExtensions
    {
        public static void FillRoundedRectangle(this Graphics g, Brush brush, float x, float y, float width, float height, float radius)
        {
            using (GraphicsPath path = RoundedRectanglePath(x, y, width, height, radius))
            {
                g.FillPath(brush, path);
            }
        }

        private static GraphicsPath RoundedRectanglePath(float x, float y, float width, float height, float radius)
        {
            float diameter = radius * 2;
            GraphicsPath path = new GraphicsPath();

            // Top left arc
            path.AddArc(x, y, diameter, diameter, 180, 90);
            // Top line
            path.AddLine(x + radius, y, x + width - radius, y);
            // Top right arc
            path.AddArc(x + width - diameter, y, diameter, diameter, 270, 90);
            // Right line
            path.AddLine(x + width, y + radius, x + width, y + height - radius);
            // Bottom right arc
            path.AddArc(x + width - diameter, y + height - diameter, diameter, diameter, 0, 90);
            // Bottom line
            path.AddLine(x + width - radius, y + height, x + radius, y + height);
            // Bottom left arc
            path.AddArc(x, y + height - diameter, diameter, diameter, 90, 90);
            // Left line
            path.AddLine(x, y + height - radius, x, y + radius);

            path.CloseFigure();
            return path;
        }
    }

}
