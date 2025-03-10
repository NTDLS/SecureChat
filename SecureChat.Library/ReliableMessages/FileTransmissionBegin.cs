﻿using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class FileTransmissionBegin
        : IRmNotification
    {
        public Guid FileId { get; set; }
        public int FileSize { get; set; }
        public Guid ConnectionId { get; set; }
        public string FileName { get; set; }
        public Guid PeerToPeerId { get; set; }

        public FileTransmissionBegin(Guid peerToPeerId, Guid connectionId, Guid fileId, string fileName, int fileSize)
        {
            PeerToPeerId = peerToPeerId;
            ConnectionId = connectionId;
            FileId = fileId;
            FileSize = fileSize;
            FileName = fileName;
        }
    }
}