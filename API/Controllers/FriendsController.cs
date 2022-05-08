using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using API.Errors;
using System.Text.Json;

namespace API.Controllers
{
    [Authorize]
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

        [HttpGet()]
        public async Task<ActionResult<IReadOnlyList<FriendToReturnDto>>> GetFriends([FromQuery] BaseSpecParams friendsSpecParams)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            try {
                int userId = Int32.Parse(identity.FindFirst("UserId").Value);

                var spec = new FriendsWithUsersSpecification(userId, friendsSpecParams);
                var friends = await _repo.ListAsync(spec);

                return Ok(_mapper.Map<IReadOnlyList<Friend>, IReadOnlyList<FriendToReturnDto>>(friends));
            } catch (Exception ex) {
                return new UnauthorizedObjectResult(new ApiResponse(401, "Faulty token"));
            }
        }

        [HttpPost("{friendUserId}")]
        public async Task<ActionResult<int>> CreateFriend(int friendUserId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            try {
                int userId = Int32.Parse(identity.FindFirst("UserId").Value);

                if (userId == friendUserId) {
                    return new BadRequestObjectResult(new ApiResponse(400, "You cannot add yourself as a friend"));
                }

                // what if they are already friends?
                var spec = new FindFriendSpecification(userId, friendUserId);
                var friend = await _repo.ListAsync(spec);

                if (friend.Count > 0) {
                    _logger.LogInformation(JsonSerializer.Serialize(friend));
                    return new BadRequestObjectResult(new ApiResponse(400, "You are already friends with this user"));
                }

                // otherwise add friend
                await _repo.CreateEntity(new Friend {
                    UserId = userId,
                    FriendUserId = friendUserId
                });
                await _repo.CreateEntity(new Friend
                {
                    UserId = friendUserId,
                    FriendUserId = userId
                });
                return Ok();
               } catch (Exception ex) {
                return new UnauthorizedObjectResult(new ApiResponse(401, "Faulty token"));
            }
        }
    }
}