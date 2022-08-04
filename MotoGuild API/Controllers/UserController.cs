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
            var users = DataManager.Current.Users;
            var usersSelectedData = SelectBasicInformationOfUsers(users);
            return Ok(usersSelectedData);
        }

        [HttpGet("{id:int}", Name = "GetUser")]
        public IActionResult GetUser(int id, [FromQuery] bool selectedData = false)
        {
            if (selectedData)
            {
                return GetUserWithSelectedData(id);
            }
            return GetUserData(id);
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
            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteUser(int id)
        {
            var user = DataManager.Current.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
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
            if (userDto == null)
            {
                return NotFound();
            }
            UpdateUserData(userDto, updateUserDto);
            return NoContent();
        }

        private void UpdateUserData(UserDto userDto, UpdateUserDto updateUserDto)
        {
            userDto.UserName = updateUserDto.UserName;
            userDto.Email = updateUserDto.Email;
            userDto.PhoneNumber = updateUserDto.PhoneNumber;
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

        private IActionResult GetUserWithSelectedData(int id)
        {
            var user = DataManager.Current.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            var userSelectedData = new UserSelectedDataDto()
                { Email = user.Email, Rating = user.Rating, Id = user.Id, UserName = user.UserName };
            return Ok(userSelectedData);
        }

        private IActionResult GetUserData(int id)
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

        private List<UserSelectedDataDto> SelectBasicInformationOfUsers(List<UserDto> users)
        {
            var usersSelectedData = new List<UserSelectedDataDto>();
            foreach (var user in users)
            {
                usersSelectedData.Add(new UserSelectedDataDto() { Email = user.Email, Rating = user.Rating, Id = user.Id, UserName = user.UserName });
            }
            return usersSelectedData;
        }
    }
}
