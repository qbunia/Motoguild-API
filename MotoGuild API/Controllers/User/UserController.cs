using AutoMapper;
using Data;
using Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MotoGuild_API.Models.User;
using MotoGuild_API.Repository.Interface;

namespace MotoGuild_API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [EnableCors("AllowAnyOrigin")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetAll();
            return Ok(_mapper.Map<List<UserDto>>(users));
        }

        [HttpGet("{id:int}", Name = "GetUser")]
        public IActionResult GetUser(int id, [FromQuery] bool selectedData = false)
        {
            var user = _userRepository.Get(id);
            if (user == null) return NotFound();
            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] CreateUserDto createUserDto)
        {
            var user = _mapper.Map<User>(createUserDto);
            _userRepository.Insert(user);
            _userRepository.Save();
            var userDto = _mapper.Map<UserDto>(user);
            return CreatedAtRoute("GetUser", new { id = userDto.Id }, userDto);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _userRepository.Get(id);
            if (user == null) return NotFound();
            _userRepository.Delete(id);
            _userRepository.Save();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            var user = _userRepository.Get(id);
            if (user == null) return NotFound();
            _mapper.Map(updateUserDto, user);
            _userRepository.Update(user);
            _userRepository.Save();
            return NoContent();
        }

    }
}
