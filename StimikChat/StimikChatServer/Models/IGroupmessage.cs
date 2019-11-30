using System;
 
 namespace StimikChatServer.Models
{ 
     public interface IGroupmessage  
   {
         int RoomId {  get; set;} 

         int GroupId {  get; set;} 

         int SenderId {  get; set;} 

         DateTime Created {  get; set;} 

         string Message {  get; set;} 

         int MessageType {  get; set;} 

     }
}


