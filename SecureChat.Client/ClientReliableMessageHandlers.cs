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
        /// The server is letting us know that it received the UDP steam initialization request and has completed it.
        /// Sent from <see cref="SecureChat.Server.DatagramMessageHandlers.InitiateNetworkAddressTranslationMessage"/>
        /// </summary>
        public void DatagramStreamReadyNotification(RmContext context, DatagramStreamReadyNotification param)
        {
            try
            {
                var activeChat = VerifyAndActiveChat(context, param.PeerToPeerId);

                if (activeChat.DatagramClient == null)
                    throw new Exception("The datagram client is not initialized");

                activeChat.DatagramClient.StartKeepAlive();
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
                var activeChat = VerifyAndActiveChat(context, param.PeerToPeerId);

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
                var activeChat = VerifyAndActiveChat(context, param.PeerToPeerId);
                if (activeChat.LastOutgoingCallControl == null)
                {
                    throw new Exception("Last outgoing call does not exist.");
                }

                //Let the local user know that the call was accepted.
                activeChat.LastOutgoingCallControl.Text = "Call accepted.";
                activeChat.LastOutgoingCallControl.Disable();

                //TODO: Setup the call...
                activeChat.StartAudioPump();
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
                var activeChat = VerifyAndActiveChat(context, param.PeerToPeerId);
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
                var activeChat = VerifyAndActiveChat(context, param.PeerToPeerId);

                //Prop up the UDP connection:
                activeChat.InitiateNetworkAddressTranslationMessage(param.PeerToPeerId, param.PeerConnectionId);

                activeChat?.AlertOfIncomingCall();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }

        /// <summary>
        /// A client is beginning to transmit a file to us.
        /// </summary>
        public void FileTransmissionBeginNotification(RmContext context, FileTransmissionBeginNotification param)
        {
            try
            {
                var activeChat = VerifyAndActiveChat(context, param.PeerToPeerId);

                activeChat.FileReceiveBuffers.Add(param.FileId, new FileReceiveBuffer(param.FileId, param.FileName, param.FileSize));
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }

        /// <summary>
        /// A client is transmitting a file chunk to us.
        /// </summary>
        public void FileTransmissionChunkNotification(RmContext context, FileTransmissionChunkNotification param)
        {
            try
            {
                var activeChat = VerifyAndActiveChat(context, param.PeerToPeerId);

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
        /// A client has finished transmitting a file to us.
        /// </summary>
        public FileTransmissionEndQueryReply FileTransmissionEndQuery(RmContext context, FileTransmissionEndQuery param)
        {
            try
            {
                var activeChat = VerifyAndActiveChat(context, param.PeerToPeerId);

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
                var activeChat = VerifyAndActiveChat(context, param.PeerToPeerId);

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
                var activeChat = VerifyAndActiveChat(context, param.PeerToPeerId);

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
        public InitiateEndToEndCryptographyQueryReply InitiateEndToEndCryptographyQuery(RmContext context, InitiateEndToEndCryptographyQuery param)
        {
            try
            {
                if (ServerConnection.Current == null)
                    throw new Exception("Local connection is not established.");

                var compoundNegotiator = new CompoundNegotiator();

                //Apply the diffie-hellman negotiation token.
                var negotiationReplyToken = compoundNegotiator.ApplyNegotiationToken(param.NegotiationToken);

                var activeChat = ServerConnection.Current.AddActiveChat(
                    param.PeerToPeerId,
                    param.PeerConnectionId,
                    param.SourceAccountId,
                    param.DisplayName,
                    compoundNegotiator.SharedSecret);

                ServerConnection.Current.FormHome.Invoke(() =>
                {
                    //We have to create the form on the main window thread.
                    activeChat.Form = new FormMessage(activeChat);
                    activeChat.Form.CreateControl(); //Force the window handle to be created before the form is shown,
                    var handle = activeChat.Form.Handle; // Accessing the Handle property forces handle creation
                    //activeChat.Form.Show();
                });

                //Reply with the applied negotiation token so that the requester can arrive at the same shared secret.
                return new InitiateEndToEndCryptographyQueryReply(negotiationReplyToken);
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                return new InitiateEndToEndCryptographyQueryReply(ex.GetBaseException());
            }
        }

        public ActiveChat VerifyAndActiveChat(RmContext context, Guid peerToPeerId)
        {
            if (ServerConnection.Current == null)
                throw new Exception("Local connection is not established.");

            if (context.GetCryptographyProvider() == null)
                throw new Exception("Cryptography has not been initialized.");

            var activeChat = ServerConnection.Current.GetActiveChat(peerToPeerId)
                ?? throw new Exception("Chat session was not found.");

            return activeChat;
        }
    }
}
