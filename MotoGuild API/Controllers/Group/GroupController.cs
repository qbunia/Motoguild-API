using System.Text.Json;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MotoGuild_API.Dto.GroupDtos;
using MotoGuild_API.Helpers;
using MotoGuild_API.Repository.Interface;

namespace MotoGuild_API.Controllers;

[ApiController]
[Route("api/groups")]
public class GroupController : ControllerBase
{
    private readonly IGroupRepository _groupRepository;
    private readonly IMapper _mapper;

    public GroupController(IGroupRepository groupRepository, IMapper mapper)
    {
        _groupRepository = groupRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetGroups([FromQuery] PaginationParams @params)
    {
        var paginationMetadata = new PaginationMetadata(_groupRepository.TotalNumberOfGroups(), @params.Page,
            @params.ItemsPerPage);
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
        var groups = _groupRepository.GetAll(@params);
        return Ok(_mapper.Map<List<SelectedGroupDto>>(groups));
    }

    [HttpGet("{id:int}", Name = "GetGroup")]
    public IActionResult GetGroup(int id, [FromQuery] bool selectedData = false)
    {
        var group = _groupRepository.Get(id);
        if (group == null) return NotFound();
        return Ok(_mapper.Map<SelectedGroupDto>(group));
    }

    [HttpPost]
    public IActionResult CreateGroup([FromBody] CreateGroupDto createGroupDto)
    {
        var group = _mapper.Map<Group>(createGroupDto);
        _groupRepository.Insert(group);
        _groupRepository.Save();
        var groupDto = _mapper.Map<SelectedGroupDto>(group);
        return CreatedAtRoute("GetGroup", new {id = groupDto.Id}, groupDto);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteGroup(int id)
    {
        var group = _groupRepository.Get(id);
        if (group == null) return NotFound();

        _groupRepository.Delete(id);
        _groupRepository.Save();
        return Ok();
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateGroup(int id, [FromBody] UpdateGroupDto updateGroupDto)
    {
        updateGroupDto.Id = id;
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var updateGroup = _mapper.Map<Group>(updateGroupDto);
        _groupRepository.Update(updateGroup);
        _groupRepository.Save();
        return NoContent();
    }

}