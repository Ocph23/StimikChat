using ModelShared.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace ModelShared.Interfaces
{
    public interface IChatRoom
    {

        string Id { get; set; }


        int OwnerId { get; set; }

        string RoomName { get; set; }
        List<Contact> Users { get; set; }

        ConversationType ChatType { get; set; }

        List<ChatMessage> Messages { get; set; }
    }


    
}


