using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IGenericRepository<User> _repo;
        private readonly IMapper _mapper;
        public UsersController(IGenericRepository<User> repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<UserToReturnDto>>> GetUsers([FromQuery]UserSpecParams userSpecParams)
        {
            var spec = new UsersPaginatedSpecification(userSpecParams);
            var users = await _repo.ListAsync(spec);

            return Ok(_mapper.Map<IReadOnlyList<User>, IReadOnlyList<UserToReturnDto>>(users));
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
            var spec = new UsersFromEmailSpecification(email);
            var user = await _repo.GetEntityWithSpec(spec);

            if (user == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<User, UserToReturnDto>(user));

        }

        [HttpGet("find")]
        public async Task<ActionResult<IReadOnlyList<UserToReturnDto>>> FindUsers([FromQuery]UserSpecParams userSpecParams)
        {
            var spec = new UsersFromSearchStringSpecification(userSpecParams);
            var users = await _repo.ListAsync(spec);

            return Ok(_mapper.Map<IReadOnlyList<User>, IReadOnlyList<UserToReturnDto>>(users));

        }

        [HttpPost]
        public string CreateUser()
        {
            return "User created";
        }

    }
}