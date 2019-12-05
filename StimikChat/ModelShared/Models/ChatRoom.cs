using System;
using System.Collections.Generic;
using ModelShared.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ModelShared.Models
{
    public class ChatRoom : IChatRoom
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string RoomName { get; set; }
        public DateTime Created { get; set; }
        public int OwnerId { get; set; }
        public List<int> Users { get; set; } = new List<int>();
        public ConversationType ChatType { get; set; }
        public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }
}


