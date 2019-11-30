using System;
 
 namespace StimikChatServer.Models
{ 
     public interface IGrouproom  
   {
         int GroupId {  get; set;} 

         DateTime Created {  get; set;} 

         int OwnerId {  get; set;} 

         string GroupName {  get; set;} 

     }
}


