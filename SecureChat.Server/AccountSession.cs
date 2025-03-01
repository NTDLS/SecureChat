using SecureChat.Library;

namespace SecureChat.Server
{
    internal class AccountSession
    {
        public Guid ConnectionId { get; private set; }
        public int? AccountId { get; private set; } = null;
        public BaselineCryptographyProvider BaselineCryptographyProvider { get; set; }

        public AccountSession(Guid connectionId, BaselineCryptographyProvider baselineCryptographyProvider)
        {
            ConnectionId = connectionId;
            BaselineCryptographyProvider = baselineCryptographyProvider;
        }

        public void SetAccountId(int accountId)
        {
            AccountId = accountId;
        }
    }
}
