using NTDLS.ReliableMessaging;
using Talkster.Library.Models;

namespace Talkster.Library.ReliableMessages
{
    public class AccountSearchQuery
        : IRmQuery<AccountSearchQueryReply>
    {
        public string DisplayName { get; set; }

        public AccountSearchQuery(string displayName)
        {
            DisplayName = displayName;
        }
    }

    public class AccountSearchQueryReply
        : IRmQueryReply
    {
        public bool IsSuccess { get; set; }
        public List<AccountSearchModel> Accounts { get; set; }

        public string? ErrorMessage { get; set; }

        public AccountSearchQueryReply(Exception exception)
        {
            Accounts = new();
            ErrorMessage = exception.GetBaseException().Message;
            IsSuccess = false;
        }

        public AccountSearchQueryReply()
        {
            Accounts = new();
            IsSuccess = true;
        }

        public AccountSearchQueryReply(List<AccountSearchModel> accounts)
        {
            Accounts = accounts;
            IsSuccess = true;
        }
    }
}
