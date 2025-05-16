using NTDLS.ReliableMessaging;
using Talkster.Library.Models;

namespace Talkster.Library.ReliableMessages
{
    public class GetAccountProfileQuery
        : IRmQuery<GetAccountProfileQueryReply>
    {
        /// <summary>
        /// The account id of the user we want to get the profile for.
        /// </summary>
        public Guid AccountId { get; set; }

        public GetAccountProfileQuery(Guid accountId)
        {
            AccountId = accountId;
        }
    }

    public class GetAccountProfileQueryReply
        : IRmQueryReply
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public ContactModel? Account { get; set; }

        public GetAccountProfileQueryReply(Exception exception)
        {
            IsSuccess = false;
            ErrorMessage = exception.GetBaseException().Message;
        }

        public GetAccountProfileQueryReply(ContactModel? account)
        {
            Account = account;
            IsSuccess = true;
        }

        public GetAccountProfileQueryReply()
        {
        }
    }
}
