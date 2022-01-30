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
    public class FriendsController : ControllerBase
    {
        private readonly IGenericRepository<Friend> _repo;
        private readonly IMapper _mapper;
        public FriendsController(IGenericRepository<Friend> friendRepo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = friendRepo;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IReadOnlyList<FriendToReturnDto>>> GetFriends(int userId)
        {
            var spec = new FriendsWithUsersSpecification(userId);
            var friends = await _repo.ListAsync(spec);

            return Ok(_mapper.Map<IReadOnlyList<Friend>, IReadOnlyList<FriendToReturnDto>>(friends));
        }
    }
}