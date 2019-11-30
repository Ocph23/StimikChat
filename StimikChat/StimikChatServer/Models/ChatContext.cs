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

    }
}
