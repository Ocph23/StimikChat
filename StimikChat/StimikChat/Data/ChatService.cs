using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace StimikChat.Data
{
    public class ChatService
    {
        public HubConnection Connection { get; set; }

        public ChatService()
        {
            
            MessageList = new ArrayList();

        }

        public ArrayList MessageList { get; set; }
        public bool Connected { get; private set; }

        public void Connect(string username)
        {
            MessageList.Clear();

            Connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44360/chatHub?userid="+ username)
                .Build();

            Connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await Connection.StartAsync();
            };


            
        }

        public async void SendButton_Click(string sender, string message)
        {
            try
            {
                await Connection.InvokeAsync("SendMessage",
                    sender, message);
            }
            catch (Exception ex)
            {
                MessageList.Add(ex.Message);
            }
        }
    }
}
