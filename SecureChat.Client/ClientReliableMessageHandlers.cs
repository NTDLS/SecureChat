using NTDLS.ReliableMessaging;
using NTDLS.SecureKeyExchange;
using SecureChat.Client.Forms;
using SecureChat.Library.ReliableMessages;
using Serilog;

namespace SecureChat.Client
{
    /// <summary>
    /// Reliable query and notification handler for client-server communication.
    /// </summary>
    internal class ClientReliableMessageHandlers
        : IRmMessageHandler
    {
        public ClientReliableMessageHandlers()
        {
        }

        public ExchangePeerToPeerQueryReply ExchangePeerToPeerQuery(RmContext context, ExchangePeerToPeerQuery param)
        {
            try
            {
                if (SessionState.Instance == null)
                    throw new Exception("The local connection is not established.");

                if (context.GetCryptographyProvider() == null)
                    throw new Exception("Message cannot be receive until cryptography has been initialized.");

                var activeChat = SessionState.Instance.ActiveChats.FirstOrDefault(o => o.AccountId == param.MessageFromAccountId);

                activeChat?.Form?.AppendReceivedMessage(param.CipherText);

                return new ExchangePeerToPeerQueryReply();
            }
            catch (Exception ex)
            {
                Log.Error("Failed to handle peer-to-peer message.", ex);
                return new ExchangePeerToPeerQueryReply(ex);
            }
        }

        public InitiateEndToEndCryptographyReply InitiateEndToEndCryptography(RmContext context, InitiateEndToEndCryptography param)
        {
            try
            {
                if (SessionState.Instance == null)
                    throw new Exception("The local connection is not established.");

                var compoundNegotiator = new CompoundNegotiator();

                //Apply the diffie-hellman negotiation token.
                var negotiationReplyToken = compoundNegotiator.ApplyNegotiationToken(param.NegotiationToken);

                //TODO: this is the NASCCL encryption key we will use for all user communication (but not control messages).
                //Console.WriteLine($"SharedSecret: {Crypto.ComputeSha256Hash(compoundNegotiator.SharedSecret)}");

                var activeChat = SessionState.Instance.AddActiveChat(param.PeerConnectionId, param.SourceAccountId, param.DisplayName, compoundNegotiator.SharedSecret);

                SessionState.Instance.FormHome.Invoke(() =>
                {
                    //We have to create the form on the main window thread.
                    activeChat.Form = new FormMessage(activeChat);
                    activeChat.Form.Show();
                });

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
