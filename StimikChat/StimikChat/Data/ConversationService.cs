﻿using ModelShared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StimikChat.Data
{
    public class ConversationService
    {
        public async Task<List<ConversationMessage>> GetConversations(int id)
        {
            try
            {
                using (var service = new RestService("https://localhost:44360"))
                {
                    var resonse = await service.GetAsync($"api/conversation/{id}");
                    if (resonse.IsSuccessStatusCode)
                    {
                        var stringResult = await resonse.Content.ReadAsStringAsync();
                        var datas = JsonConvert.DeserializeObject<List<ConversationMessage>>(stringResult);
                        return datas;
                    }

                    return default(List<ConversationMessage>);
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

    }
}