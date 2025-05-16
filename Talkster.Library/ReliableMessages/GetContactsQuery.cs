using NTDLS.ReliableMessaging;
using Talkster.Library.Models;

namespace Talkster.Library.ReliableMessages
{
    public class GetContactsQuery
        : IRmQuery<GetContactsQueryReply>
    {
        public GetContactsQuery()
        {
        }
    }

    public class GetContactsQueryReply
        : IRmQueryReply
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public List<ContactModel> Contacts { get; set; } = new();

        public GetContactsQueryReply(Exception exception)
        {
            IsSuccess = false;
            ErrorMessage = exception.GetBaseException().Message;
        }

        public GetContactsQueryReply(List<ContactModel> contacts)
        {
            Contacts = contacts;
            IsSuccess = true;
        }

        public GetContactsQueryReply()
        {
            Contacts = new();
        }
    }
}
