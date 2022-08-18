using Data;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Controllers;

[ApiController]
[Route("api/groups/{groupId:int}/pendingusers")]
public class GroupPendingUsersController : ControllerBase
{
    private readonly MotoGuildDbContext _db;

    public GroupPendingUsersController(MotoGuildDbContext dbContext)
    {
        _db = dbContext;
    }

    [HttpGet]
    public IActionResult GetGroupPendingUsers(int groupId)
    {
        var group = _db.Groups
            .Include(g => g.PendingUsers)
            .FirstOrDefault(g => g.Id == groupId);
        if (group == null) return NotFound();

        var pendingUsers = group.PendingUsers.ToList();

        if (pendingUsers == null) return NotFound();
        var pendingUsersDto = GetGroupPendingUsersDtos(pendingUsers);
        return Ok(pendingUsersDto);
    }

    [HttpGet("{id:int}", Name = "GetGroupPendingUser")]
    public IActionResult GetGroupPendingUser(int groupId, int id)
    {
        var group = _db.Groups
            .Include(g => g.PendingUsers)
            .FirstOrDefault(g => g.Id == groupId);
        if (group == null) return NotFound();

        var participant = group.PendingUsers.FirstOrDefault(p => p.Id == id);

        if (participant == null) return NotFound();

        var participantDto = GetGroupPendingUserDto(participant);
        return Ok(participantDto);
    }

    [HttpPost("{id:int}")]
    public IActionResult AddGroupPendingUserByUserId(int groupId, int id)
    {
        var group = _db.Groups
            .Include(g => g.PendingUsers)
            .FirstOrDefault(g => g.Id == groupId);
        if (group == null) return NotFound();

        var pendingUser = _db.Users.FirstOrDefault(p => p.Id == id);

        if (pendingUser == null) return NotFound();

        SaveGroupPendingUserToDataBase(group, pendingUser);
        var pendingUserDto = GetGroupPendingUserDto(pendingUser);
        return Ok(pendingUserDto);
    }

    private void SaveGroupPendingUserToDataBase(Group group, User pendingUser)
    {
        group.PendingUsers.Add(pendingUser);
        _db.SaveChanges();
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteGroupPendingUserByUserId(int groupId, int id)
    {
        var group = _db.Groups
            .Include(g => g.PendingUsers)
            .FirstOrDefault(g => g.Id == groupId);
        if (group == null) return NotFound();

        var pendingUser = _db.Users.FirstOrDefault(p => p.Id == id);

        if (pendingUser == null) return NotFound();

        DeleteGroupPendingUserFromDataBase(group, pendingUser);
        var pendingUserDto = GetGroupPendingUserDto(pendingUser);
        return Ok(pendingUserDto);
    }

    private void DeleteGroupPendingUserFromDataBase(Group group, User pendingUser)
    {
        group.PendingUsers.Remove(pendingUser);
        _db.SaveChanges();
    }

    private List<UserSelectedDataDto> GetGroupPendingUsersDtos(List<User> pendingUsers)
    {
        var pendingUsersDtos = new List<UserSelectedDataDto>();
        foreach (var pendingUser in pendingUsers)
            pendingUsersDtos.Add(new UserSelectedDataDto
            {
                Email = pendingUser.Email, Rating = pendingUser.Rating, Id = pendingUser.Id,
                UserName = pendingUser.UserName
            });
        return pendingUsersDtos;
    }

    private UserSelectedDataDto GetGroupPendingUserDto(User pendingUser)
    {
        var pendingUserDto = new UserSelectedDataDto
        {
            Email = pendingUser.Email,
            Id = pendingUser.Id,
            Rating = pendingUser.Rating,
            UserName = pendingUser.UserName
        };
        return pendingUserDto;
    }
}