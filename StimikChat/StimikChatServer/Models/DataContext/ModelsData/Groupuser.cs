using ModelShared.Interfaces;
using Ocph.DAL;

namespace StimikChatServer.Models.DataContext.ModelsData
{
    [TableName("GroupUser")] 
     public class Groupuser :IGroupuser  
   {
          [PrimaryKey("GroupId")] 
          [DbColumn("GroupId")] 
          public int GroupId {  get; set;} 

          [PrimaryKey("UserId")] 
          [DbColumn("UserId")] 
          public int UserId {  get; set;} 

          [DbColumn("Role")] 
          public int Role {  get; set;} 

     }
}


