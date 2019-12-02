using ModelShared.Interfaces;
using Ocph.DAL;

namespace StimikChatServer.Models.DataContext.ModelsData
{
    [TableName("Connection")]
    public class Connection:IConnection
    {
        [PrimaryKey("UserId")]
        [DbColumn("UserId")]
        public int UserId { get; set; }

        [DbColumn("ConnectionID")]
        public string ConnectionID { get; set; }

        [DbColumn("Connected")]
        public bool Connected { get; set; }
    }
}
