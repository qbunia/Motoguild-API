using Microsoft.AspNetCore.Mvc;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = DataManager.Current.UsersWithSelectedData;
            return Ok(users);
        }

        [HttpGet("{id:int}", Name = "GetUser")]
        public IActionResult GetUser(int id, [FromQuery] bool selectedData = false)
        {
            if (selectedData)
            {
                return GetUserWithSelectedData(id);
            }
            return GetAllUserData(id);
        }

        private IActionResult GetUserWithSelectedData(int id)
        {
            var user = DataManager.Current.UsersWithSelectedData.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        private IActionResult GetAllUserData(int id)
        {
            var user = DataManager.Current.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int newId = DataManager.Current.Users.Max(u => u.Id) + 1;
            var user = SaveUserToDataManager(createUserDto, newId);
            SaveUserSelectedDataToDataManager(createUserDto, newId);
            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }


        private UserDto SaveUserToDataManager(CreateUserDto createUserDto, int id)
        {
            UserDto user = new UserDto()
            {
                Id = id,
                UserName = createUserDto.UserName,
                Email = createUserDto.Email,
                PhoneNumber = createUserDto.PhoneNumber,
                Rating = 0
            };
            DataManager.Current.Users.Add(user);
            return user;
        }

        private void SaveUserSelectedDataToDataManager(CreateUserDto createUserDto, int id)
        {
            UserSelectedDataDto userSelectedData = new UserSelectedDataDto()
            {
                Id = id,
                UserName = createUserDto.UserName,
                Email = createUserDto.Email,
                Rating = 0
            };
            DataManager.Current.UsersWithSelectedData.Add(userSelectedData);
        }
    }
}
