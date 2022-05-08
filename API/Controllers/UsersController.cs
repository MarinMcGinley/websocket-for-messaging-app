using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IGenericRepository<User> _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;
        private readonly IAuthenticationHelpers _authHelpers;

        public UsersController(
            IGenericRepository<User> repo, 
            IMapper mapper, 
            ILogger<UsersController> logger,
            IAuthenticationHelpers authHelpers
        ) {
            _logger = logger;
            _authHelpers = authHelpers;
            _mapper = mapper;
            _repo = repo;
        }


        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<UserToReturnDto>>> GetUsers([FromQuery]BaseSpecParams userSpecParams)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            try {
                int userId = Int32.Parse(identity.FindFirst("UserId").Value);

                var spec = new UsersPaginatedSpecification(userSpecParams, userId);
                var users = await _repo.ListAsync(spec);

                return Ok(_mapper.Map<IReadOnlyList<User>, IReadOnlyList<UserToReturnDto>>(users));
            } catch (Exception ex) {
                return new UnauthorizedObjectResult(new ApiResponse(401, "Faulty token"));
            }
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<UserToReturnDto>> GetUser(int id)
        {
            var user = await _repo.GetByIdAsync(id);

            if (user == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<User, UserToReturnDto>(user));
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserToReturnDto>> GetUserFromEmail(string email)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            try {
                int userId = Int32.Parse(identity.FindFirst("UserId").Value);

                var spec = new UsersFromEmailSpecification(email, userId);
                var user = await _repo.GetEntityWithSpec(spec);

                if (user == null) return NotFound(new ApiResponse(404));
                return Ok(_mapper.Map<User, UserToReturnDto>(user));
            } catch (Exception ex) {
                return new UnauthorizedObjectResult(new ApiResponse(401, "Faulty token"));
            }
        }

        [HttpGet("find")]
        public async Task<ActionResult<IReadOnlyList<UserToReturnDto>>> FindUsers([FromQuery]UserSpecParams userSpecParams)
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;

            try {
                int userId = Int32.Parse(identity.FindFirst("UserId").Value);

                var spec = new UsersFromSearchStringSpecification(userSpecParams, userId);
                var users = await _repo.ListAsync(spec);

                return Ok(_mapper.Map<IReadOnlyList<User>, IReadOnlyList<UserToReturnDto>>(users));
            } catch (Exception ex) {
                return new UnauthorizedObjectResult(new ApiResponse(401, "Faulty token"));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<int>> CreateUser(User user)
        {
            try {
                string hashedPassword = _authHelpers.GenerateHash(user.Password);

                await _repo.CreateEntity(new User(
                    Email: user.Email,
                    Password: hashedPassword,
                    FirstName: user.FirstName,
                    LastName: user.LastName
                ));
                return Ok();
            } catch (Exception ex) {
                _logger.LogError(ex, "Unable to create user");
                return new BadRequestObjectResult(new ApiResponse(400, "A user with this email already exists"));
            }
            
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] UserToPatch user) {
            try {
                _logger.LogInformation(JsonSerializer.Serialize(user));
                var userInDB = await _repo.GetByIdAsync(id);

                if (userInDB == null) {
                    return NotFound(new ApiResponse(404));
                }

                if (!_authHelpers.DoesUserHaveAccess(HttpContext.User.Identity as ClaimsIdentity, id)) {
                    return new UnauthorizedObjectResult(new ApiResponse(401, "You cannot update other users"));
                }
                
                if (user.FirstName != null) userInDB.FirstName = user.FirstName;
                if (user.LastName != null) userInDB.LastName = user.LastName;

                var results = await _repo.UpdateEntity(userInDB);

                var updatedUser = await _repo.GetByIdAsync(id);
                return Ok(updatedUser);
            } catch (Exception ex) {
                _logger.LogError(ex, "Unable to update user");
                return new BadRequestObjectResult(new ApiResponse(400, "Unable to update user"));
            }
        }
    }
}