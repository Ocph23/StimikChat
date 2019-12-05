using System;
using System.Collections.Generic;
using ModelShared.Interfaces;

namespace ModelShared.Models
{
    public class ChatMessage : IChatMessage
    {
        public string MessageId { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public bool Readed { get; set; }
        public int SenderId { get; set; }
        public int RecieverId { get; set; }
        public MessageType MessageType { get; set; }
    }


    public enum MessageType
    {
        Text, Image, HTML, Animation
    }
}


