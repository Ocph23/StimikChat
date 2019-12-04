/*using Ocph.DAL.Provider.MySql;
using Ocph.DAL.Repository;
using StimikChatServer.Models.DataContext.ModelsData;
using System;

namespace StimikChatServer.Models.DataContext
{
    public class OcphDbContext : MySqlDbConnection,IDisposable
    {
        public OcphDbContext()
        {
          //  ConnectionString = "Server=localhost;database=stimikchatdb;uid=root;password=;port=3306";
            ConnectionString = "Server=remotemysql.com;database=JkCjm21I8b;uid=JkCjm21I8b;password=SMTi87KMkH;port=3306";
        }

        public IRepository<User> Users{ get { return new Repository<User>(this); } }
        public IRepository<ContactDto> Contacs { get { return new Repository<ContactDto>(this); } }
        public IRepository<Contactitem> ContacItems { get { return new Repository<Contactitem>(this); } }
        public IRepository<Conversation> Conversations{ get { return new Repository<Conversation>(this); } }
        public IRepository<Grouproom> Groups { get { return new Repository<Grouproom>(this); } }
        public IRepository<Groupuser> GroupMember { get { return new Repository<Groupuser>(this); } }

        public IRepository<Groupmessage> GroupMessages{ get { return new Repository<Groupmessage>(this); } }

        public IRepository<Connection> Connections { get { return new Repository<Connection>(this); } }

        public new void Dispose()
        {
            if (this.State != System.Data.ConnectionState.Closed)
                this.Close();
        }

    }
}
*/