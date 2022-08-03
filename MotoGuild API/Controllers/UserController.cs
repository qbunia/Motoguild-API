using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{id:int}")]
        public IActionResult GetUser(int id, [FromQuery] bool selectedData = false)
        {
            if (selectedData)
            {
                return GetUserWithSelectedData(id);
            }
            return GetAllUserData(id);
        }

        public IActionResult GetUserWithSelectedData(int id)
        {
            var user = DataManager.Current.UsersWithSelectedData.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        public IActionResult GetAllUserData(int id)
        {
            var user = DataManager.Current.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        //[HttpPost]
        //public IActionResult CreateUser()
        //{

        //}
    }
}
