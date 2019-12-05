using ModelShared.Models;
using System;
using System.Collections.Generic;

namespace ModelShared.Interfaces
{ 
     public interface IUser  
   {
         int UserId {  get; set;} 

         string UserName {  get; set;} 

         string FirstName {  get; set;} 

         string Photo {  get; set;}

            List<Contact> Contacts { get; set; }



    }
}


