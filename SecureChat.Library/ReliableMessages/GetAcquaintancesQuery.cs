using NTDLS.ReliableMessaging;
using SecureChat.Server.Models;

namespace SecureChat.Library.ReliableMessages
{
    public class GetAcquaintancesQuery
        : IRmQuery<GetAcquaintancesQueryReply>
    {
        public GetAcquaintancesQuery()
        {
        }
    }

    public class GetAcquaintancesQueryReply
        : IRmQueryReply
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public List<AcquaintancesModel> Acquaintances { get; set; } = new();

        public GetAcquaintancesQueryReply(Exception exception)
        {
            IsSuccess = false;
            ErrorMessage = exception.GetBaseException().Message;
        }

        public GetAcquaintancesQueryReply(List<AcquaintancesModel> acquaintances)
        {
            Acquaintances = acquaintances;
            IsSuccess = true;
        }

        public GetAcquaintancesQueryReply()
        {
            Acquaintances = new();
        }
    }
}
