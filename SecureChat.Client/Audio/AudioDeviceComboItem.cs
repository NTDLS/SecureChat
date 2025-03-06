namespace SecureChat.Client.Audio
{
    internal class AudioDeviceComboItem
    {
        public string Text { get; set; }
        public string DeviceId { get; set; }

        public AudioDeviceComboItem(string text, string deviceId)
        {
            Text = text;
            DeviceId = deviceId;
        }

        public override string ToString()
        {
            return Text.ToString();
        }
    }
}
