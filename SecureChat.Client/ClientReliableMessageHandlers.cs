using NTDLS.ReliableMessaging;
using NTDLS.SecureKeyExchange;
using SecureChat.Client.Forms;
using SecureChat.Library.ReliableMessages;
using Serilog;
using System.Diagnostics;

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

        /// <summary>
        /// The client is beginning to transmit a file.
        /// </summary>
        public void FileTransmissionBegin(RmContext context, FileTransmissionBegin param)
        {
            try
            {
                if (LocalSession.Current == null)
                    throw new Exception("Local connection is not established.");

                if (context.GetCryptographyProvider() == null)
                    throw new Exception("Cryptography has not been initialized.");

                var activeChat = LocalSession.Current.GetActiveChat(param.PeerToPeerId)
                    ?? throw new Exception("Chat session was not found.");

                activeChat.FileReceiveBuffers.Add(param.FileId, new FileReceiveBuffer(param.FileId, param.FileName, param.FileSize));
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }

        /// <summary>
        /// The client transmitting a file chunk.
        /// </summary>
        public void FileTransmissionChunk(RmContext context, FileTransmissionChunk param)
        {
            try
            {
                if (LocalSession.Current == null)
                    throw new Exception("Local connection is not established.");

                if (context.GetCryptographyProvider() == null)
                    throw new Exception("Cryptography has not been initialized.");

                var activeChat = LocalSession.Current.GetActiveChat(param.PeerToPeerId)
                    ?? throw new Exception("Chat session was not found.");

                if (activeChat.FileReceiveBuffers.TryGetValue(param.FileId, out var buffer))
                {
                    buffer.AppendData(activeChat.Cipher(param.Bytes));
                }
                else
                {
                    throw new Exception("File buffer not found.");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }

        /// <summary>
        /// The client has finished transmitting a file.
        /// </summary>
        public FileTransmissionEndReply FileTransmissionEnd(RmContext context, FileTransmissionEnd param)
        {
            try
            {
                if (LocalSession.Current == null)
                    throw new Exception("Local connection is not established.");

                if (context.GetCryptographyProvider() == null)
                    throw new Exception("Cryptography has not been initialized.");

                var activeChat = LocalSession.Current.GetActiveChat(param.PeerToPeerId)
                    ?? throw new Exception("Chat session was not found.");

                if (activeChat.FileReceiveBuffers.TryGetValue(param.FileId, out var buffer))
                {
                    var imageBytes = buffer.GetFileBytes();
                    activeChat.ReceiveImage(imageBytes);
                    buffer.Dispose();
                    activeChat.FileReceiveBuffers.Remove(param.FileId);
                }
                else
                {
                    throw new Exception("File buffer not found.");
                }

                return new FileTransmissionEndReply();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                return new FileTransmissionEndReply(ex);
            }
        }

        public void TerminateChat(RmContext context, TerminateChat param)
        {
            try
            {
                if (LocalSession.Current == null)
                    throw new Exception("Local connection is not established.");

                if (context.GetCryptographyProvider() == null)
                    throw new Exception("Cryptography has not been initialized.");

                var activeChat = LocalSession.Current.GetActiveChat(param.PeerToPeerId)
                    ?? throw new Exception("Chat session was not found.");

                activeChat?.Terminate();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }

        public ExchangePeerToPeerQueryReply ExchangePeerToPeerQuery(RmContext context, ExchangePeerToPeerQuery param)
        {
            try
            {
                if (LocalSession.Current == null)
                    throw new Exception("Local connection is not established.");

                if (context.GetCryptographyProvider() == null)
                    throw new Exception("Cryptography has not been initialized.");

                var activeChat = LocalSession.Current.GetActiveChat(param.PeerToPeerId)
                    ?? throw new Exception("Chat session was not found.");

                activeChat?.ReceiveMessage(param.CipherText);

                return new ExchangePeerToPeerQueryReply();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                return new ExchangePeerToPeerQueryReply(ex);
            }
        }

        public InitiateEndToEndCryptographyReply InitiateEndToEndCryptography(RmContext context, InitiateEndToEndCryptography param)
        {
            try
            {
                if (LocalSession.Current == null)
                    throw new Exception("Local connection is not established.");

                var compoundNegotiator = new CompoundNegotiator();

                //Apply the diffie-hellman negotiation token.
                var negotiationReplyToken = compoundNegotiator.ApplyNegotiationToken(param.NegotiationToken);

                //TODO: this is the NASCCL encryption key we will use for all user communication (but not control messages).
                //Console.WriteLine($"SharedSecret: {Crypto.ComputeSha256Hash(compoundNegotiator.SharedSecret)}");

                var activeChat = LocalSession.Current.AddActiveChat(param.PeerToPeerId, param.PeerConnectionId, param.SourceAccountId, param.DisplayName, compoundNegotiator.SharedSecret);

                LocalSession.Current.FormHome.Invoke(() =>
                {
                    //We have to create the form on the main window thread.
                    activeChat.Form = new FormMessage(activeChat);
                    activeChat.Form.CreateControl(); //Force the window handle to be created before the form is shown,
                    var handle = activeChat.Form.Handle; // Accessing the Handle property forces handle creation
                    //activeChat.Form.Show();
                });

                //Reply with the applied negotiation token so that the requester can arrive at the same shared secret.
                return new InitiateEndToEndCryptographyReply(negotiationReplyToken);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                return new InitiateEndToEndCryptographyReply(ex.GetBaseException());
            }
        }
    }
}
