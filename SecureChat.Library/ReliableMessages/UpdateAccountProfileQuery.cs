using NTDLS.ReliableMessaging;
using SecureChat.Library.Models;

namespace SecureChat.Library.ReliableMessages
{
    public class UpdateAccountProfileQuery
        : IRmQuery<UpdateAccountProfileQueryReply>
    {
        public string DisplayName { get; set; }
        public AccountProfileModel Profile { get; set; }

        public UpdateAccountProfileQuery(string displayName, AccountProfileModel profile)
        {
            DisplayName = displayName;
            Profile = profile;
        }
    }

    public class UpdateAccountProfileQueryReply
        : IRmQueryReply
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public UpdateAccountProfileQueryReply(Exception exception)
        {
            ErrorMessage = exception.GetBaseException().Message;
            IsSuccess = false;
        }

        public UpdateAccountProfileQueryReply()
        {
            IsSuccess = true;
        }
    }
}
