namespace SecureChat.Client.Controls
{
    public static class ControlExtensions
    {
        public static void DoubleBuffered(this Control control, bool enable)
        {
            var prop = control.GetType().GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic);
            if (prop != null)
            {
                prop.SetValue(control, enable, null);
            }
        }
    }
}
