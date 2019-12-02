using StimikChat.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StimikChat.Models
{
    public class ChatRoom
    {
        public ChatRoom()
        {

        }
        public ChatRoom(ChatService service)
        {
            Service = service;
        }

        public ChatService Service { get; }
    }
}
