using ModelShared.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ModelShared.Models
{
    public class Connection:IConnection
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int UserId { get; set; }

        public string ConnectionID { get; set; }

        public bool Connected { get; set; }
    }
}
