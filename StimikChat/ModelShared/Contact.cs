using System;
using System.Collections.Generic;
using ModelShared.Interfaces;
using ModelShared.Models;

namespace ModelShared
{
    public class Contact : IUser
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string Photo { get; set; }

   
        public DateTime Created { get; internal set; }

        public List<Conversation> Conversations { get; set; } = new List<Conversation>();
    }
}


