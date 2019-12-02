using System;
using ModelShared.Interfaces;
using Ocph.DAL;

namespace StimikChatServer.Models.DataContext.ModelsData
{
    [TableName("Conversation")]
    public class Conversation : IConversation
    {

        [PrimaryKey("MessageId")]
        [DbColumn("MessageId")]
        public string MessageId { get; set; }

        [DbColumn("SenderId")]
        public int SenderId { get; set; }

        [DbColumn("RecieverId")]
        public int RecieverId { get; set; }

        [DbColumn("Message")]
        public string Message { get; set; }

        [DbColumn("Created")]
        public DateTime Created { get; set; }

        [DbColumn("Readed")]
        public bool Readed { get; set; }

    }
}


