namespace SecureChat.Client.Controls
{
    internal interface IFileTransmissionControl
    {
        FileOutboundTransfer Transfer { get; }
        bool IsCancelled { get; }

        void SetProgressValue(int value);
        void Remove();
    }
}
