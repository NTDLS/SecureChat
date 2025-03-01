using NTDLS.ReliableMessaging;
using NTDLS.SecureKeyExchange;
using SecureChat.Library;
using SecureChat.Library.ReliableMessages;

namespace SecureChat.Client
{
    /// <summary>
    /// Reliable query and notification handler for client-server communication.
    /// </summary>
    internal class ClientReliableMessageHandlers
        : IRmMessageHandler
    {
        //private readonly IConfiguration _configuration;

        public ClientReliableMessageHandlers()
        {
        }

        public InitiateEndToEndCryptographyReply InitiateEndToEndCryptography(RmContext context, InitiateEndToEndCryptography param)
        {
            try
            {
                var compoundNegotiator = new CompoundNegotiator();

                //Apply the diffie-hellman negotiation token.
                var negotiationReplyToken = compoundNegotiator.ApplyNegotiationToken(param.NegotiationToken);

                //TODO: this is the NASCCL encryption key we will use for all user communication (but not control messages).
                Console.WriteLine($"SharedSecret: {Crypto.ComputeSha256Hash(compoundNegotiator.SharedSecret)}");

                //Reply with the applied negotiation token so that the requester can arrive at the same shared secret.
                return new InitiateEndToEndCryptographyReply(negotiationReplyToken);
            }
            catch (Exception ex)
            {
                return new InitiateEndToEndCryptographyReply(ex.GetBaseException());
            }
        }
    }
}
