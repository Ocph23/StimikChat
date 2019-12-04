using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelShared;
using ModelShared.Interfaces;
using ModelShared.Models;
using StimikChatServer.Models;

namespace StimikChatServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private IUserContext context;

        public ContactController(IUserContext _userContext)
        {
            context = _userContext;
        }


        // GET: api/Contact
        [HttpGet("{action}/{id}",Name ="GetContacts")]
        public async Task<IEnumerable<Contact>> GetByOwenerId(int id)
        {
            return await context.GetContactsByOwnerId(id);
        }



        [HttpGet("{action}/{name}", Name = "Find")]
        public async Task<IEnumerable<User>> Find(string name)
        {
            return await context.Find(name);
        }



        [HttpGet("{action}/{ownerId}/{userId}", Name = "AddToContact")]
        public async Task<User> AddToContact(int ownerId,int userid)
        {
            return await context.AddToContact(ownerId,userid);
        }

        // PUT: api/Contact/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
