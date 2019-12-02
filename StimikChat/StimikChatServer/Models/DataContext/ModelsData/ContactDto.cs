using System;
using ModelShared.Interfaces;
using Ocph.DAL;

namespace StimikChatServer.Models.DataContext.ModelsData
{
    [TableName("Contact")] 
     public class ContactDto : IContact  
   {
          [PrimaryKey("UserId")] 
          [DbColumn("UserId")] 
          public int UserId {  get; set;} 

          [DbColumn("Created")] 
          public DateTime Created {  get; set;} 

     }
}


