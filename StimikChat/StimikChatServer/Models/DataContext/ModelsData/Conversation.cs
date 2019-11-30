using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ocph.DAL;
 
 namespace StimikChatServer.Models.DataContext.ModelsData 
{ 
     [TableName("Conversation")] 
     public class Conversation :IConversation  
   {
          [PrimaryKey("SenderId")] 
          [DbColumn("SenderId")] 
          public int SenderId {  get; set;} 

          [PrimaryKey("RecieverId")] 
          [DbColumn("RecieverId")] 
          public int RecieverId {  get; set;} 

          [DbColumn("Message")] 
          public string Message {  get; set;} 

          [DbColumn("Created")] 
          public DateTime Created {  get; set;} 

          [DbColumn("Readed")] 
          public int Readed {  get; set;} 

     }
}


