namespace SecureChat.Client
{
    public static class Imaging
    {
        public static Icon LoadIconFromResources(byte[] iconData)
        {
            using var stream = new MemoryStream(iconData);
            return new Icon(stream);
        }
    }
}
