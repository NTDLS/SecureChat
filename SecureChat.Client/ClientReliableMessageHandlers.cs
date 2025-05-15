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
        /// A client is letting us know that they are accepting a file transfer request.
        /// </summary>
        public void FileTransferAcceptRequestNotification(RmContext context, FileTransferAcceptRequestNotification param)
        {
            try
            {
                var activeChat = VerifyAndActiveChat(context, param.SessionId);
                activeChat.FileTransferAccepted(param.FileId);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }

        /// <summary>
        /// A client is letting us know that they are declining a file transfer request.
        /// </summary>
        public void FileTransferDeclineRequestNotification(RmContext context, FileTransferDeclineRequestNotification param)
        {
            try
            {
                var activeChat = VerifyAndActiveChat(context, param.SessionId);
                activeChat.FileTransferDeclined(param.FileId);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }

        /// <summary>
        /// Remote client is requesting that another client accept a large or binary file
        /// where we need to give the remote client a chance to select a save location.
        /// </summary>
        public void FileTransferBeginRequestNotification(RmContext context, FileTransferBeginRequestNotification param)
        {
            try
            {
                var activeChat = VerifyAndActiveChat(context, param.SessionId);

                activeChat.ReceiveFileTransferRequestMessage(param.FileId, param.FileName, param.FileSize, param.IsImage);
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
                activeChat.AppendSuccessMessageLine("The voice call is now connected.");
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
        public void FileTransferCancelNotification(RmContext context, FileTransferCancelNotification param)
        {
            try
            {
                var activeChat = VerifyAndActiveChat(context, param.SessionId);

                if (activeChat.PendingFileTransfers.TryGetValue(param.FileId, out var pendingControl))
                {
                    pendingControl.Cancel();
                    pendingControl.Remove();
                    activeChat.InboundFileTransfers.Remove(param.FileId);
                    activeChat.AppendSystemMessageLine($"File transfer cancelled: {Path.GetFileName(pendingControl.FileName)}");
                }

                if (activeChat.InboundFileTransfers.TryGetValue(param.FileId, out var inboundControl))
                {
                    inboundControl.Cancel();
                    inboundControl.Remove();
                    activeChat.InboundFileTransfers.Remove(param.FileId);
                    activeChat.AppendSystemMessageLine($"File transfer cancelled: {Path.GetFileName(inboundControl.Transfer.FileName)}");
                }

                if (activeChat.OutboundFileTransfers.TryGetValue(param.FileId, out var outboundControl))
                {
                    outboundControl.Cancel();
                    outboundControl.Remove();
                    activeChat.OutboundFileTransfers.Remove(param.FileId);
                    activeChat.AppendSystemMessageLine($"File transfer cancelled: {Path.GetFileName(outboundControl.Transfer.FileName)}");
                }

            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }

        /// <summary>
        /// A client is beginning to transmit a file to us.
        /// </summary>
        public FileTransferBeginQueryReply FileTransferBeginQuery(RmContext context, FileTransferBeginQuery param)
        {
            try
            {
                var activeChat = VerifyAndActiveChat(context, param.SessionId);
                if (activeChat.InboundFileTransfers.ContainsKey(param.FileId) == false)
                {
                    var control = activeChat.AppendFileTransferReceiveProgress(param.FileId, param.FileName, param.FileSize, param.IsImage);
                    activeChat.InboundFileTransfers.Add(param.FileId, control);
                }

                return new FileTransferBeginQueryReply();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                return new FileTransferBeginQueryReply(ex);
            }
        }

        /// <summary>
        /// A client is transmitting a file chunk to us.
        /// </summary>
        public void FileTransferChunkQuery(RmContext context, FileTransferChunkQuery param)
        {
            try
            {
                var activeChat = VerifyAndActiveChat(context, param.SessionId);

                if (activeChat.InboundFileTransfers.TryGetValue(param.FileId, out var control))
                {
                    if (control.Transfer.AppendChunk(param.Bytes, param.ChunkNumber))
                    {
                        if (control.Transfer.IsImage)
                        {
                            //The file is an image, so we need to display it.
                            activeChat.ReceiveImageMessage(param.FileId, control.Transfer.GetFileBytes());
                        }
                        else
                        {
                            //The file is not an image, so we need to show the control with a link to the local file.
                            activeChat.ReceiveFileMessage(param.FileId, control.Transfer.SaveAsFileName);
                        }

                        control.Remove();
                        activeChat.InboundFileTransfers.Remove(param.FileId);

                        //Let the user know that the file transfer is complete.
                        context.Notify(new FileTransferAcknowledgmentNotification(param.SessionId, activeChat.PeerConnectionId, param.FileId));
                    }
                    else
                    {
                        control.SetProgressValue(control.Transfer.PercentComplete);
                    }
                }
                else
                {
                    throw new Exception("File buffer not found, transfer cancelled?.");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
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
        /// A client is reporting that it received a file.
        /// </summary>
        public void FileTransferAcknowledgmentNotification(RmContext context, FileTransferAcknowledgmentNotification param)
        {
            try
            {
                var activeChat = VerifyAndActiveChat(context, param.SessionId);
                activeChat?.ReceiveFileTransferAcknowledgment(param.FileId);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }

        /// <summary>
        /// A client is reporting that it received a text message.
        /// </summary>
        public void TextMessageAcknowledgmentNotification(RmContext context, TextMessageAcknowledgmentNotification param)
        {
            try
            {
                var activeChat = VerifyAndActiveChat(context, param.SessionId);
                activeChat?.ReceiveTextMessageAcknowledgment(param.MessageId);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }

        /// <summary>
        /// A client is sending us a text message.
        /// </summary>
        public void TextMessageNotification(RmContext context, TextMessageNotification param)
        {
            try
            {
                var activeChat = VerifyAndActiveChat(context, param.SessionId);

                activeChat?.ReceiveTextMessage(param);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
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
