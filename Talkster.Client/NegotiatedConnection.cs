using NTDLS.ReliableMessaging;

namespace Talkster.Client
{
    /// <summary>
    /// Used to store state information about a server connection that has encryption negotiated.
    /// </summary>
    internal class NegotiatedConnection
    {
        public Version ServerVersion { get; set; }
        public RmClient Client { get; set; }

        public NegotiatedConnection(RmClient client, Version serverVersion)
        {
            Client = client;
            ServerVersion = serverVersion;
        }
    }
}
