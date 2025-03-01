namespace SecureChat.Client.Models
{
    internal class LoginResult
    {
        public string ServerAddress { get; set; }
        public string DisplayName { get; set; }
        public int ServerPort { get; set; }

        public LoginResult(string serverAddress, int serverPort, string displayName)
        {
            ServerAddress = serverAddress;
            ServerPort = serverPort;
            DisplayName = displayName;
        }
    }
}
