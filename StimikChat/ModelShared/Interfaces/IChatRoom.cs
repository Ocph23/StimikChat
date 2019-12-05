using ModelShared.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace ModelShared.Interfaces
{
    public interface IChatRoom
    {
        ObjectId Id { get; set; }

        DateTime Created { get; set; }

        int OwnerId { get; set; }

        string RoomName { get; set; }
        List<int> Users { get; set; }

        ConversationType ChatType { get; set; }

        List<ChatMessage> Messages { get; set; }
    }


    
}


