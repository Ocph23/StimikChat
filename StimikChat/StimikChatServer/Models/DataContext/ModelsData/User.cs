using System.Collections.Generic;
using ModelShared;
using ModelShared.Interfaces;
using Ocph.DAL;

namespace StimikChatServer.Models.DataContext.ModelsData
{
    [TableName("User")]
    public class User : IUser
    {
        [PrimaryKey("UserId")]
        [DbColumn("UserId")]
        public int UserId { get; set; }

        [DbColumn("UserName")]
        public string UserName { get; set; }

        [DbColumn("FirstName")]
        public string FirstName { get; set; }

        [DbColumn("Photo")]
        public string Photo { get; set; }
        public IEnumerable<Conversation> SendMessaage { get; set; }
        public IEnumerable<Conversation> RecieveMessage { get; set; }
        public List<ConversationMessage> Conversations { get; set; }
    }
}


