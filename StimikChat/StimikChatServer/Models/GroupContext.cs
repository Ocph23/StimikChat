using ModelShared.Models;
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
                /*  using (var db = new OcphDbContext())
                  {
                      IEnumerable<Grouproom> result = from a in db.GroupMember.Where(x => x.UserId== userId).ToList()
                                   join b in db.Groups.Select() on a.GroupId equals b.GroupId
                                   select b;
                      return Task.FromResult(result);
                  }*/

                return Task.FromResult(default(IEnumerable<Grouproom>));
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Task<IEnumerable<Grouproom>> GetGroups(int groupid)
        {
            try
            {
                return Task.FromResult(default(IEnumerable<Grouproom>));
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
                return Task.FromResult(default(List<User>));
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
                return Task.FromResult(default(List<Grouproom>));
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
                return Task.FromResult(false);
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
                return Task.FromResult(false);
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
                return Task.FromResult(default(IEnumerable<Groupmessage>));
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
                return Task.FromResult(false);
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
                return Task.FromResult(default(Grouproom));
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

    }
}
