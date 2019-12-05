using Microsoft.AspNetCore.SignalR;
using ModelShared;
using ModelShared.Interfaces;
using ModelShared.Models;
using StimikChatServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StimikChatServer
{
    public class ChatHub : Hub
    {
        private IChatContext chatContext;
        private IConnectionContext connectionContext;
        private IUserContext userContext;

        public ChatHub(IChatContext _chatContext, IConnectionContext _connection, IUserContext _userContext)
        {
            chatContext = _chatContext;
            connectionContext = _connection;
            userContext = _userContext;
        }

        public async Task SendMessage(ChatMessage message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
        public async Task SendMessageTo(ChatMessage message)
        {
            try
            {
                var connection = await connectionContext.GetConnectionByUserId(message.RecieverId);
                if (connection != null)
                    await Clients.Client(connection.ConnectionID).SendAsync("ReceiveMessageFrom",  message);
                else
                    this.SendByPushNotification(message.SenderId, message.Message);
                await chatContext.AddMessageMyConversation(new ChatMessage 
                { MessageId=message.MessageId, Created = DateTime.Now, Message = message.Message, Readed = false, SenderId = message.SenderId, RecieverId = message.RecieverId});

            }
            catch (Exception ex)
            {
                SendError(Context.ConnectionId, ex.Message);
            }
        }


        public async Task ReadMessage(List<ChatMessage> messages)
        {
            try
            {
                var defaultMessage = messages.FirstOrDefault();
                if(defaultMessage!=null)
                {
                    var connection = await connectionContext.GetConnectionByUserId(defaultMessage.SenderId);
                    if (connection != null)
                        await Clients.Client(connection.ConnectionID).SendAsync("OnReadMessage", messages);

                  await  chatContext.ReadedMessage(messages);
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
            IUser user = await userContext.GetByUserId(userId);
            if (user != null)
            {
                //await hubContext.Groups.CreateGroup(user.UserId, groupName);
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

                var user = userContext.GetByUserId(userId).Result;
                if (user == null)
                {
                    user = userContext.CreateUser(token).Result;
                }

                connectionContext.AddUser(user, Context.ConnectionId);
               
                /*var rooms = hubContext.Groups.GetGroupsByUserId(user.UserId).Result;
                foreach (var item in rooms)
                {
                    Groups.AddToGroupAsync(Context.ConnectionId, item.GroupName).Wait();
                }*/

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
            try
            {
                Connection con = connectionContext.Remove(Context.ConnectionId).Result;
                if (con != null)
                    OnUserChangeOnlineStatus(con.UserId, false);
                return base.OnDisconnectedAsync(exception);
            }
            catch (Exception ex)
            {
                SendError(Context.ConnectionId, ex.Message);
                return base.OnDisconnectedAsync(exception);
            }
        }

        #endregion

    }
}
