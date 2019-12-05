using ModelShared.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace ModelShared.Interfaces
{
    public interface IContact:ICloneable
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string Photo { get; set; }


        public DateTime Created { get; set; }
        List<ChatMessage> Conversations { get; set; }


    }
}
