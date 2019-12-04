using ModelShared.Interfaces;

namespace ModelShared.Models
{
     public class Groupuser :IGroupuser  
   {
          public int GroupId {  get; set;} 

          public int UserId {  get; set;} 

          public int Role {  get; set;} 

     }
}


