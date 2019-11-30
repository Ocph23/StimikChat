using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StimikChatServer.Models
{
    public class HubContext
    {

        public ChatContext Conversations { get { return new ChatContext(); } }

        public GroupContext Groups { get { return new GroupContext(); } }

        public ContactContext Contacts { get { return new ContactContext(); } }

        public ConnectionContext Connections { get { return new ConnectionContext(); } }

    }
}
