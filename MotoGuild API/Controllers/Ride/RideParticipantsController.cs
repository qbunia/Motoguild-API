using System.Runtime.ExceptionServices;
using Data;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Models.User;


namespace MotoGuild_API.Controllers;


[ApiController]
[Route("api/rides/{rideId:int}/participants")]
public class RideParticipantsController : ControllerBase
{
    private MotoGuildDbContext _db;

    public RideParticipantsController(MotoGuildDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult GetRideParticipants(int rideId)
    {
        var participants = _db.Rides.Include(r => r.Participants)
            .Where(r => r.Id == rideId).Select(r => r.Participants).First().ToList();
        var participantsDto = GetRideParticipantsDtos(participants);
        return Ok(participantsDto);
    }

    [HttpGet("{id:int}", Name = "GetRideParticipant")]
    public IActionResult GetRideParticipant(int rideId, int id)
    {
        var ride = _db.Rides.Include(r => r.Participants)
            .FirstOrDefault(r => r.Id == rideId);
        if (ride == null)
        {
            return NotFound();
        }

        var participant = ride.Participants.FirstOrDefault(p => p.Id == id);
        if (participant == null)
        {
            return NotFound();
        }

        var participantDto = GetRideParticipantDto(participant);
        return Ok(participantDto);
    }

    [HttpPost("{id:int}")]
    public IActionResult AddRideParticipantByUserId(int rideId, int id)
    {
        var ride = _db.Rides.Include(r => r.Participants)
            .FirstOrDefault(p => p.Id == rideId);
        if (ride == null)
        {
            return NotFound();
        }

        var participant = _db.Users.FirstOrDefault(p => p.Id == id);
        if (participant == null)
        {
            return NotFound();
        }

        AddRideParticipantToRide(ride, participant);
        var participantDto = GetRideParticipantDto(participant);
        return Ok(participantDto);

    }

    private void AddRideParticipantToRide(Domain.Ride ride, User participant)
    {
        ride.Participants.Add(participant);
        _db.SaveChanges();
    }

    private UserSelectedDataDto GetRideParticipantDto(User participant)
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

    private List<UserSelectedDataDto> GetRideParticipantsDtos(List<User> participants)
    {
        var participantsDtos = new List<UserSelectedDataDto>();
        foreach (var participant in participants)
        {
            participantsDtos.Add(new UserSelectedDataDto()
            {
                Email = participant.Email, Rating = participant.Rating, Id = participant.Id,
                UserName = participant.UserName
            });
        }

        return participantsDtos;
    }
    
    [HttpDelete("{id:int}")]
    public IActionResult DeleteRideParticipantByUserId(int rideId, int id)
    {
        var ride = _db.Rides.Include(r => r.Participants)
            .FirstOrDefault(p => p.Id == rideId);
        if (ride == null)
        {
            return NotFound();
        }
        var participant = _db.Users.FirstOrDefault(p => p.Id == id);
        if (participant == null)
        {
            return NotFound();
        }
        DeleteRideParticipantFromRide(ride, participant);
        return NoContent();
    }
    
    private void DeleteRideParticipantFromRide(Domain.Ride ride, User participant)
    {
        ride.Participants.Remove(participant);
        _db.SaveChanges();
    }
}