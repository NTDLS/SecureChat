namespace SecureChat.Client.Helpers
{
    public static class Imaging
    {
        public static Bitmap LoadImageFromResources(byte[] bitmapData)
        {
            using var stream = new MemoryStream(bitmapData);
            return new Bitmap(stream);
        }


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
