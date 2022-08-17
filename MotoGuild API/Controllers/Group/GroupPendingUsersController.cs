using System.Runtime.ExceptionServices;
using Data;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Models.Group;
using MotoGuild_API.Models.Post;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Controllers
{
    [ApiController]
    [Route("api/groups/{groupId:int}/pendingusers")]
    public class GroupPendingUsersController : ControllerBase
    {
        private MotoGuildDbContext _db;

        public GroupPendingUsersController(MotoGuildDbContext dbContext)
        {
            _db = dbContext;
        }

        [HttpGet]
        public IActionResult GetPendingUsers(int groupId)
        {

            var group = _db.Groups
                .Include(g => g.PendingUsers)
                .FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                return NotFound();
            }

            var pendingUsers = group.PendingUsers.ToList();

            if (pendingUsers == null)
            {
                return NotFound();
            }
            var pendingUsersDto = GetPendingUsersDtos(pendingUsers);
            return Ok(pendingUsersDto);
        }

        [HttpGet("{id:int}", Name = "GetPendingUser")]
        public IActionResult GetPendingUser(int groupId, int id)
        {
            var group = _db.Groups
                .Include(g => g.PendingUsers)
                .FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                return NotFound();
            }

            var participant = group.PendingUsers.FirstOrDefault(p => p.Id == id);

            if (participant == null)
            {
                return NotFound();
            }

            var participantDto = GetPendingUserDto(participant);
            return Ok(participantDto);
        }

        [HttpPost("{id:int}")]
        public IActionResult AddPendingUserByUserId(int groupId, int id)
        {
            
            var group = _db.Groups
                .Include(g => g.PendingUsers)
                .FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                return NotFound();
            }

            var pendingUser = _db.Users.FirstOrDefault(p => p.Id == id);

            if (pendingUser == null)
            {
                return NotFound();
            }

            AddPendingUserToGroup(group, pendingUser);
            var pendingUserDto = GetPendingUserDto(pendingUser);
            return Ok(pendingUserDto);
        }

        private void AddPendingUserToGroup(Group group, User pendingUser)
        {
            group.PendingUsers.Add(pendingUser);
            _db.SaveChanges();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeletePendingUserByUserId(int groupId, int id)
        {

            var group = _db.Groups
                .Include(g => g.PendingUsers)
                .FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                return NotFound();
            }

            var pendingUser = _db.Users.FirstOrDefault(p => p.Id == id);

            if (pendingUser == null)
            {
                return NotFound();
            }

            DeletePendingUserFromGroup(group, pendingUser);
            var pendingUserDto = GetPendingUserDto(pendingUser);
            return Ok(pendingUserDto);
        }

        private void DeletePendingUserFromGroup(Group group, User pendingUser)
        {
            group.PendingUsers.Remove(pendingUser);
            _db.SaveChanges();
        }

        private List<UserSelectedDataDto> GetPendingUsersDtos(List<User> pendingUsers)
        {
            var pendingUsersDtos = new List<UserSelectedDataDto>();
            foreach (var pendingUser in pendingUsers)
            {
                pendingUsersDtos.Add(new UserSelectedDataDto() { Email = pendingUser.Email, Rating = pendingUser.Rating, Id = pendingUser.Id, UserName = pendingUser.UserName });
            }
            return pendingUsersDtos;
        }

        private UserSelectedDataDto GetPendingUserDto(User pendingUser)
        {
            var pendingUserDto = new UserSelectedDataDto()
            {
                Email = pendingUser.Email,
                Id = pendingUser.Id,
                Rating = pendingUser.Rating,
                UserName = pendingUser.UserName
            };
            return pendingUserDto;
        }
    }
}
