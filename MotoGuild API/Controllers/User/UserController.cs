
using Data;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private MotoGuildDbContext _db;

        public UserController(MotoGuildDbContext dbContext)
        {
            _db = dbContext;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            List<User> users = _db.Users.ToList();
            var UsersDto = GetUsersDtos(users);
            return Ok(UsersDto);
        }

        [HttpGet("{id:int}", Name = "GetUser")]
        public IActionResult GetUser(int id, [FromQuery] bool selectedData = false)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var UserDto = GetUserDto(user);
            return Ok(UserDto);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = SaveUserToDataBase(createUserDto);
            var userDto = GetUserDto(user);
            return CreatedAtRoute("GetUser", new { id = userDto.Id }, userDto);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            _db.Remove(user);
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = _db.Users.FirstOrDefault(i => i.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            UpdateUserData(user, updateUserDto);
            return NoContent();
        }

        private void UpdateUserData(User user, UpdateUserDto updateUserDto)
        {
            user.UserName = updateUserDto.UserName;
            user.Email = updateUserDto.Email;
            user.PhoneNumber = updateUserDto.PhoneNumber;
            _db.SaveChanges();
        }

        private User SaveUserToDataBase(CreateUserDto createUserDto)
        {
            User user = new User()
            {
                UserName = createUserDto.UserName,
                Email = createUserDto.Email,
                PhoneNumber = createUserDto.PhoneNumber,
                Rating = 0
            };
            _db.Users.Add(user);
            _db.SaveChanges();
            return user;
        }


        private List<UserSelectedDataDto> GetUsersDtos(List<User> users)
        {
            var usersDtos = new List<UserSelectedDataDto>();
            foreach (var user in users)
            {
                usersDtos.Add(new UserSelectedDataDto() { Email = user.Email, Rating = user.Rating, Id = user.Id, UserName = user.UserName });
            }
            return usersDtos;
        }

        private UserSelectedDataDto GetUserDto(User user)
        {
            var userDto = new UserSelectedDataDto()
                { Email = user.Email, Rating = user.Rating, Id = user.Id, UserName = user.UserName };
            return userDto;
        }
    }
}
