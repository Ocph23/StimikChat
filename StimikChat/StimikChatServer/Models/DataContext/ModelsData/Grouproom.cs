using System;
using ModelShared.Interfaces;
using Ocph.DAL;

namespace StimikChatServer.Models.DataContext.ModelsData
{
    [TableName("GroupRoom")] 
     public class Grouproom :IGrouproom  
   {
          [PrimaryKey("GroupId")] 
          [DbColumn("GroupId")] 
          public int GroupId {  get; set;} 

          [DbColumn("Created")] 
          public DateTime Created {  get; set;} 

          [DbColumn("OwnerId")] 
          public int OwnerId {  get; set;} 

          [DbColumn("GroupName")] 
          public string GroupName {  get; set;} 

     }
}


