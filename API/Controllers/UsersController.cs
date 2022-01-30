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
    public class UsersController : ControllerBase
    {
        private readonly IGenericRepository<User> _repo;
        private readonly IMapper _mapper;
        public UsersController(IGenericRepository<User> repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<UserToReturnDto>>> GetUsers()
        {
            var users = await _repo.ListAllAsync();

            return Ok(_mapper.Map<IReadOnlyList<User>, IReadOnlyList<UserToReturnDto>>(users));
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<UserToReturnDto>> GetUser(int id)
        {
            var user = await _repo.GetByIdAsync(id);

            return Ok(_mapper.Map<User, UserToReturnDto>(user));
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserToReturnDto>> GetUserFromEmail(string email)
        {
            var spec = new UsersFromEmailSpecification(email);
            var user = await _repo.GetEntityWithSpec(spec);

            return Ok(_mapper.Map<User, UserToReturnDto>(user));

        }

        [HttpGet("find/{searchString}")]
        public async Task<ActionResult<IReadOnlyList<UserToReturnDto>>> FindUsers(string searchString)
        {
            var spec = new UsersFromSearchStringSpecification(searchString);
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