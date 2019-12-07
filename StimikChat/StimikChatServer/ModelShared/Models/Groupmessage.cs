using System;
using ModelShared.Interfaces;

namespace ModelShared.Models
{
     public class Groupmessage :IGroupmessage  
   {
          public int RoomId {  get; set;} 

          public int GroupId {  get; set;} 

          public int SenderId {  get; set;} 

          public DateTime Created {  get; set;} 

          public string Message {  get; set;} 

          public int MessageType {  get; set;} 

     }
}


