using StimikChatServer.Models.DataContext;
using StimikChatServer.Models.DataContext.ModelsData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StimikChatServer.Models
{
    public class GroupContext
    {
        public Task<IEnumerable<Grouproom>> GetGroupsByUserId(int userId)
        {
            try
            {
                using (var db = new OcphDbContext())
                {
                    IEnumerable<Grouproom> result = from a in db.GroupMember.Where(x => x.UserId== userId).ToList()
                                 join b in db.Groups.Select() on a.GroupId equals b.GroupId
                                 select b;
                    return Task.FromResult(result);
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Task<List<Grouproom>> GetGroups(int groupid)
        {
            try
            {
                using (var db = new OcphDbContext())
                {
                    var result = from a in db.Groups.Where(x=>x.GroupId==groupid).ToList()
                                   select a;
                    return Task.FromResult(result.ToList());
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Task<List<User>> GroupMembers(int groupid)
        {
            try
            {
                using (var db = new OcphDbContext())
                {
                    var result = from a in db.GroupMember.Where(x => x.GroupId == groupid).ToList()
                                 join b in db.Users.Select() on a.UserId equals b.UserId
                                 select b;
                    return Task.FromResult(result.ToList());
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }



        public Task<List<Grouproom>> Find(string groupName)
        {
            try
            {
                using (var db = new OcphDbContext())
                {
                    var contacts = from a in db.Groups.Where(x => x.GroupName.Contains(groupName))
                                   select a;
                    return Task.FromResult(contacts.ToList());
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Task<bool> AddToMemberGroup(int groupId, int userId)
        {
            var data = new Groupuser { GroupId = groupId,   UserId= userId };
            try
            {
                using (var db = new OcphDbContext())
                {
                    var result = db.GroupMember.Insert(data);

                    return Task.FromResult(result);
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Task<bool> RemoveMemberFromGroup(int groupId, int userId)
        {
            try
            {
                using (var db = new OcphDbContext())
                {
                    var result = db.GroupMember.Delete(x => x.GroupId == groupId&& x.UserId== userId);

                    return Task.FromResult(result);
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }


        public Task<IEnumerable<Groupmessage>> GetMessagesFromGroup(int groupId)
        {
            try
            {
                using (var db = new OcphDbContext())
                {
                    IEnumerable<Groupmessage> result = db.GroupMessages.Where(x => x.GroupId == groupId);

                    return Task.FromResult(result);
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Task<bool> AddMessageToGroup(Groupmessage message)
        {
            try
            {
                using (var db = new OcphDbContext())
                {
                   var  result = db.GroupMessages.Insert(message);

                    return Task.FromResult(result);
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Task<Grouproom>CreateGroup(int owner ,string groupName)
        {
            try
            {
                var model = new Grouproom { GroupName = groupName, Created=DateTime.Now, OwnerId= owner};
                using (var db = new OcphDbContext())
                {
                    model.GroupId = db.Groups.InsertAndGetLastID(model);

                    return Task.FromResult(model);
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

    }
}
