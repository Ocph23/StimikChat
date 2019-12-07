using Microsoft.AspNetCore.SignalR.Client;
using ModelShared;
using ModelShared.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StimikChat.Data
{

    public delegate void Refresh();
    public class ChatService
    {
        public event Refresh OnRefresh;

        public HubConnection Connection { get; set; }
        public UserAccount MyAccount { get; set; }
        public List<ConversationRoom> ChatRooms { get; set; } = new List<ConversationRoom>();
        public bool Connected { get;  set; }
        public ConversationRoom CurrentRoom { get; set; }

        #region Connection
        private async Task Connection_Closed(Exception arg)
        {
            Connected = false;
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await Connection.StartAsync();
        }

        public async void Connect(UserAccount account)
        {
            MyAccount = account;
            Connection = new HubConnectionBuilder()
                .WithUrl($"https://stimikchatapi.herokuapp.com/chatHub?userid={MyAccount.IdUser}&&token={MyAccount.Token}")
                .Build();

            Connection.On<ChatMessage>("ReceiveMessageFrom", OnRecieveMessageFrom);
            Connection.On<ChatMessage>("ReceiveMessage", OnRecieveMessage);
            Connection.On <List<ChatMessage>>("OnReadMessage", OnReadMessage);

            Connection.Closed += Connection_Closed;
            await Connection.StartAsync();
            GetProfile();
            Connected = true;
        }



        #endregion

        private void OnReadMessage(List<ChatMessage> obj)
        {
            var messages = from a in ChatRooms.SelectMany(x => x.Conversations)
                           join d in obj on a.SenderId equals d.SenderId
                           select a;

            foreach(var item in messages.ToList())
            {
                item.Readed = true;
            }
            Refresh();
        }

        private void OnRecieveMessage(ChatMessage obj)
        {
            //inbox Broadcase
            if (CurrentRoom != null && obj.SenderId == CurrentRoom.UserId)
            {
                CurrentRoom.Conversations.Add(obj);
                Refresh();
            }
        }

        private async void OnRecieveMessageFrom(ChatMessage obj)
        {
            if (CurrentRoom != null && obj.SenderId == CurrentRoom.UserId)
            {
                obj.Readed = true;
                CurrentRoom.Conversations.Add(obj);
                await  ReadMessage(new List<ChatMessage> { obj });
                Refresh();
            }
            else
            {
                var room = ChatRooms.Where(x => x.UserId == obj.SenderId).FirstOrDefault();
                if (room != null)
                {
                    room.Conversations.Add(obj);
                }
            }
        }

        #region RecieveMethod 

        #endregion

        private async void GetProfile()
        {
            var   contactService = new ContactService();
            var result = await contactService.GetProfile(MyAccount.IdUser);
            if (result != null && result.Contacts!=null)
            {
                foreach (var item in result.Contacts)
                {
                    var room = new ConversationRoom(ConversationType.Private, MyAccount, item);
                    room.OnSendMessage += Room_OnSendMessage;
                    ChatRooms.Add(room);
                }
            }
        }

        public async void Refresh()
        {
            await Task.Delay(500);
            if(OnRefresh!=null)
            {
                OnRefresh();
            }
        }

        private void Room_OnSendMessage(ChatMessage message)
        {
            SendMessageTo(message);
        }

        public async void SendMessage(string message)
        {
            try
            {
                if(Connection.State== HubConnectionState.Disconnected)
                {
                    await Task.Delay(new Random().Next(0, 5) * 1000);
                }
                await Connection.InvokeAsync("SendMessage",MyAccount.IdUser, message);
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async void SendMessageTo(ChatMessage message)
        {
            try
            {
                if (Connection.State == HubConnectionState.Disconnected)
                {
                    await Task.Delay(new Random().Next(0, 5) * 1000);
                }
                await Connection.InvokeAsync("SendMessageTo",message);
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }
        private async Task ReadMessage(List<ChatMessage> messages)
        {
            try
            {
                await Task.Delay(200);
                await Connection.InvokeAsync("ReadMessage", messages);
            }
            catch (Exception)
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await Connection.InvokeAsync("ReadMessage", messages);
            }
            finally
            {

            }
        }

        public async void SetCurrentRoom(int contactId)
        {
            var room = ChatRooms.Where(x => x.UserId == contactId).FirstOrDefault();
            if(room==null)
            {
                var userAccount = MyAccount.Contacts.Where(x => x.UserId == contactId).FirstOrDefault();
                if(userAccount!=null)
                {
                    room = new ConversationRoom(ConversationType.Private, MyAccount, userAccount);
                    room.OnSendMessage += Room_OnSendMessage;
                    this.ChatRooms.Add(room);
                }
            }

            CurrentRoom = room;

            if(CurrentRoom!=null && CurrentRoom.Conversations!=null && CurrentRoom.Conversations.Count<=0 )
            {
                var conversationService = new ConversationService();
                var chatRoom = await conversationService.GetConversations(MyAccount.IdUser, contactId);
                if(chatRoom!=null)
                foreach (var item in chatRoom.Messages)
                {
                    CurrentRoom.Conversations.Add(item);
                } 
            }

          

            if(CurrentRoom!=null && CurrentRoom.Conversations != null)
            {
                var unReaded = CurrentRoom.Conversations.Where(x =>x.SenderId==CurrentRoom.UserId && x.Readed==false).ToList();
                if (unReaded!=null && unReaded.Count() > 0)
                {
                    foreach (var item in unReaded)
                    {
                        item.Readed = true;
                    }
                    await ReadMessage(unReaded.ToList());
                }
               

            }

              this.Refresh();

        }
    }

    public delegate void SendMessage(ChatMessage message);
    public class ConversationRoom :Contact
    {
        public event SendMessage OnSendMessage;

        public ConversationRoom(ConversationType type, UserAccount myAccount, Contact contact)
        {
            this.ConversationType = type;
            this.myAccount = myAccount;

            this.FirstName = contact.FirstName;
            this.Photo = contact.Photo;
            this.UserId = contact.UserId;
            this.UserName = contact.UserName;
            if(contact.Conversations!=null)
                this.Conversations = contact.Conversations;
        }

        public ConversationType ConversationType { get; }
        public UserAccount myAccount { get; }

        public void SendMessage(string message)
        {
            var guidId = Guid.NewGuid().ToString();
            var messageData= new ChatMessage
            {
                MessageId = $"{myAccount.IdUser}{UserId}{guidId}",
                SenderId = myAccount.IdUser,
                RecieverId= UserId,
                Message = message
            };

            Conversations.Add(messageData);
            if(OnSendMessage!=null)
            {
                OnSendMessage(messageData);
            }

        }

    }

   


    
}
