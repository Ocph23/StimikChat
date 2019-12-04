using System;
using ModelShared.Interfaces;

namespace ModelShared.Models
{
    public class Conversation : IConversation
    {

        public string MessageId { get; set; }

        public int SenderId { get; set; }

        public int RecieverId { get; set; }

        public string Message { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public bool Readed { get; set; }

    }
}


