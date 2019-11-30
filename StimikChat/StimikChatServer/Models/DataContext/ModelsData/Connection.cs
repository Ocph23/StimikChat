using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StimikChatServer.Models.DataContext.ModelsData
{
    [TableName("connection")]
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
