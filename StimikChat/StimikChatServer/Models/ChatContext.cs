using ModelShared;
using StimikChatServer.Models.DataContext;
using StimikChatServer.Models.DataContext.ModelsData;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StimikChatServer.Models
{
    public class ChatContext
    {
        public Task<IEnumerable<Conversation>> GetConversation(int owner)
        {
            try
            {
                using (var db = new OcphDbContext())
                {
                    IEnumerable<Conversation> result = db.Conversations.Where(x => x.SenderId == owner || x.RecieverId==owner);

                    return Task.FromResult(result);
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Task<bool> AddMessageMyConversation(Conversation message)
        {
            try
            {
                using (var db = new OcphDbContext())
                {
                    var result = db.Conversations.Insert(message);

                    return Task.FromResult(result);
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        internal async void ReadedMessage(List<ConversationMessage> messages)
        {
            await Task.Delay(100);
            try
            {
                using (var db = new OcphDbContext())
                {
                    foreach(var item in messages)
                    {
                        db.Conversations.Update(x => new { x.Readed }, new Conversation { MessageId = item.MessageId, Readed = item.Readed }, x => x.MessageId == item.MessageId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }
    }
}
