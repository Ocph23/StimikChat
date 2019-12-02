using Microsoft.AspNetCore.SignalR;
using ModelShared;
using ModelShared.Interfaces;
using StimikChatServer.Models;
using StimikChatServer.Models.DataContext.ModelsData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StimikChatServer
{
    public class ChatHub : Hub
    {
        private HubContext hubContext = new HubContext();
        public async Task SendMessage(ConversationMessage message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
        public async Task SendMessageTo(ConversationMessage message)
        {
            try
            {
                var connection = await hubContext.Connections.GetConnectionByUserId(message.RecieveId);
                if (connection != null)
                    await Clients.Client(connection.ConnectionID).SendAsync("ReceiveMessageFrom",  message);
                else
                    this.SendByPushNotification(message.SenderId, message.Message);
                await hubContext.Conversations.AddMessageMyConversation(new Conversation 
                { MessageId=message.MessageId, Created = DateTime.Now, Message = message.Message, Readed = false, SenderId = message.SenderId, RecieverId = message.RecieveId });

            }
            catch (Exception ex)
            {
                SendError(Context.ConnectionId, ex.Message);
            }
        }


        public async Task ReadMessage(List<ConversationMessage> messages)
        {
            try
            {
                var defaultMessage = messages.FirstOrDefault();
                if(defaultMessage!=null)
                {
                    var connection = await hubContext.Connections.GetConnectionByUserId(defaultMessage.SenderId);
                    if (connection != null)
                        await Clients.Client(connection.ConnectionID).SendAsync("OnReadMessage", messages);

                    hubContext.Conversations.ReadedMessage(messages);
                }
            }
            catch (Exception ex)
            {
                SendError(Context.ConnectionId, ex.Message);
            }
        }

        private void SendByPushNotification(int sender, string message)
        {
            Console.WriteLine($" Send By Push Notification {sender} , {message}");
        }

        public async Task SendToGroup(string groupname, string user, string message)
        {
            await Clients.Groups(groupname).SendAsync("RecieveFromGroup", groupname, user, message);
        }

        public async Task CreateGroup(string groupName)
        {
            var userId = Convert.ToInt32(Context.GetHttpContext().Request.Query["userid"]);
            IUser user = await hubContext.Contacts.GetBayUserName(userId);
            if (user != null)
            {
                await hubContext.Groups.CreateGroup(user.UserId, groupName);
            }
        }

        #region private

        private async void SendError(string connectionId, string message)
        {
            await Clients.Client(connectionId).SendAsync("ErroMessage", message);
        }

        private void OnUserChangeOnlineStatus(int UserId, bool status)
        {
            if(UserId>0)
                Clients.All.SendAsync("ChangeOnlineStatus", UserId, status);
        }

        #endregion


        #region Connection

        public override Task OnConnectedAsync()
        {
            try
            {
                var userId = Convert.ToInt32(Context.GetHttpContext().Request.Query["userid"]);
                string token = Context.GetHttpContext().Request.Query["token"];

                var user = hubContext.Contacts.GetBayUserName(userId).Result;
                if (user == null)
                {
                    user = hubContext.Contacts.CreateUser(token).Result;
                }

                hubContext.Connections.AddUser(user, Context.ConnectionId).Wait();
               
                var rooms = hubContext.Groups.GetGroupsByUserId(user.UserId).Result;
                foreach (var item in rooms)
                {
                    Groups.AddToGroupAsync(Context.ConnectionId, item.GroupName).Wait();
                }

                OnUserChangeOnlineStatus(user.UserId, true);
            }
            catch (Exception ex)
            {
                SendError(Context.ConnectionId, ex.Message);
            }

            return base.OnConnectedAsync();
        }


        public override Task OnDisconnectedAsync(Exception exception)
        {
            int userId = hubContext.Connections.Remove(Context.ConnectionId).Result;
            OnUserChangeOnlineStatus(userId, false);
            return base.OnDisconnectedAsync(exception);
        }

        #endregion

    }
}
