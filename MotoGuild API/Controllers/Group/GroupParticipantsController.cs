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
    [Route("api/groups/{groupId:int}/participants")]
    public class GroupParticipantsController : ControllerBase
    {
        private MotoGuildDbContext _db;

        public GroupParticipantsController(MotoGuildDbContext dbContext)
        {
            _db = dbContext;
        }

        [HttpGet]
        public IActionResult GetParticipants(int groupId)
        {
            var participants = _db.Groups.Include(g => g.Participants).Where(g => g.Id == groupId).Select(g => g.Participants).First().ToList();
            var participantsDto = GetParticipantsDtos(participants);
            return Ok(participantsDto);
        }

        [HttpGet("{id:int}", Name = "GetParticipant")]
        public IActionResult GetParticipant(int groupId, int id, [FromQuery] bool selectedData = false)
        {
            var group = _db.Groups
                .Include(g => g.Participants)
                .FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                return NotFound();
            }

            var participant = group.Participants.FirstOrDefault(p => p.Id == id);

            if (participant == null)
            {
                return NotFound();
            }

            var participantDto = GetParticipantDto(participant);
            return Ok(participantDto);
        }
        //[HttpPost]
        //public IActionResult CreateGroup([FromBody] CreateGroupDto createGroupDto)
        //{
        //    if (!UserExists(createGroupDto.OwnerId))
        //    {
        //        ModelState.AddModelError(key: "Description", errorMessage: "User not found");
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var group = SaveGroupToDataBase(createGroupDto);
        //    var groupDto = GetGroupDto(group);
        //    return CreatedAtRoute("GetGroup", new { id = groupDto.Id }, groupDto);
        //}

        //[HttpDelete("{id:int}")]
        //public IActionResult DeleteGroup(int id)
        //{
        //    var group = _db.Groups.FirstOrDefault(u => u.Id == id);
        //    if (group == null)
        //    {
        //        return NotFound();
        //    }

        //    _db.Remove(group);
        //    _db.SaveChanges();
        //    return Ok();
        //}

        //[HttpPut("{id:int}")]
        //public IActionResult UpdateGroup(int id, [FromBody] UpdateGroupDto updateGroupDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var group = _db.Groups.FirstOrDefault(i => i.Id == id);
        //    if (group == null)
        //    {
        //        return NotFound();
        //    }
        //    UpdateGroupData(group, updateGroupDto);
        //    return NoContent();
        //}

        //private void UpdateGroupData(Group group, UpdateGroupDto updateGroupDto)
        //{
        //    group.Name = updateGroupDto.Name;
        //    group.IsPrivate = updateGroupDto.IsPrivate;
        //    _db.SaveChanges();
        //}

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

        //private bool UserExists(int id)
        //{
        //    return _db.Users.FirstOrDefault(u => u.Id == id) != null;
        //}

        //private Group SaveGroupToDataBase(CreateGroupDto createGroupDto)
        //{
        //    var owner = _db.Users.FirstOrDefault(u => u.Id == createGroupDto.OwnerId);
        //    var group = new Group()
        //    {
        //        Name = createGroupDto.Name,
        //        CreationDate = DateTime.Now,
        //        IsPrivate = createGroupDto.IsPrivate,
        //        Owner = owner,
        //        Participants = new List<User>()
        //    };
        //    group.Participants.Add(owner);
        //    _db.Groups.Add(group);
        //    _db.SaveChanges();
        //    return group;
        //}



        private List<UserSelectedDataDto> GetParticipantsDtos(List<User> participants)
        {
            var participantsDtos = new List<UserSelectedDataDto>();
            foreach (var participant in participants)
            {
                participantsDtos.Add(new UserSelectedDataDto() { Email = participant.Email, Rating = participant.Rating, Id = participant.Id, UserName = participant.UserName });
            }
            return participantsDtos;
        }

        private UserSelectedDataDto GetParticipantDto(User participant)
        {
            var participantDto = new UserSelectedDataDto()
            {
                Email = participant.Email,
                Id = participant.Id,
                Rating = participant.Rating,
                UserName = participant.UserName
            };
            return participantDto;
        }
    }
}
