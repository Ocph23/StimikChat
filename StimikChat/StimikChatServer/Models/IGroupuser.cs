using System;
 
 namespace StimikChatServer.Models
{ 
     public interface IGroupuser  
   {
         int GroupId {  get; set;} 

         int UserId {  get; set;} 

         int Role {  get; set;} 

     }
}


