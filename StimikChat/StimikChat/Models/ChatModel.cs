using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StimikChat.Models
{
    public class ChatModel
    {

        public string Group { get; set; }
        public string UserName { get; set; }
        public string Destination { get; set; }
        public string Message { get; set; }
    }
}
