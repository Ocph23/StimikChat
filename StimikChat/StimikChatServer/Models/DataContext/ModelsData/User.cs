using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ocph.DAL;
 
 namespace StimikChatServer.Models.DataContext.ModelsData 
{ 
     [TableName("User")] 
     public class User :IUser  
   {
          [PrimaryKey("UserId")] 
          [DbColumn("UserId")] 
          public int UserId {  get; set;} 

          [DbColumn("UserName")] 
          public string UserName {  get; set;} 

          [DbColumn("FirstName")] 
          public string FirstName {  get; set;} 

          [DbColumn("Photo")] 
          public string Photo {  get; set;} 

     }
}


