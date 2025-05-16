using NTDLS.ReliableMessaging;
using static Talkster.Library.ScConstants;

namespace Talkster.Library.ReliableMessages
{
    public class UpdateAccountStateNotification
        : IRmNotification
    {
        public ScOnlineState State { get; set; }
        public Guid AccountId { get; set; }

        public UpdateAccountStateNotification(Guid accountId, ScOnlineState state)
        {
            AccountId = accountId;
            State = state;
        }
    }
}
