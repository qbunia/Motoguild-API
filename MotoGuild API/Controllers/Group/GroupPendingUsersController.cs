﻿using AutoMapper;
using Data;
using Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Dto.UserDtos;

namespace MotoGuild_API.Controllers;

[ApiController]
[Route("api/groups/{groupId:int}/pendingusers")]
[EnableCors("AllowAnyOrigin")]
public class GroupPendingUsersController : ControllerBase
{
    private readonly IGroupPendingUsersRepository _groupPendingUsersRepository;
    private readonly IMapper _mapper;

    public GroupPendingUsersController(IGroupPendingUsersRepository groupPendingUsersRepository, IMapper mapper)
    {
        _groupPendingUsersRepository = groupPendingUsersRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetGroupPendingUsers(int groupId)
    {
        var pendingUsers = _groupPendingUsersRepository.GetAll(groupId);
        if (pendingUsers == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<List<UserDto>>(pendingUsers));
    }

    [HttpGet("{id:int}", Name = "GetGroupPendingUser")]
    public IActionResult GetGroupPendingUser(int groupId, int id)
    {
        var pendingUsers = _groupPendingUsersRepository.Get(groupId, id);
        if (pendingUsers == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<UserDto>(pendingUsers));
    }

    [HttpPost("{id:int}")]
    public IActionResult AddGroupPendingUserByUserId(int groupId, int id)
    {
        if (!_groupPendingUsersRepository.GroupExist(groupId) || !_groupPendingUsersRepository.UserExits(id))
        {
            return NotFound();
        }

        if (_groupPendingUsersRepository.UserInPendingUsers(groupId, id))
        {
            return BadRequest();
        }
        _groupPendingUsersRepository.AddPendingUserByUserId(groupId, id);
        _groupPendingUsersRepository.Save();
        var user = _groupPendingUsersRepository.GetUser(id);
        var userDto = _mapper.Map<UserDto>(user);
        return CreatedAtRoute("GetGroupPendingUser", new { id = userDto.Id, groupId = groupId }, userDto);
    }


    [HttpDelete("{id:int}")]
    public IActionResult DeleteGroupPendingUserByUserId(int groupId, int id)
    {
        if (!_groupPendingUsersRepository.GroupExist(groupId) || !_groupPendingUsersRepository.UserExits(id))
        {
            return NotFound();
        }

        if (!_groupPendingUsersRepository.UserInPendingUsers(groupId, id))
        {
            return NotFound();
        }
        _groupPendingUsersRepository.DeletePendingUserByUserId(groupId, id);
        _groupPendingUsersRepository.Save();

        return NoContent();
    }

}