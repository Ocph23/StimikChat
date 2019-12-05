using ModelShared;
using ModelShared.Models;
using MongoDB.Driver;
using StimikChatServer.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StimikChatServer.Models
{
    public class ChatContext:IChatContext
    {
        private readonly IMongoCollection<ChatRoom> _context;
       // private readonly IMongoCollection<User> _userContext;

        public ChatContext(IStimikChatDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _context = database.GetCollection<ChatRoom>("Rooms");
        }
        public Task<ChatRoom> GetConversation(int sender, int reciever)
        {
            try
            {

                var filter = Builders<ChatRoom>.Filter;
                var filter1 = filter.And(filter.Eq(x => x.ChatType, ConversationType.Private), filter.ElemMatch(z => z.Users, c => c == sender && c == reciever));
                return _context.Find<ChatRoom>(filter1, new FindOptions()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Task  AddMessageMyConversation(ChatMessage message)
        {
            try
            {

                var room = new ChatRoom() { ChatType= ConversationType.Private, Created=DateTime.Now };
                room.Users = new List<int> { message.SenderId, message.RecieverId };

                var opts = new UpdateOptions()
                {
                    IsUpsert = true
                };
                var filter = Builders<ChatRoom>.Filter;
                var filter1 = filter.And(filter.Eq(x=>x.ChatType,ConversationType.Private), filter.ElemMatch(z => z.Users, c => c== message.SenderId && c==message.RecieverId));

                var update = Builders<ChatRoom>.Update;
                var data = update.Push(x=>x.Messages, message);
                var result = _context.UpdateOneAsync(filter1, data, opts);


                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }


        public Task ReadedMessage(List<ChatMessage> messages)
        {
            try
            {
                var model = messages.FirstOrDefault();
                var filter = Builders<ChatRoom>.Filter;
                var filter1 = filter.And(filter.Eq(x => x.ChatType, ConversationType.Private), filter.ElemMatch(z => z.Users, 
                    c => c == model.SenderId && c == model.RecieverId));
                var update = Builders<ChatRoom>.Update;
                var data = update.Set("Messages.$.Readed", messages);
                _context.UpdateManyAsync(filter1,data);

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
        Task ReadedMessage(List<ChatMessage> messages);
        Task AddMessageMyConversation(ChatMessage message);
        Task<ChatRoom> GetConversation(int sender, int reciever);
    }
}
