using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FriendsController : BaseApiController
    {
        private readonly IGenericRepository<Friend> _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<FriendsController> _logger;
        public FriendsController(IGenericRepository<Friend> friendRepo, IMapper mapper, ILogger<FriendsController> logger)
        {
            _mapper = mapper;
            _repo = friendRepo;
            _logger = logger;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IReadOnlyList<FriendToReturnDto>>> GetFriends(int userId, [FromQuery] BaseSpecParams friendsSpecParams)
        {
            _logger.LogInformation("logging get request for friends try again");
            var spec = new FriendsWithUsersSpecification(userId, friendsSpecParams);
            var friends = await _repo.ListAsync(spec);

            return Ok(_mapper.Map<IReadOnlyList<Friend>, IReadOnlyList<FriendToReturnDto>>(friends));
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateFriend(Friend friend)
        {
            await _repo.CreateEntity(friend);
            await _repo.CreateEntity(new Friend
            {
                UserId = friend.FriendUserId,
                FriendUserId = friend.UserId
            });
            return Ok();
        }
    }
}