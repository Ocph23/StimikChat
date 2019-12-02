using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelShared.Interfaces;
using StimikChatServer.Models;

namespace StimikChatServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private HubContext hubContext;

        public ContactController()
        {
              hubContext = new HubContext();
        }
        // GET: api/Contact
        [HttpGet("{action}/{id}",Name ="GetContacts")]
        public async Task<IEnumerable<IUser>> GetByOwenerId(int id)
        {
            return await hubContext.Contacts.Get(id);
        }



        [HttpGet("{action}/{name}", Name = "Find")]
        public async Task<IEnumerable<IUser>> Find(string name)
        {
            return await hubContext.Contacts.Find(name);
        }



        [HttpGet("{action}/{ownerId}/{userId}", Name = "AddToContact")]
        public async Task<bool> AddToContact(int ownerId,int userid)
        {
            return await hubContext.Contacts.AddToContact(ownerId,userid);
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
