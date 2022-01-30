using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IGenericRepository<Message> _repo;
        private readonly IMapper _mapper;
        public MessagesController(IGenericRepository<Message> repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet("{userId}/{friendId}")]
        public async Task<ActionResult<IReadOnlyList<MessageToReturnDto>>> GetMessagesBetweenFriends(int userId, int friendId)
        {
            var spec = new MessagesBetweenFriendsSpecification(userId, friendId);
            var messages = await _repo.ListAsync(spec);

            return Ok(_mapper.Map<IReadOnlyList<Message>, IReadOnlyList<MessageToReturnDto>>(messages));
        }
    }
}