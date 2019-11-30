using Microsoft.Extensions.Primitives;
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
        public  Task<List<User>> Get(int ownerId)
        {
            try
            {
                using (var db = new OcphDbContext())
                {
                    var contacts = from a in db.ContacItems.Where(x => x.ContactId == ownerId).ToList()
                                  join b in db.Users.Select() on a.MemberId equals b.UserId
                                  select b;
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
                    var contacts = from a in db.Users.Where(x=>x.UserName.Contains(data) || x.FirstName.Contains(data))
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

        internal Task<User> GetBayUserName(string userName)
        {
            try
            {
                using (var db = new OcphDbContext())
                {
                    var contacts = db.Users.Where(x => x.UserName == userName).FirstOrDefault();
                    return Task.FromResult(contacts);
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        internal User CreateUser(string userid)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveFromContact(int userOwner, int userId)
        {
            try
            {
                using (var db = new OcphDbContext())
                {
                    var result = db.ContacItems.Delete(x=>x.ContactId==userOwner && x.MemberId==userId);

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
