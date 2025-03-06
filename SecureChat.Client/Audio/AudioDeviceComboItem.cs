namespace SecureChat.Client.Audio
{
    internal class AudioDeviceComboItem
    {
        public string Text { get; set; }
        public int DeviceIndex { get; set; }

        public AudioDeviceComboItem(string text, int deviceIndex)
        {
            Text = text;
            DeviceIndex = deviceIndex;
        }

        public override string ToString()
        {
            return Text.ToString();
        }
    }
}
