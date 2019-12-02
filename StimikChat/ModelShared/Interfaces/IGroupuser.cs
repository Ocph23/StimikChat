using System;
 
 namespace ModelShared.Interfaces
{ 
     public interface IGroupuser  
   {
         int GroupId {  get; set;} 

         int UserId {  get; set;} 

         int Role {  get; set;} 

     }
}


