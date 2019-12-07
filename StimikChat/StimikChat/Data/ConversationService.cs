using ModelShared;
using ModelShared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StimikChat.Data
{
    public class ConversationService
    {
        public async Task<ChatRoom> GetConversations(int id,int contactId)
        {
            try
            {
                using (var service = new RestService("http://localhost:54340"))
                {
                    var resonse = await service.GetAsync($"api/conversation/{id}/{contactId}");
                    if (resonse.IsSuccessStatusCode)
                    {
                        var stringResult = await resonse.Content.ReadAsStringAsync();
                        var datas = JsonConvert.DeserializeObject<ChatRoom>(stringResult);
                        return datas;
                    }

                    return default(ChatRoom);
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

    }
}
