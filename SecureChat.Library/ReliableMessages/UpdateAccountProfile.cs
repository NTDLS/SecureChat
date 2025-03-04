using NTDLS.ReliableMessaging;
using SecureChat.Library.Models;

namespace SecureChat.Library.ReliableMessages
{
    public class UpdateAccountProfile
        : IRmQuery<UpdateAccountProfileReply>
    {
        public string DisplayName { get; set; }
        public AccountProfileModel Profile { get; set; }

        public UpdateAccountProfile(string displayName, AccountProfileModel profile)
        {
            DisplayName = displayName;
            Profile = profile;
        }
    }

    public class UpdateAccountProfileReply
        : IRmQueryReply
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public UpdateAccountProfileReply(Exception exception)
        {
            ErrorMessage = exception.GetBaseException().Message;
            IsSuccess = false;
        }

        public UpdateAccountProfileReply()
        {
            IsSuccess = true;
        }
    }
}
