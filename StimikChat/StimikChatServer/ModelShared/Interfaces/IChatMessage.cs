using ModelShared.Models;
using System;

namespace ModelShared.Interfaces
{ 
     public interface IChatMessage  
   {
       
         string Message {  get; set;} 

         DateTime Created {  get; set;} 

         bool Readed {  get; set;}

        int SenderId { get; set; }
        int RecieverId { get; set; }

        MessageType MessageType { get; set; }

    }
}


