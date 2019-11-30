using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ocph.DAL;
 
 namespace StimikChatServer.Models.DataContext.ModelsData 
{ 
     [TableName("Groupmessage")] 
     public class Groupmessage :IGroupmessage  
   {
          [PrimaryKey("RoomId")] 
          [DbColumn("RoomId")] 
          public int RoomId {  get; set;} 

          [DbColumn("GroupId")] 
          public int GroupId {  get; set;} 

          [DbColumn("SenderId")] 
          public int SenderId {  get; set;} 

          [DbColumn("Created")] 
          public DateTime Created {  get; set;} 

          [DbColumn("Message")] 
          public string Message {  get; set;} 

          [DbColumn("MessageType")] 
          public int MessageType {  get; set;} 

     }
}


