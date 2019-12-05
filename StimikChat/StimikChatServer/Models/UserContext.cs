using Microsoft.Extensions.Primitives;
using ModelShared;
using ModelShared.Models;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StimikChatServer.Models.DataContext;

namespace StimikChatServer.Models
{
    public class UserContext: IUserContext
    {
        private readonly IMongoCollection<User> _context;
        private readonly IMongoCollection<ChatRoom> _chatContext;

        public UserContext(IStimikChatDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _context = database.GetCollection<User>("Users");
            _chatContext = database.GetCollection<ChatRoom>("Rooms");
        }
        public Task<User> GetUserByUserId(int ownerId)
        {
            try
            {
              return _context.Find(x => x.UserId == ownerId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task<List<Contact>> GetContactsByOwnerId(int ownerId)
        {
            try
            {
                await Task.Delay(200);
                var data= await _context.Find(x => x.UserId == ownerId).FirstOrDefaultAsync();
                return data.Contacts;
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

                return _context.Find(x => x.FirstName.ToLower().Contains(data.ToLower()) || x.UserName.Contains(data)).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Task<bool> AddToContact(int SenderId, int RecieverId)
        {
            try
            {
                var opts = new UpdateOptions()
                {
                    IsUpsert = true
                };

                //add recieve to sender
                var model1 = _context.Find(x => x.UserId == SenderId).FirstOrDefault();
                var model2 = _context.Find(x => x.UserId == RecieverId).FirstOrDefault();


                var filter1 = Builders<User>.Filter.And(Builders<User>.Filter.Eq(x => x.UserId, SenderId),
                    Builders<User>.Filter.ElemMatch(x => x.Contacts, x => x.UserId == RecieverId));
                var data1 = Builders<User>.Update.Push(x => x.Contacts, new Contact
                {
                    Created = DateTime.Now,
                    UserId = model2.UserId,
                    FirstName = model2.FirstName,
                    UserName = model2.UserName,
                    Photo = model2.Photo
                });

                //add sender to sender
                var filter2 = Builders <User> .Filter.And(Builders<User>.Filter.Eq(x => x.UserId, RecieverId),
                    Builders<User>.Filter.ElemMatch(x => x.Contacts, x => x.UserId == SenderId));
                var data2 = Builders<User>.Update.Push(x => x.Contacts, new Contact
                {
                    Created = DateTime.Now,
                    UserId = model1.UserId,
                    FirstName = model1.FirstName,
                    UserName = model1.UserName,
                    Photo = model1.Photo
                });

                var result = _context.UpdateOneAsync(filter1, data1, opts);
                var result1 = _context.UpdateOneAsync(filter2, data2, opts);


                //Create Room

                var room = new ChatRoom() { ChatType = ConversationType.Private, Created = DateTime.Now };
                room.Users = new List<int> { SenderId, RecieverId };

                var filter = Builders<ChatRoom>.Filter;
                var filterRoom = filter.And(filter.Eq(x => x.ChatType, ConversationType.Private),
                    filter.ElemMatch(z => z.Users, c => c == SenderId && c == RecieverId));

                var update = Builders<ChatRoom>.Update;
                var data = update.SetOnInsert(x => x.ChatType, ConversationType.Private)
                    .SetOnInsert(x => x.Created, DateTime.Now)
                    .PushEach(x => x.Users, room.Users);
                var resultRoom = _chatContext.UpdateOneAsync(filterRoom, data, opts);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Task<User> GetByUserId(int userName)
        {
            try
            {
                return _context.Find(x => x.UserId == userName).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task<User> CreateUser(string userToken)
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
                            var opts = new UpdateOptions()
                            {
                                IsUpsert = true
                            };
                            var model = new User { FirstName = mahasiswa.Nmmhs, UserId = mahasiswa.IdUser, UserName = mahasiswa.Npm, Contacts=new List<Contact>() };
                            var filter1 = Builders<User>.Filter.And(Builders<User>.Filter.Eq(x => x.UserId, model.UserId),
                                 Builders<User>.Filter.ElemMatch(x => x.Contacts, x => x.UserId == model.UserId));
                            var data1 = Builders<User>.Update
                                .Set(x =>x.FirstName,model.FirstName)
                                .Set(x=>x.Photo,model.Photo)
                                .Set(x=>x.UserName,model.UserName);
                            var resultRoom = _context.UpdateOneAsync(filter1, data1, opts);
                            return model;
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

        public Task<User> RemoveFromContact(int userOwner, int userId)
        {
            try
            {
                var data = _context.Find(x => x.UserId == userOwner).FirstOrDefault();
                var removeData =data.Contacts.Where(x=>x.UserId==userId).FirstOrDefault();
                data.Contacts.Remove(removeData);
               return _context.FindOneAndReplaceAsync(x => x.UserId == data.UserId, data);
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }


    }

    public interface IUserContext
    {
        Task<User> GetUserByUserId(int ownerId);
        Task<List<Contact>> GetContactsByOwnerId(int ownerId);
        Task<List<User>> Find(string data);
        Task<bool> AddToContact(int userOwner, int userId);
        Task<User> GetByUserId(int userName);
        Task<User> CreateUser(string userToken);
        Task<User> RemoveFromContact(int userOwner, int userId);
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
