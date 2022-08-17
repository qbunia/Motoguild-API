using Data;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Models.Comment;
using MotoGuild_API.Models.Group;
using MotoGuild_API.Models.Post;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Controllers
{
    [ApiController]
    [Route("api/groups")]
    public class GroupController : ControllerBase
    {
        private MotoGuildDbContext _db;

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
                .Include(g => g.Posts)
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
                .Include(g => g.Posts)
                .FirstOrDefault(g => g.Id == id);
            if (group == null)
            {
                return NotFound();
            }

            var groupDto = GetGroupDto(group);
            return Ok(groupDto);
        }

        [HttpPost]
        public IActionResult CreateGroup([FromBody] CreateGroupDto createGroupDto)
        {
            if (!UserExists(createGroupDto.OwnerId))
            {
                ModelState.AddModelError(key: "Description", errorMessage: "User not found");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var group = SaveGroupToDataBase(createGroupDto);
            var groupDto = GetGroupDto(group);
            return CreatedAtRoute("GetGroup", new { id = groupDto.Id }, groupDto);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteGroup(int id)
        {
            var group = _db.Groups.FirstOrDefault(u => u.Id == id);
            if (group == null)
            {
                return NotFound();
            }

            _db.Remove(group);
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateGroup(int id, [FromBody] UpdateGroupDto updateGroupDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var group = _db.Groups.FirstOrDefault(i => i.Id == id);
            if (group == null)
            {
                return NotFound();
            }
            UpdateGroupData(group, updateGroupDto);
            return NoContent();
        }

        private void UpdateGroupData(Group group, UpdateGroupDto updateGroupDto)
        {
            group.Name = updateGroupDto.Name;
            group.IsPrivate = updateGroupDto.IsPrivate;
            _db.SaveChanges();
        }

        //[HttpPut("{id:int}")]
        //public IActionResult AddMember([FromBody] GroupAddMemberDto addMemberDto, int id)
        //{
        //    var group = DataManager.Current.Groups.FirstOrDefault(g => g.Id == id);
        //    var member = DataManager.Current.Users.FirstOrDefault(u => u.Id == addMemberDto.MemberId);
        //    if (group == null || member == null)
        //    {
        //        return NotFound();
        //    }
        //    if (group.Members.Any(u => u.Id == addMemberDto.MemberId) || group.Owner.Id == addMemberDto.MemberId)
        //    {
        //        ModelState.AddModelError(key: "Description", errorMessage: "User is already in group");
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }


        //    var memberSelectedData = new UserSelectedDataDto()
        //        { Email = member.Email, Id = member.Id, Rating = member.Rating, UserName = member.UserName };
        //    if (group.IsPrivate)
        //    {
        //        group.PendingMembers.Add(memberSelectedData);
        //        return NoContent();
        //    }
        //    group.Members.Add(memberSelectedData);
        //    //member.Groups.Add(group);
        //    return NoContent();

        //}

        //[HttpPut("accept/{id:int}")]
        //public IActionResult AcceptMember([FromBody] GroupAddMemberDto addMemberDto, int id)
        //{
        //    var group = DataManager.Current.Groups.FirstOrDefault(g => g.Id == id);
        //    var member = DataManager.Current.Users.FirstOrDefault(u => u.Id == addMemberDto.MemberId);
        //    if (group == null || member == null)
        //    {
        //        return NotFound();
        //    }
        //    var memberSelectedData = new UserSelectedDataDto()
        //    { Email = member.Email, Id = member.Id, Rating = member.Rating, UserName = member.UserName };
        //    if (group.PendingMembers.FirstOrDefault(m => m.Id == memberSelectedData.Id) == null)
        //    {
        //        ModelState.AddModelError(key: "Description", errorMessage: "User not found");
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    group.PendingMembers.Remove(memberSelectedData);
        //    //member.Groups.Add(group);
        //    group.Members.Add(memberSelectedData);
        //    return NoContent();
        //}

        private bool UserExists(int id)
        {
            return _db.Users.FirstOrDefault(u => u.Id == id) != null;
        }

        private Group SaveGroupToDataBase(CreateGroupDto createGroupDto)
        {
            var owner = _db.Users.FirstOrDefault(u => u.Id == createGroupDto.OwnerId);
            var group = new Group()
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

        

        private List<GroupDto> GetGroupsDtos(List<Group> groups)
        {
            var groupsDtos = new List<GroupDto>();
            foreach (var group in groups)
            {
                var userDto = new UserSelectedDataDto()
                {
                    Email = group.Owner.Email,
                    Id = group.Owner.Id,
                    Rating = group.Owner.Rating,
                    UserName = group.Owner.UserName
                };
                var participantsDto = new List<UserSelectedDataDto>();
                foreach (var participant in group.Participants)
                {
                    participantsDto.Add(new UserSelectedDataDto()
                    {
                        Email = participant.Email,
                        Id = participant.Id,
                        Rating = participant.Rating,
                        UserName = participant.UserName
                    });
                }
                var pendingUserDto = new List<UserSelectedDataDto>();
                foreach (var pendingUser in group.PendingUsers)
                {
                    pendingUserDto.Add(new UserSelectedDataDto()
                    {
                        Email = pendingUser.Email,
                        Id = pendingUser.Id,
                        Rating = pendingUser.Rating,
                        UserName = pendingUser.UserName
                    });
                }
                groupsDtos.Add(new GroupDto() 
                    { 
                        Id = group.Id, 
                        IsPrivate = group.IsPrivate, 
                        Name = group.Name, Owner = userDto, 
                        Participants = participantsDto,
                        PendingUsers = pendingUserDto

                });
            }
            return groupsDtos;
        }

        private GroupDto GetGroupDto(Group group)
        {
            var userDto = new UserSelectedDataDto()
            {
                Email = group.Owner.Email,
                Id = group.Owner.Id,
                Rating = group.Owner.Rating,
                UserName = group.Owner.UserName
            };
            var participantsDto = new List<UserSelectedDataDto>();
            foreach (var participant in group.Participants)
            {
                participantsDto.Add(new UserSelectedDataDto()
                {
                    Email = participant.Email,
                    Id = participant.Id,
                    Rating = participant.Rating,
                    UserName = participant.UserName
                });
            }

            var pendingUserDto = new List<UserSelectedDataDto>();
            foreach (var pendingUser in group.PendingUsers)
            {
                pendingUserDto.Add(new UserSelectedDataDto()
                {
                    Email = pendingUser.Email,
                    Id = pendingUser.Id,
                    Rating = pendingUser.Rating,
                    UserName = pendingUser.UserName
                });
            }

            var postsDto = new List<PostDto>();
            foreach (var post in group.Posts)
            {
                var authorDto = new UserSelectedDataDto()
                {
                    Email = post.Author.Email,
                    Id = post.Author.Id,
                    Rating = post.Author.Rating,
                    UserName = post.Author.UserName
                };
                var commentsDto = new List<CommentDto>();
                foreach (var comment in post.Comments)
                {
                    var commentauthorDto = new UserSelectedDataDto()
                    {
                        Email = comment.Author.Email,
                        Id = comment.Author.Id,
                        Rating = comment.Author.Rating,
                        UserName = comment.Author.UserName
                    };
                    commentsDto.Add(new CommentDto()
                    {
                        Author = commentauthorDto,
                        Content = comment.Content,
                        CreateTime = comment.CreateTime,
                        Id = comment.Id
                    });
                }
                postsDto.Add(new PostDto()
                {
                    Author = authorDto,
                    Comments = commentsDto,
                    Content = post.Content,
                    CreateTime = post.CreateTime
                });
            }

            var groupDto = new GroupDto()
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
}
