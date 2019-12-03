using System;
 
 namespace ModelShared.Interfaces
{ 
     public interface IConversation  
   {
         int SenderId {  get; set;} 

         int RecieverId {  get; set;} 

         string Message {  get; set;} 

         DateTime Created {  get; set;} 

         bool Readed {  get; set;} 

     }
}


