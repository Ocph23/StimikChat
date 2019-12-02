using Microsoft.Extensions.Primitives;
using ModelShared;
using Newtonsoft.Json;
using StimikChatServer.Models.DataContext;
using StimikChatServer.Models.DataContext.ModelsData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StimikChatServer.Models
{
    public class ContactContext
    {
        public Task<List<User>> Get(int ownerId)
        {
            try
            {
                using (var db = new OcphDbContext())
                {
                    var contacts = (from a in db.ContacItems.Where(x => x.ContactId == ownerId)
                                    join b in db.Users.Select() on a.MemberId equals b.UserId
                                    join c in db.Conversations.Where(x => x.SenderId == ownerId || x.RecieverId == ownerId)
                                    on a.MemberId equals c.SenderId into revieves
                                    join d in db.Conversations.Where(x => x.SenderId == ownerId || x.RecieverId == ownerId)
                                    on a.MemberId equals d.RecieverId into sends
                                    select new User
                                    {
                                        FirstName = b.FirstName,
                                        Photo = b.Photo,
                                        UserId = b.UserId,
                                        UserName = b.UserName,
                                        SendMessaage = sends,
                                        RecieveMessage = revieves
                                    }).ToList();
                    foreach (var item in contacts)
                    {
                        var list = new List<ConversationMessage>();
                        foreach (var data in item.SendMessaage)
                        {
                            list.Add(new ConversationMessage
                            {
                                Created = data.Created,
                                Message = data.Message,
                                MessageId = data.MessageId,
                                Readed = data.Readed,
                                RecieveId = data.RecieverId,
                                SenderId = data.SenderId
                            });
                        }

                        foreach (var data in item.RecieveMessage)
                        {
                            list.Add(new ConversationMessage
                            {
                                Created = data.Created,
                                Message = data.Message,
                                MessageId = data.MessageId,
                                Readed = data.Readed,
                                RecieveId = data.RecieverId,
                                SenderId = data.SenderId
                            });
                        }

                        item.Conversations = list.OrderBy(x => x.Created).ToList();
                    }

                    return Task.FromResult(contacts.ToList());

                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Task<List<User>> Find(string data)
        {
            try
            {
                using (var db = new OcphDbContext())
                {
                    var contacts = from a in db.Users.Select().Where(x => x.UserName.ToLower().Contains(data.ToLower()) 
                                   || x.FirstName.ToLower().Contains(data.ToLower()))
                                   select a;
                    return Task.FromResult(contacts.ToList());
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Task<bool> AddToContact(int userOwner, int userId)
        {
            var data = new Contactitem { ContactId = userOwner, MemberId = userId };
            try
            {
                using (var db = new OcphDbContext())
                {
                    var result = db.ContacItems.Insert(data);

                    return Task.FromResult(result);
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        internal Task<User> GetBayUserName(int userName)
        {
            try
            {
                using (var db = new OcphDbContext())
                {
                    var contacts = db.Users.Where(x => x.UserId == userName).FirstOrDefault();
                    return Task.FromResult(contacts);
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        internal async Task<User> CreateUser(string userToken)
        {
            try
            {
                using (var service = new RestService())
                {
                    service.SetToken(userToken);
                    var profileResponse = await service.GetAsync($"api/home/getHome?role=Mahasiswa");
                    if (profileResponse.IsSuccessStatusCode)
                    {
                        var profileString = await profileResponse.Content.ReadAsStringAsync();
                        ResponseResult resResult = JsonConvert.DeserializeObject<ResponseResult>(profileString);
                        var mahasiswas = JsonConvert.DeserializeObject<List<Mahasiswa>>(resResult.data.ToString());
                        if (mahasiswas != null && mahasiswas.Count > 0)
                        {
                            var mahasiswa = mahasiswas.FirstOrDefault();
                            using (var db = new OcphDbContext())
                            {
                                var result = new User { FirstName = mahasiswa.Nmmhs, UserId = mahasiswa.IdUser, UserName = mahasiswa.Npm };
                                db.Users.Insert(result);
                                db.Contacs.Insert(new ContactDto { UserId = result.UserId, Created = DateTime.Now });
                                return result;
                            }
                        }
                        else
                            throw new SystemException("Anda Tidak Memiliki Profile");
                    }
                    else
                    {
                        throw new System.Exception(profileResponse.StatusCode.ToString());
                    }
                };
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        public Task<bool> RemoveFromContact(int userOwner, int userId)
        {
            try
            {
                using (var db = new OcphDbContext())
                {
                    var result = db.ContacItems.Delete(x => x.ContactId == userOwner && x.MemberId == userId);

                    return Task.FromResult(result);
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }


    }


    public class Mahasiswa
    {
        private string _jk;

        [JsonProperty("npm")]
        public string Npm { get; set; }

        [JsonProperty("kdps")]
        public string Kdps { get; set; }

        [JsonProperty("jenjang")]
        public string Jenjang { get; set; }

        [JsonProperty("kelas")]
        public string Kelas { get; set; }

        [JsonProperty("nmmhs")]
        public string Nmmhs { get; set; }

        [JsonProperty("tmlhr")]
        public string Tmlhr { get; set; }

        [JsonProperty("tglhr")]
        public DateTime Tglhr { get; set; }

        [JsonProperty("jk")]
        public string Jk
        {
            get { return _jk; }
            set
            {

                if (value == "L")
                    _jk = "Laki-Laki";
                else
                    _jk = "Perempuan";
            }
        }


        [JsonProperty("agama")]
        public string Agama { get; set; }

        [JsonProperty("kewarga")]
        public string Kewarga { get; set; }

        [JsonProperty("pendidikan")]
        public string Pendidikan { get; set; }

        [JsonProperty("nmsmu")]
        public string Nmsmu { get; set; }

        [JsonProperty("jursmu")]
        public string Jursmu { get; set; }

        [JsonProperty("kotasmu")]
        public string Kotasmu { get; set; }

        [JsonProperty("kabsmu")]
        public string Kabsmu { get; set; }

        [JsonProperty("provsmu")]
        public string Provsmu { get; set; }

        [JsonProperty("pekerjaan")]
        public string Pekerjaan { get; set; }

        [JsonProperty("almt")]
        public string Almt { get; set; }

        [JsonProperty("notlp")]
        public string Notlp { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("jmsaudara")]
        public int Jmsaudara { get; set; }

        [JsonProperty("nmayah")]
        public string Nmayah { get; set; }

        [JsonProperty("almtayah")]
        public string Almtayah { get; set; }

        [JsonProperty("nmibu")]
        public string Nmibu { get; set; }

        [JsonProperty("sumbiaya")]
        public string Sumbiaya { get; set; }

        [JsonProperty("statuskul")]
        public string Statuskul { get; set; }

        [JsonProperty("tgdaftar")]
        public DateTime Tgdaftar { get; set; }

        [JsonProperty("kurikulum")]
        public int Kurikulum { get; set; }

        [JsonProperty("IdUser")]
        public int IdUser { get; set; }

        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("nmps")]
        public string Nmps { get; set; }

        [JsonProperty("jenjang1")]
        public string Jenjang1 { get; set; }

        [JsonProperty("Nama")]
        public string Nama { get; set; }

        [JsonProperty("nmdsn")]
        public string Nmdsn { get; set; }

        [JsonProperty("nidn")]
        public string Nidn { get; set; }


        [JsonIgnore]
        public string TTL
        {
            get { return $"{Tmlhr}, {Tglhr.Day}/{Tglhr.Month}/{Tglhr.Year}"; }
        }

    }
}
