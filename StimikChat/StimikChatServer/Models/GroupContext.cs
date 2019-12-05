using ModelShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StimikChatServer.Models
{
    public class GroupContext
    {
        public Task<IEnumerable<ChatRoom>> GetGroupsByUserId(int userId)
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

                return Task.FromResult(default(IEnumerable<ChatRoom>));
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Task<IEnumerable<ChatRoom>> GetGroups(int groupid)
        {
            try
            {
                return Task.FromResult(default(IEnumerable<ChatRoom>));
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



        public Task<List<ChatRoom>> Find(string groupName)
        {
            try
            {
                return Task.FromResult(default(List<ChatRoom>));
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

        public Task<ChatRoom>CreateGroup(int owner ,string groupName)
        {
            try
            {
                return Task.FromResult(default(ChatRoom));
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

    }
}
