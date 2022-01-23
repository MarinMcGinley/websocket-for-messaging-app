using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendsController : ControllerBase
    {
        private readonly IFriendRepository _repo;
        public FriendsController(IFriendRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<List<Friend>>> GetFriends(int userId) {
            var friends = await _repo.GetFriends(userId);
            
            return Ok(friends);
        }

    }
}