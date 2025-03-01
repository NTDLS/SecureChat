namespace SecureChat.Server
{
    internal class AccountSession
    {
        public Guid ConnectionId { get; private set; }
        public int AccountId { get; set; }

        public AccountSession(Guid connectionId, int accountId)
        {
            ConnectionId = connectionId;
            AccountId = accountId;
        }
    }
}
