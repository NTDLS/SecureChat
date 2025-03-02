using NTDLS.ReliableMessaging;
using static SecureChat.Library.ScConstants;

namespace SecureChat.Library.ReliableMessages
{
    public class UpdateAccountStatus
        : IRmNotification
    {
        public ScOnlineState State { get; set; }
        /// <summary>
        /// User supplied status text.
        /// </summary>
        public string Status { get; set; }
        public Guid ConnectionId { get; set; }
        public Guid AccountId { get; set; }

        public UpdateAccountStatus(Guid accountId, ScOnlineState state, string status)
        {
            AccountId = accountId;
            State = state;
            Status = status;
        }
    }
}
