using Microsoft.AspNetCore.SignalR.Client;
using ModelShared;
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
                .WithUrl($"https://localhost:44360/chatHub?userid={MyAccount.IdUser}&&token={MyAccount.Token}")
                .Build();

            Connection.On<ConversationMessage>("ReceiveMessageFrom", OnRecieveMessageFrom);
            Connection.On<ConversationMessage>("ReceiveMessage", OnRecieveMessage);
            Connection.On <List<ConversationMessage>>("OnReadMessage", OnReadMessage);

            Connection.Closed += Connection_Closed;
            await Connection.StartAsync();
            GetContacts();
            Connected = true;
        }



        #endregion

        private void OnReadMessage(List<ConversationMessage> obj)
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

        private void OnRecieveMessage(ConversationMessage obj)
        {
            //inbox Broadcase
            if (CurrentRoom != null && obj.SenderId == CurrentRoom.UserId)
            {
                CurrentRoom.Conversations.Add(obj);
                Refresh();
            }
        }

        private async void OnRecieveMessageFrom(ConversationMessage obj)
        {
            if (CurrentRoom != null && obj.SenderId == CurrentRoom.UserId)
            {
                obj.Readed = true;
                CurrentRoom.Conversations.Add(obj);
                await  ReadMessage(new List<ConversationMessage> { obj });
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

        private async void GetContacts()
        {
            var   contactService = new ContactService();
            var result = await contactService.GetContacts(MyAccount.IdUser);
            foreach(var item in result)
            {
                var room = new ConversationRoom(ConversationType.Chat, MyAccount, item);
                room.OnSendMessage += Room_OnSendMessage;
                ChatRooms.Add(room);
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

        private void Room_OnSendMessage(ConversationMessage message)
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

        public async void SendMessageTo(ConversationMessage message)
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
        private async Task ReadMessage(List<ConversationMessage> messages)
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
                    room = new ConversationRoom(ConversationType.Chat, MyAccount, userAccount);
                    room.OnSendMessage += Room_OnSendMessage;
                    this.ChatRooms.Add(room);
                }
            }

            CurrentRoom = room;
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

        }
    }

    public delegate void SendMessage(ConversationMessage message);
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
            var messageData= new ConversationMessage
            {
                MessageId = $"{myAccount.IdUser}{UserId}{guidId}",
                SenderId = myAccount.IdUser,
                RecieveId = UserId,
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
