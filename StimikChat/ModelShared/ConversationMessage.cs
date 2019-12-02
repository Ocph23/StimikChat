using System;
using System.Collections.Generic;
using System.Text;

namespace ModelShared
{
    public class ConversationMessage
    {
        public string MessageId { get; set; }
        public string Message { get; set; }

        public int SenderId { get; set; }

        public int RecieveId { get; set; }

        public bool Readed { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;
    }
}
