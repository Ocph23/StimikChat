using System;
using System.Collections.Generic;
using ModelShared.Interfaces;

namespace ModelShared
{
    public class Contact : IUser
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string Photo { get; set; }
        public List<ConversationMessage> Conversations { get; set; } = new List<ConversationMessage>();
    }
}


