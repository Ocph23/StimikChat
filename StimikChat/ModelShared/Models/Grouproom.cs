using System;
using ModelShared.Interfaces;

namespace ModelShared.Models
{
     public class Grouproom :IGrouproom  
   {
          public int GroupId {  get; set;} 

          public DateTime Created {  get; set;} 

          public int OwnerId {  get; set;} 

          public string GroupName {  get; set;} 

     }
}


