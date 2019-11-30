using System;
 
 namespace StimikChatServer.Models
{ 
     public interface IUser  
   {
         int UserId {  get; set;} 

         string UserName {  get; set;} 

         string FirstName {  get; set;} 

         string Photo {  get; set;} 

     }
}


