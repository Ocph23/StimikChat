using System.Collections.Generic;
using ModelShared;
using ModelShared.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ModelShared.Models
{
    public class User : IUser
    {
        public string Id { get; set; }
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string Photo { get; set; }

        public List<Contact> Contacts { get; set; }

        [BsonIgnore]
        [JsonIgnore]
        public List<Conversation> Conversations { get; set; }

    }
}


