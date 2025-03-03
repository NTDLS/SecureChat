using NTDLS.ReliableMessaging;
using static SecureChat.Library.ScConstants;

namespace SecureChat.Library.ReliableMessages
{
    public class UpdateAccountState
        : IRmNotification
    {
        public ScOnlineState State { get; set; }

        public Guid AccountId { get; set; }

        public UpdateAccountState(Guid accountId, ScOnlineState state)
        {
            AccountId = accountId;
            State = state;
        }
    }
}
