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

        [HttpPost]
        public IActionResult CreateUser([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int newId = GetNewId();
            var user = SaveUserToDataManager(createUserDto, newId);
            SaveUserSelectedDataToDataManager(createUserDto, newId);
            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteUser(int id)
        {
            var user = DataManager.Current.Users.FirstOrDefault(u => u.Id == id);
            var userSelectedData = DataManager.Current.UsersWithSelectedData.FirstOrDefault(u => u.Id == id);
            if (user == null || userSelectedData == null)
            {
                return NotFound();
            }
            DataManager.Current.UsersWithSelectedData.Remove(userSelectedData);
            DataManager.Current.Users.Remove(user);
            return Ok();
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userDto = DataManager.Current.Users.FirstOrDefault(i => i.Id == id);
            var userSelectDataDto = DataManager.Current.UsersWithSelectedData.FirstOrDefault(i => i.Id == id);
            if (userDto == null || userSelectDataDto == null)
            {
                return NotFound();
            }
            UpdateAllUserData(userDto, updateUserDto);
            UpdateUserSelectData(userSelectDataDto, updateUserDto);
            return NoContent();
        }

        private void UpdateAllUserData(UserDto userDto, UpdateUserDto updateUserDto)
        {
            userDto.UserName = updateUserDto.UserName;
            userDto.Email = updateUserDto.Email;
            userDto.PhoneNumber = updateUserDto.PhoneNumber;
        }

        private void UpdateUserSelectData(UserSelectedDataDto userSelectDataDto, UpdateUserDto updateUserDto)
        {
            userSelectDataDto.UserName = updateUserDto.UserName;
            userSelectDataDto.Email = updateUserDto.Email;
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

        private int GetNewId()
        {
            if (DataManager.Current.Users.Count == 0)
            {
                return 1;
            }
            return DataManager.Current.Users.Max(u => u.Id) + 1;
        }
    }
}
