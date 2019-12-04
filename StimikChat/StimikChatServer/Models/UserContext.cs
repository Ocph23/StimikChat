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
        public UserContext(IStimikChatDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _context = database.GetCollection<User>("Users");
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

        public async Task<User> AddToContact(int userOwner, int userId)
        {
            var data = _context.Find(x => x.UserId == userOwner).FirstOrDefault();
            var newUser = _context.Find(x => x.UserId == userId).FirstOrDefault();

            data.Contacts.Add(new Contact { UserId=newUser.UserId, UserName=newUser.UserName, FirstName=newUser.FirstName });

            return await _context.FindOneAndReplaceAsync(x=>x.UserId==data.UserId, data);

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
                            var model = new User { FirstName = mahasiswa.Nmmhs, UserId = mahasiswa.IdUser, UserName = mahasiswa.Npm, Contacts=new List<Contact>() };
                            model.Id = Guid.NewGuid().ToString();
                            var op = new InsertOneOptions();
                            await _context.InsertOneAsync(model);
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

        Task<User> AddToContact(int userOwner, int userId);
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
