using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MotoGuild_API.Dto.UserDtos;
using MotoGuild_API.Repository.Interface;

namespace MotoGuild_API.Controllers;

[ApiController]
[Route("api/groups/{groupId:int}/participants")]
[EnableCors("AllowAnyOrigin")]
public class GroupParticipantsController : ControllerBase
{
    private readonly IGroupParticipantsRepository _groupParticipantsRepository;
    private readonly IMapper _mapper;

    public GroupParticipantsController(IGroupParticipantsRepository groupParticipantsRepository, IMapper mapper)
    {
        _groupParticipantsRepository = groupParticipantsRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetGroupParticipants(int groupId)
    {
        var participants = _groupParticipantsRepository.GetAll(groupId);
        if (participants == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<List<UserDto>>(participants));
    }

    [HttpGet("{id:int}", Name = "GetGroupParticipant")]
    public IActionResult GetGroupParticipant(int groupId, int id)
    {
        var participant = _groupParticipantsRepository.Get(groupId, id);
        if (participant == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<UserDto>(participant));
    }

    [HttpPost("{id:int}")]
    public IActionResult AddGroupParticipantByUserId(int groupId, int id)
    {
        if (!_groupParticipantsRepository.GroupExist(groupId) || !_groupParticipantsRepository.UserExits(id))
        {
            return NotFound();
        }

        if (_groupParticipantsRepository.UserInGroup(groupId, id))
        {
            return BadRequest();
        }
        _groupParticipantsRepository.AddParticipantByUserId(groupId, id);
        _groupParticipantsRepository.Save();
        var user = _groupParticipantsRepository.GetUser(id);
        var userDto = _mapper.Map<UserDto>(user);
        return CreatedAtRoute("GetGroupParticipant", new { id = userDto.Id, groupId = groupId }, userDto);
    }


    [HttpDelete("{id:int}")]
    public IActionResult DeleteGroupParticipantByUserId(int groupId, int id)
    {
        if (!_groupParticipantsRepository.GroupExist(groupId) || !_groupParticipantsRepository.UserExits(id))
        {
            return NotFound();
        }

        if (!_groupParticipantsRepository.UserInGroup(groupId, id))
        {
            return NotFound();
        }
        _groupParticipantsRepository.DeleteParticipantByUserId(groupId, id);
        _groupParticipantsRepository.Save();

        return NoContent();
    }

}