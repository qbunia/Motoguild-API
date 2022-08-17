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

            var group = _db.Groups
                .Include(g => g.Participants)
                .FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                return NotFound();
            }

            var participants = group.Participants.ToList();

            if (participants == null)
            {
                return NotFound();
            }
            var participantsDto = GetParticipantsDtos(participants);
            return Ok(participantsDto);
        }

        [HttpGet("{id:int}", Name = "GetParticipant")]
        public IActionResult GetParticipant(int groupId, int id)
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

        [HttpPost("{id:int}")]
        public IActionResult AddParticipantByUserId(int groupId, int id)
        {
            
            var group = _db.Groups
                .Include(g => g.Participants)
                .FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                return NotFound();
            }

            var participant = _db.Users.FirstOrDefault(p => p.Id == id);

            if (participant == null)
            {
                return NotFound();
            }

            AddParticipantToGroup(group, participant);
            var participantDto = GetParticipantDto(participant);
            return Ok(participantDto);
        }

        private void AddParticipantToGroup(Group group, User participant)
        {
            group.Participants.Add(participant);
            _db.SaveChanges();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteParticipantByUserId(int groupId, int id)
        {

            var group = _db.Groups
                .Include(g => g.Participants)
                .FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                return NotFound();
            }

            var participant = _db.Users.FirstOrDefault(p => p.Id == id);

            if (participant == null)
            {
                return NotFound();
            }

            DeleteParticipantFromGroup(group, participant);
            var participantDto = GetParticipantDto(participant);
            return Ok(participantDto);
        }

        private void DeleteParticipantFromGroup(Group group, User participant)
        {
            group.Participants.Remove(participant);
            _db.SaveChanges();
        }

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
