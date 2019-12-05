using ModelShared;
using ModelShared.Models;
using Newtonsoft.Json;
using StimikChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StimikChat.Data
{
    public class ContactService
    {
        private string chatServerUrl = "https://localhost:44360";
        public async Task<IEnumerable<Contact>> GetContacts(int id)
        {
            try
            {
                using (var service = new RestService(chatServerUrl))
                {
                   var resonse= await service.GetAsync($"api/contact/GetByOwenerId/{id}");
                    if(resonse.IsSuccessStatusCode)
                    {
                        var stringResult = await resonse.Content.ReadAsStringAsync();
                        var datas = JsonConvert.DeserializeObject<IEnumerable<Contact>>(stringResult);
                        return datas;
                    }

                    return new List<Contact>();
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task<List<Contact>> Find(string data)
        {
            try
            {
                using (var service = new RestService(chatServerUrl))
                {
                    var resonse = await service.GetAsync($"api/contact/Find/{data}");
                    if (resonse.IsSuccessStatusCode)
                    {
                        var stringResult = await resonse.Content.ReadAsStringAsync();
                        var datas = JsonConvert.DeserializeObject<List<Contact>>(stringResult);
                        return datas;
                    }

                    return default(List<Contact>);
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task<bool> AddToContact(int ownerId,Contact data)
        {
            try
            {
                using (var service = new RestService(chatServerUrl))
                {
                    var resonse = await service.GetAsync($"api/contact/addtocontact/{ownerId}/{data.UserId}");
                    if (resonse.IsSuccessStatusCode)
                    {
                        var stringResult = await resonse.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<bool>(stringResult);
                    }

                    return default(bool);
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }
    }


   
}
