using Microsoft.AspNetCore.SignalR;
using StimikChatServer.Models;
using StimikChatServer.Models.DataContext;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StimikChatServer
{
    public class ChatHub :Hub
    {
        private  HubContext hubContext = new  HubContext();
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task SendMessageTo(int recieverId, int sender, string message)
        {
            try
            {
                var connection = await hubContext.Connections.GetConnectionByUserId(recieverId);
                await Clients.Client(connection.ConnectionID).SendAsync("ReceiveFromMessage", sender, message);
            }
            catch (Exception ex)
            {
               SendError(Context.ConnectionId, ex.Message);
            }
        }

        private async void SendError(string connectionId, string message)
        {
            await Clients.Client(connectionId).SendAsync("ErroMessage", message);
        }

        public async Task SendToGroup(string groupname,string user, string message)
        {
           await Clients.Groups(groupname).SendAsync("RecieveFromGroup",groupname, user, message);
        }

        public async Task CreateGroup(string groupName)
        {
            await Task.Delay(100);
            var userName = Context.GetHttpContext().Request.Query["userid"];
            IUser  user = await hubContext.Contacts.GetBayUserName(userName);
            if(user!=null)
            {
                await hubContext.Groups.CreateGroup(user.UserId, groupName);
            }
        }


        public override  Task OnConnectedAsync()
        {
            try
            {
                string userid = Context.GetHttpContext().Request.Query["userid"];

                var user= hubContext.Contacts.GetBayUserName(userid).Result;
                if (user == null)
                {
                    user = hubContext.Contacts.CreateUser(userid);
                }

                hubContext.Connections.AddUser(user, Context.ConnectionId);

                var rooms = hubContext.Groups.GetGroupsByUserId(user.UserId).Result;
                foreach (var item in rooms)
                {
                   Groups.AddToGroupAsync(Context.ConnectionId, item.GroupName);
                }

               
            }
            catch (Exception ex)
            {
                SendError(Context.ConnectionId, ex.Message);
            }
            return base.OnConnectedAsync();

        }


        public override Task OnDisconnectedAsync(Exception exception)
        {
            hubContext.Connections.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
     
       
    }
}
