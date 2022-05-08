using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        private readonly IGenericRepository<Message> _repo;
        private readonly IAuthenticationHelpers _authHelpers;
        private readonly IMapper _mapper;
        public MessagesController(
            IGenericRepository<Message> repo, 
            IAuthenticationHelpers authHelpers, 
            IMapper mapper
        ) {
            _mapper = mapper;
            _repo = repo;
            _authHelpers = authHelpers;
        }

        [HttpGet("{friendId}")]
        public async Task<ActionResult<IReadOnlyList<MessageToReturnDto>>> GetMessagesBetweenFriends(int friendId, [FromQuery]BaseSpecParams messagesSpecParams)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            try {
                int userId = Int32.Parse(identity.FindFirst("UserId").Value);

                var spec = new MessagesBetweenFriendsSpecification(userId, friendId, messagesSpecParams);
                var messages = await _repo.ListAsync(spec);

                return Ok(_mapper.Map<IReadOnlyList<Message>, IReadOnlyList<MessageToReturnDto>>(messages));
            } catch (Exception ex) {
                return new UnauthorizedObjectResult(new ApiResponse(401, "Faulty token"));
            }
        }
    }
}