﻿using StimikChatServer.Models.DataContext;
using StimikChatServer.Models.DataContext.ModelsData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StimikChatServer.Models
{
    public class ConnectionContext 
    {
        public Task<Connection> GetConnectionByUserId(int userId)
        {
            try
            {
                using (var db = new OcphDbContext())
                {
                    var contacts = db.Connections.Where(x => x.UserId == userId).FirstOrDefault();
                    return Task.FromResult(contacts);
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        internal Task AddUser(User user, string connectionId)
        {
            try
            {
                using (var db = new OcphDbContext())
                {
                    var contacts = db.Connections.Insert(new Connection {  UserId=user.UserId, ConnectionID=connectionId,Connected=true });
                    return Task.FromResult(contacts);
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        internal Task Remove(string connectionId)
        {
            try
            {
                using (var db = new OcphDbContext())
                {
                    var contacts = db.Connections.Delete(x=>x.ConnectionID==connectionId);
                    return Task.FromResult(contacts);
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }
    }
}