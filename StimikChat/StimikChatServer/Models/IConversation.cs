using System;
 
 namespace StimikChatServer.Models
{ 
     public interface IConversation  
   {
         int SenderId {  get; set;} 

         int RecieverId {  get; set;} 

         string Message {  get; set;} 

         DateTime Created {  get; set;} 

         int Readed {  get; set;} 

     }
}


