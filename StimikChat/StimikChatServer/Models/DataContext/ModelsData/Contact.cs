using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ocph.DAL;
 
 namespace StimikChatServer.Models.DataContext.ModelsData 
{ 
     [TableName("Contact")] 
     public class Contact :IContact  
   {
          [PrimaryKey("UserId")] 
          [DbColumn("UserId")] 
          public int UserId {  get; set;} 

          [DbColumn("Created")] 
          public DateTime Created {  get; set;} 

     }
}


