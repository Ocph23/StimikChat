using ModelShared;
using ModelShared.Models;
using MongoDB.Driver;
using StimikChatServer.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StimikChatServer.Models
{
    public class ChatContext:IChatContext
    {
        private readonly IMongoCollection<Conversation> _context;
        public ChatContext(IStimikChatDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _context = database.GetCollection<Conversation>("Conversation");
        }
        public Task<List<Conversation>> GetConversation(int owner)
        {
            try
            {
                return _context.Find<Conversation>(x => x.SenderId == owner || x.RecieverId==owner).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Task  AddMessageMyConversation(Conversation message)
        {
            try
            {

               return _context.InsertOneAsync(message);
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }


        public Task ReadedMessage(List<Conversation> messages)
        {
            try
            {
                foreach (var item in messages)
                {
                    _context.ReplaceOne(x => x.MessageId==item.MessageId, item);
                }

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }
    }

    public interface IChatContext
    {
        Task ReadedMessage(List<Conversation> messages);
        Task AddMessageMyConversation(Conversation message);
        Task<List<Conversation>> GetConversation(int owner);
    }
}
