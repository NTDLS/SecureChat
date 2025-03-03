﻿using NTDLS.NASCCL;
using SecureChat.Client.Controls;
using SecureChat.Client.Forms;
using SecureChat.Library.ReliableMessages;
using System.Text;

namespace SecureChat.Client
{
    internal class ActiveChat
    {
        public bool IsTerminated { get; private set; } = false;
        public FormMessage? Form { get; set; }
        public Guid AccountId { get; private set; }
        public string DisplayName { get; private set; }
        public Guid ConnectionId { get; private set; }
        private readonly CryptoStream _streamCryptography;
        public Dictionary<Guid, FileReceiveBuffer> FileReceiveBuffers { get; set; } = new();

        public ActiveChat(Guid connectionId, Guid accountId, string displayName, byte[] sharedSecret)
        {
            _streamCryptography = new CryptoStream(sharedSecret);
            ConnectionId = connectionId;
            AccountId = accountId;
            DisplayName = displayName;
        }

        public string DecryptString(byte[] cipherText)
        {
            lock (_streamCryptography)
            {
                var plainTextBytes = _streamCryptography.Cipher(cipherText);
                _streamCryptography.ResetStream();
                return Encoding.UTF8.GetString(plainTextBytes);
            }
        }

        public byte[] EncryptString(string plainText)
        {
            lock (_streamCryptography)
            {
                var cipherText = _streamCryptography.Cipher(plainText);
                _streamCryptography.ResetStream();
                return cipherText;
            }
        }

        public byte[] Cipher(byte[] bytes)
        {
            lock (_streamCryptography)
            {
                var cipherText = _streamCryptography.Cipher(bytes);
                _streamCryptography.ResetStream();
                return cipherText;
            }
        }

        public void Terminate()
        {
            if (IsTerminated)
            {
                return;
            }
            IsTerminated = true;
            LocalSession.Current?.Client.Notify(new TerminateChat(ConnectionId, LocalSession.Current.AccountId));
            Form?.AppendSystemMessageLine($"Chat ended at {DateTime.Now}.", Color.Red);
        }

        public void ReceiveImage(byte[] imageBytes)
        {
            if (IsTerminated)
            {
                return;
            }

            Form?.AppendFlowControl(new FlowControlImage(imageBytes));
        }

        public void ReceiveMessage(byte[] cipherText)
        {
            if (IsTerminated)
            {
                return;
            }

            Form?.AppendReceivedMessageLine(DisplayName, DecryptString(cipherText), Color.DarkRed);
        }

        public bool SendMessage(string plaintText)
        {
            if (IsTerminated)
            {
                return false;
            }

            return LocalSession.Current?.Client.Query(new ExchangePeerToPeerQuery(
                    ConnectionId, LocalSession.Current.AccountId, EncryptString(plaintText))).ContinueWith(o =>
                    {
                        if (!o.IsFaulted && o.Result.IsSuccess)
                        {
                            return true;
                        }
                        return false;
                    }).Result ?? false;
        }

        public void TransmitFile(string fileName, byte[] fileBytes)
        {
            var fileId = Guid.NewGuid();

            LocalSession.Current?.Client.Notify(new FileTransmissionBegin(ConnectionId, LocalSession.Current.AccountId, fileId, fileName, fileBytes.Length));

            using (MemoryStream memoryStream = new MemoryStream(fileBytes))
            {
                var buffer = new byte[Settings.Instance.FileTransmissionChunkSize];

                int bytesRead;

                while ((bytesRead = memoryStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    var chunkToSend = buffer; // Default: Full buffer
                    if (bytesRead < buffer.Length) // Handle last partial chunk
                    {
                        chunkToSend = new byte[bytesRead];
                        Array.Copy(buffer, chunkToSend, bytesRead);
                    }

                    LocalSession.Current?.Client.Notify(new FileTransmissionChunk(ConnectionId, LocalSession.Current.AccountId, fileId, Cipher(chunkToSend)));
                }
            }

            LocalSession.Current?.Client.Query(new FileTransmissionEnd(ConnectionId, LocalSession.Current.AccountId, fileId)).ContinueWith(o =>
            {
                if (!o.IsFaulted && o.Result.IsSuccess)
                {
                    //Only show the image locally if the file was successfully transmitted.
                    Form?.AppendFlowControl(new FlowControlImage(fileBytes));
                }
                else
                {
                    Form?.AppendSystemMessageLine($"Failed to transmit file.", Color.Red);
                }
            });
        }
    }
}
