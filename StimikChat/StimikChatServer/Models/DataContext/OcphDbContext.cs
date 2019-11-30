﻿using Ocph.DAL.Provider.MySql;
using Ocph.DAL.Repository;
using StimikChatServer.Models.DataContext.ModelsData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StimikChatServer.Models.DataContext
{
    public class OcphDbContext : MySqlDbConnection
    {
        public OcphDbContext()
        {
            ConnectionString = "Server=localhost;database=stimikchatdb;uid=root;password=;port=3306";
        }


        public IRepository<User> Users{ get { return new Repository<User>(this); } }
        public IRepository<Contact> Contacs { get { return new Repository<Contact>(this); } }
        public IRepository<Contactitem> ContacItems { get { return new Repository<Contactitem>(this); } }
        public IRepository<Conversation> Conversations{ get { return new Repository<Conversation>(this); } }
        public IRepository<Grouproom> Groups { get { return new Repository<Grouproom>(this); } }
        public IRepository<Groupuser> GroupMember { get { return new Repository<Groupuser>(this); } }

        public IRepository<Groupmessage> GroupMessages{ get { return new Repository<Groupmessage>(this); } }

        public IRepository<Connection> Connections { get { return new Repository<Connection>(this); } }


    }
}
