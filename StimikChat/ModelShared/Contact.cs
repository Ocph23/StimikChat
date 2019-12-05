using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ModelShared.Interfaces;
using ModelShared.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ModelShared
{
    public class Contact : IContact
    {
        [JsonIgnore]
        public string Id { get; set; }
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string Photo { get; set; }
   
        public DateTime Created { get; set; }

        [BsonIgnore]
        public List<ChatMessage> Conversations { get; set; } = new List<ChatMessage>();

        public object Clone()
        {
           return this.MemberwiseClone();
        }
    }
}


