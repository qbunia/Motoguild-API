using Data;
using Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Models.Group;
using MotoGuild_API.Models.Post;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Controllers;

[ApiController]
[Route("api/groups")]
[EnableCors("AllowAnyOrigin")]
public class GroupController : ControllerBase
{
    private readonly MotoGuildDbContext _db;

    public GroupController(MotoGuildDbContext dbContext)
    {
        _db = dbContext;
    }

    [HttpGet]
    public IActionResult GetGroups()
    {
        var groups = _db.Groups
            .Include(g => g.Owner)
            .Include(g => g.Participants)
            .Include(g => g.PendingUsers)
            .Include(g => g.Posts).ThenInclude(p => p.Author)
            .ToList();
        var groupsDto = GetGroupsDtos(groups);
        return Ok(groupsDto);
    }

    [HttpGet("{id:int}", Name = "GetGroup")]
    public IActionResult GetGroup(int id, [FromQuery] bool selectedData = false)
    {
        var group = _db.Groups
            .Include(g => g.Owner)
            .Include(g => g.Participants)
            .Include(g => g.PendingUsers)
            .Include(g => g.Posts).ThenInclude(p => p.Author)
            .FirstOrDefault(g => g.Id == id);
        if (group == null) return NotFound();

        var groupDto = GetGroupDto(group);
        return Ok(groupDto);
    }

    [HttpPost]
    public IActionResult CreateGroup([FromBody] CreateGroupDto createGroupDto)
    {
        if (!UserExists(createGroupDto.OwnerId)) ModelState.AddModelError("Description", "User not found");

        if (!ModelState.IsValid) return BadRequest(ModelState);

        var group = SaveGroupToDataBase(createGroupDto);
        var groupFull = _db.Groups
            .Include(g => g.PendingUsers)
            .Include(g => g.Posts)
            .Include(g => g.Participants)
            .FirstOrDefault(g => g.Id == group.Id);
        var groupDto = GetGroupDto(groupFull);
        return CreatedAtRoute("GetGroup", new {id = groupDto.Id}, groupDto);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteGroup(int id)
    {
        var group = _db.Groups.FirstOrDefault(u => u.Id == id);
        if (group == null) return NotFound();

        _db.Remove(group);
        _db.SaveChanges();
        return Ok();
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateGroup(int id, [FromBody] UpdateGroupDto updateGroupDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var group = _db.Groups.FirstOrDefault(i => i.Id == id);
        if (group == null) return NotFound();
        UpdateGroupData(group, updateGroupDto);
        return NoContent();
    }

    private void UpdateGroupData(Group group, UpdateGroupDto updateGroupDto)
    {
        group.Name = updateGroupDto.Name;
        group.IsPrivate = updateGroupDto.IsPrivate;
        _db.SaveChanges();
    }


    private bool UserExists(int id)
    {
        return _db.Users.FirstOrDefault(u => u.Id == id) != null;
    }

    private Group SaveGroupToDataBase(CreateGroupDto createGroupDto)
    {
        var owner = _db.Users.FirstOrDefault(u => u.Id == createGroupDto.OwnerId);
        var group = new Group
        {
            Name = createGroupDto.Name,
            CreationDate = DateTime.Now,
            IsPrivate = createGroupDto.IsPrivate,
            Owner = owner,
            Participants = new List<User>()
        };
        group.Participants.Add(owner);
        _db.Groups.Add(group);
        _db.SaveChanges();
        return group;
    }


    private List<SelectedGroupDto> GetGroupsDtos(List<Group> groups)
    {
        var groupsDtos = new List<SelectedGroupDto>();
        foreach (var group in groups)
        {
            var userDto = new UserSelectedDataDto
            {
                Email = group.Owner.Email,
                Id = group.Owner.Id,
                Rating = group.Owner.Rating,
                UserName = group.Owner.UserName
            };
            var participantsDto = new List<UserSelectedDataDto>();
            foreach (var participant in group.Participants)
                participantsDto.Add(new UserSelectedDataDto
                {
                    Email = participant.Email,
                    Id = participant.Id,
                    Rating = participant.Rating,
                    UserName = participant.UserName
                });
            var pendingUserDto = new List<UserSelectedDataDto>();
            foreach (var pendingUser in group.PendingUsers)
                pendingUserDto.Add(new UserSelectedDataDto
                {
                    Email = pendingUser.Email,
                    Id = pendingUser.Id,
                    Rating = pendingUser.Rating,
                    UserName = pendingUser.UserName
                });

            var postsDto = new List<PostDto>();
            foreach (var post in group.Posts)
            {
                var authorDto = new UserSelectedDataDto
                {
                    Email = post.Author.Email,
                    Id = post.Author.Id,
                    Rating = post.Author.Rating,
                    UserName = post.Author.UserName
                };
                postsDto.Add(new PostDto
                {
                    Id = post.Id,
                    Author = authorDto,
                    Content = post.Content,
                    CreateTime = post.CreateTime
                });
            }

            groupsDtos.Add(new SelectedGroupDto
            {
                Id = group.Id,
                IsPrivate = group.IsPrivate,
                Name = group.Name, Owner = userDto,
                Participants = participantsDto,
                PendingUsers = pendingUserDto,
                Posts = postsDto
            });
        }

        return groupsDtos;
    }

    private GroupDto GetGroupDto(Group group)
    {
        var userDto = new UserSelectedDataDto
        {
            Email = group.Owner.Email,
            Id = group.Owner.Id,
            Rating = group.Owner.Rating,
            UserName = group.Owner.UserName
        };
        var participantsDto = new List<UserSelectedDataDto>();
        foreach (var participant in group.Participants)
            participantsDto.Add(new UserSelectedDataDto
            {
                Email = participant.Email,
                Id = participant.Id,
                Rating = participant.Rating,
                UserName = participant.UserName
            });

        var pendingUserDto = new List<UserSelectedDataDto>();
        foreach (var pendingUser in group.PendingUsers)
            pendingUserDto.Add(new UserSelectedDataDto
            {
                Email = pendingUser.Email,
                Id = pendingUser.Id,
                Rating = pendingUser.Rating,
                UserName = pendingUser.UserName
            });

        var postsDto = new List<PostDto>();
        foreach (var post in group.Posts)
        {
            var authorDto = new UserSelectedDataDto
            {
                Email = post.Author.Email,
                Id = post.Author.Id,
                Rating = post.Author.Rating,
                UserName = post.Author.UserName
            };

            postsDto.Add(new PostDto
            {
                Author = authorDto,
                Content = post.Content,
                CreateTime = post.CreateTime
            });
        }

        var groupDto = new GroupDto
        {
            Id = group.Id,
            IsPrivate = group.IsPrivate,
            Name = group.Name,
            Owner = userDto,
            CreationDate = group.CreationDate,
            Participants = participantsDto,
            PendingUsers = pendingUserDto,
            Posts = postsDto
        };
        return groupDto;
    }
}