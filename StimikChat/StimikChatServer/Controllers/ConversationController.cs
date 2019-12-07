using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelShared;
using ModelShared.Models;
using StimikChatServer.Models;

namespace StimikChatServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private IChatContext chatContext;

        public ConversationController(IChatContext context)
        {
            chatContext = context;
        }
        // GET: api/Conversation/5
        [HttpGet("{senderId}/{recieverId}", Name = "Get")]
        public async Task<ChatRoom> Get(int senderId, int recieverId)
        {
            return await chatContext.GetConversation(senderId, senderId);
        }

        // POST: api/Conversation
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Conversation/5
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
