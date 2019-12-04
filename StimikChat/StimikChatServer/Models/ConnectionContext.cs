using ModelShared;
using ModelShared.Models;
using MongoDB.Driver;
using StimikChatServer.Models.DataContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StimikChatServer.Models
{
    public class ConnectionContext : IConnectionContext
    {
        private readonly IMongoCollection<Connection> _connecntions;
        public ConnectionContext(IStimikChatDatabaseSettings settings )
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _connecntions = database.GetCollection<Connection>("Connection");
        }

        public  Task<Connection> GetConnectionByUserId(int userId)
        {
            try
            {
                return _connecntions.Find<Connection>(x => x.UserId == userId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task AddUser(User user, string connectionId)
        {
            try
            {

                var opts = new FindOneAndUpdateOptions<Connection>()
                {
                    IsUpsert = true
                };
                var update = Builders<Connection>.Update
               .Set(p => p.Connected, true)
               .Set(p => p.ConnectionID, connectionId);
                await _connecntions.FindOneAndUpdateAsync<Connection>(x => x.UserId == user.UserId, update, opts);
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task<Connection> Remove(string connectionId)
        {
            try
            {
                var connection =  _connecntions.Find(x => x.ConnectionID == connectionId).FirstOrDefault();
                if(connection!=null)
                        await _connecntions.DeleteManyAsync(x => x.UserId== connection.UserId);
                return connection;
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

      
    }


    public interface IConnectionContext
    {
        Task<Connection> Remove(string connectionId);
        Task AddUser(User user, string connectionId);
        Task<Connection> GetConnectionByUserId(int userId);
    }
}