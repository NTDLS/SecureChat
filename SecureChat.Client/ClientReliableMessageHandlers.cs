using NTDLS.ReliableMessaging;
using NTDLS.SecureKeyExchange;
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
        /// Remote client is requesting that another client accept a large or binary file
        /// where we need to give the remote client a chance to select a save location.
        /// </summary>
        public void FileTransmissionBeginRequestNotification(RmContext context, FileTransmissionBeginRequestNotification param)
        {
            try
            {
                var activeChat = VerifyAndActiveChat(context, param.SessionId);

                //TODO: Show a dialog to the user to select a file location.
                //if (accepted)
                {
                    activeChat.FileReceiveBuffers.Add(param.FileId, new FileReceiveBuffer(param.FileId, param.FileName, param.FileSize));
                }

            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }

        /// <summary>
        /// A client that requested a voice call with us is cancelling that request.
        /// </summary>
        public void CancelVoiceCallRequestNotification(RmContext context, CancelVoiceCallRequestNotification param)
        {
            try
            {
                var activeChat = VerifyAndActiveChat(context, param.SessionId);

                //TODO: inform the user that the call request was cancelled.
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }


        /// <summary>
        /// The client that we requested a voice call with has accepted that call.
        /// </summary>
        public void AcceptVoiceCallNotification(RmContext context, AcceptVoiceCallNotification param)
        {
            try
            {
                var activeChat = VerifyAndActiveChat(context, param.SessionId);
                if (activeChat.LastOutgoingCallControl == null)
                {
                    throw new Exception("Last outgoing call does not exist.");
                }

                //Let the local user know that the call was accepted.
                activeChat.LastOutgoingCallControl.Text = "Call accepted.";
                activeChat.LastOutgoingCallControl.Disable();

                activeChat.StartAudioPump();
                activeChat.AppendSystemMessageLine("The voice call is now connected.");
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }

        /// <summary>
        /// The client that we requested a voice call with has declined that call.
        /// </summary>
        public void DeclineVoiceCallNotification(RmContext context, DeclineVoiceCallNotification param)
        {
            try
            {
                var activeChat = VerifyAndActiveChat(context, param.SessionId);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }

        /// <summary>
        /// A client is requesting a voice call with us.
        /// </summary>
        public void RequestVoiceCallNotification(RmContext context, RequestVoiceCallNotification param)
        {
            try
            {
                var activeChat = VerifyAndActiveChat(context, param.SessionId);

                activeChat?.AlertOfIncomingCall();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }

        /// <summary>
        /// A client is requesting that an active voice call be terminated.
        /// </summary>
        public void TerminateVoiceCallNotification(RmContext context, TerminateVoiceCallNotification param)
        {
            try
            {
                var activeChat = VerifyAndActiveChat(context, param.SessionId);
                activeChat?.TerminateVoiceCall();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }

        /// <summary>
        /// Client has requested that a file transfer be cancelled.
        /// </summary>
        public void FileTransmissionCancelNotification(RmContext context, FileTransmissionCancelNotification param)
        {
            try
            {
                var activeChat = VerifyAndActiveChat(context, param.SessionId);
                activeChat.FileReceiveBuffers.Remove(param.FileId);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }

        /// <summary>
        /// A client is beginning to transmit a file to us.
        /// </summary>
        public FileTransmissionBeginQueryReply FileTransmissionBeginQuery(RmContext context, FileTransmissionBeginQuery param)
        {
            try
            {
                var activeChat = VerifyAndActiveChat(context, param.SessionId);
                if (activeChat.FileReceiveBuffers.ContainsKey(param.FileId) == false)
                {
                    activeChat.FileReceiveBuffers.Add(param.FileId, new FileReceiveBuffer(param.FileId, param.FileName, param.FileSize));
                }

                return new FileTransmissionBeginQueryReply();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                return new FileTransmissionBeginQueryReply(ex);
            }
        }

        /// <summary>
        /// A client is transmitting a file chunk to us.
        /// </summary>
        public FileTransmissionChunkQueryReply FileTransmissionChunkQuery(RmContext context, FileTransmissionChunkQuery param)
        {
            try
            {
                var activeChat = VerifyAndActiveChat(context, param.SessionId);

                if (activeChat.FileReceiveBuffers.TryGetValue(param.FileId, out var buffer))
                {
                    buffer.AppendData(activeChat.Cipher(param.Bytes));
                    return new FileTransmissionChunkQueryReply();
                }
                else
                {
                    throw new Exception("File buffer not found, transmission cancelled?.");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                return new FileTransmissionChunkQueryReply(ex);
            }
        }

        /// <summary>
        /// A client has finished transmitting a file to us.
        /// </summary>
        public FileTransmissionEndQueryReply FileTransmissionEndQuery(RmContext context, FileTransmissionEndQuery param)
        {
            try
            {
                var activeChat = VerifyAndActiveChat(context, param.SessionId);

                if (activeChat.FileReceiveBuffers.TryGetValue(param.FileId, out var buffer))
                {
                    var imageBytes = buffer.GetFileBytes();
                    activeChat.ReceiveImage(imageBytes);
                    buffer.Dispose();
                    activeChat.FileReceiveBuffers.Remove(param.FileId);
                }
                else
                {
                    throw new Exception("File buffer not found, transmission cancelled?.");
                }

                return new FileTransmissionEndQueryReply();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                return new FileTransmissionEndQueryReply(ex);
            }
        }

        /// <summary>
        /// A client is letting us know that they are terminating the chat session.
        /// </summary>
        public void TerminateChatNotification(RmContext context, TerminateChatNotification param)
        {
            try
            {
                var activeChat = VerifyAndActiveChat(context, param.SessionId);

                activeChat?.Terminate();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }

        /// <summary>
        /// A client is sending us a text message.
        /// </summary>
        public ExchangeMessageTextQueryReply ExchangeMessageTextQuery(RmContext context, ExchangeMessageTextQuery param)
        {
            try
            {
                var activeChat = VerifyAndActiveChat(context, param.SessionId);

                activeChat?.ReceiveMessage(param.CipherText);

                return new ExchangeMessageTextQueryReply();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                return new ExchangeMessageTextQueryReply(ex);
            }
        }

        /// <summary>
        /// A client is requesting the initiation of end-to-end encryption.
        /// They have supplied a Diffie-Hellman negotiation token, so apply it and reply with the result.
        /// This is also where we prop up the chat session.
        /// </summary>
        public InitiatePeerToPeerSessionQueryReply InitiatePeerToPeerSessionQuery(RmContext context, InitiatePeerToPeerSessionQuery param)
        {
            try
            {
                if (ServerConnection.Current == null)
                    throw new Exception("Local connection is not established.");

                var compoundNegotiator = new CompoundNegotiator();

                //Apply the diffie-hellman negotiation token.
                var negotiationReplyToken = compoundNegotiator.ApplyNegotiationToken(param.NegotiationToken);

                var activeChat = ServerConnection.Current.AddActiveChat(
                    param.SessionId,
                    param.PeerConnectionId,
                    param.SourceAccountId,
                    param.DisplayName,
                    compoundNegotiator.SharedSecret);

                //Reply with the applied negotiation token so that the requester can arrive at the same shared secret.
                return new InitiatePeerToPeerSessionQueryReply(negotiationReplyToken);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                return new InitiatePeerToPeerSessionQueryReply(ex.GetBaseException());
            }
        }

        public ActiveChat VerifyAndActiveChat(RmContext context, Guid sessionId)
        {
            if (ServerConnection.Current == null)
                throw new Exception("Local connection is not established.");

            if (context.GetCryptographyProvider() == null)
                throw new Exception("Cryptography has not been initialized.");

            var activeChat = ServerConnection.Current.GetActiveChat(sessionId)
                ?? throw new Exception("Chat session was not found.");

            return activeChat;
        }
    }
}
