namespace SecureChat.Client.Helpers
{
    public static class Imaging
    {
        public static Icon LoadIconFromResources(byte[] iconData)
        {
            using var stream = new MemoryStream(iconData);
            return new Icon(stream);
        }

        public static byte[] ImageToPngBytes(Image image)
        {
            using var ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

    }
}
