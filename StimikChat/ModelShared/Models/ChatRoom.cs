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
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string RoomName { get; set; }
        public int OwnerId { get; set; }
        public List<Contact> Users { get; set; } = new List<Contact>();
        public ConversationType ChatType { get; set; }
        public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();


        

    }
}


