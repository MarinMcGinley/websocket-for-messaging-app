using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageRepository _repo;
        public MessagesController(IMessageRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("{userId}/{friendId}")]
        public async Task<ActionResult<List<Message>>> GetMessagesBetweenFriends(int userId, int friendId) {
            var messages = await _repo.GetMessagesBetweenFriendsAsync(userId, friendId);
            
            return Ok(messages);
        }
    }
}