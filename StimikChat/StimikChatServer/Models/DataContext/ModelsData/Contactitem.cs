using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ocph.DAL;
 
 namespace StimikChatServer.Models.DataContext.ModelsData 
{ 
     [TableName("Contactitem")] 
     public class Contactitem :IContactitem  
   {
          [DbColumn("ContactId")] 
          public int ContactId {  get; set;} 

          [DbColumn("MemberId")] 
          public int MemberId {  get; set;} 

     }
}


