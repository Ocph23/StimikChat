using System;
using System.Collections.Generic;
using ModelShared.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ModelShared.Models
{
    public class ChatMessage : IChatMessage
    {
         [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
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


