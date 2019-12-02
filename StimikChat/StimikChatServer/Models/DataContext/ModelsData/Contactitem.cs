using ModelShared.Interfaces;
using Ocph.DAL;

namespace StimikChatServer.Models.DataContext.ModelsData
{
    
       [TableName("ContactItem")] 
     public class Contactitem :IContactitem  
   {
          [DbColumn("ContactId")] 
          public int ContactId {  get; set;} 

          [DbColumn("MemberId")] 
          public int MemberId {  get; set;} 

     }
}


