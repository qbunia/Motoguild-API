using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MotoGuild_API.Dto.UserDtos;
using MotoGuild_API.Repository.Interface;

namespace MotoGuild_API.Controllers;

[ApiController]
[Route("api/rides/{rideId:int}/participants")]
[EnableCors("AllowAnyOrigin")]
public class RideParticipantsController : ControllerBase
{
    private readonly IRideParticipantsRepository _rideParticipantsRepository;
    private readonly IMapper _mapper;

    public RideParticipantsController(IRideParticipantsRepository rideParticipantsRepository, IMapper mapper)
    {
        _rideParticipantsRepository = rideParticipantsRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetRideParticipants(int rideId)
    {
        var participants = _rideParticipantsRepository.GetAll(rideId);
        if (participants == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<List<UserDto>>(participants));
    }

    [HttpGet("{id:int}", Name = "GetRideParticipant")]
    public IActionResult GetRideParticipant(int rideId, int id)
    {
        var participant = _rideParticipantsRepository.Get(rideId, id);
        if (participant == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<UserDto>(participant));
    }

    [HttpPost("{id:int}")]
    public IActionResult AddRideParticipantByUserId(int rideId, int id)
    {
        if (!_rideParticipantsRepository.RideExist(rideId) || !_rideParticipantsRepository.UserExits(id))
        {
            return NotFound();
        }

        if (_rideParticipantsRepository.UserInRide(rideId, id))
        {
            return BadRequest();
        }
        _rideParticipantsRepository.AddParticipantByUserId(rideId, id);
        _rideParticipantsRepository.Save();
        var user = _rideParticipantsRepository.GetUser(id);
        var userDto = _mapper.Map<UserDto>(user);
        return CreatedAtRoute("GetRideParticipant", new { id = userDto.Id, rideId = rideId }, userDto);
    }


    [HttpDelete("{id:int}")]
    public IActionResult DeleteRideParticipantByUserId(int rideId, int id)
    {
        if (!_rideParticipantsRepository.RideExist(rideId) || !_rideParticipantsRepository.UserExits(id))
        {
            return NotFound();
        }

        if (!_rideParticipantsRepository.UserInRide(rideId, id))
        {
            return NotFound();
        }
        _rideParticipantsRepository.DeleteParticipantByUserId(rideId, id);
        _rideParticipantsRepository.Save();

        return NoContent();
    }

}