using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

using MotoGuild_API.Repository.Interface;
using System;
using Igor.Gateway.Dtos.Events;
using MotoGuild_API.Dto.EventDtos;
using EventDto = MotoGuild_API.Dto.EventDtos.EventDto;

namespace MotoGuild_API.Controllers;

[ApiController]
[Route("api/events")]

public class EventController : ControllerBase
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public EventController(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetEvents()
    {
        var events = _eventRepository.GetAll();
        var eventsDto = _mapper.Map<IEnumerable<EventDto>>(events);
        return Ok(eventsDto);
    }

    [HttpGet("{id}", Name = "GetEvent")]
    public IActionResult GetEvent(int id, [FromQuery] bool includePosts = false)
    {
        var eve = _eventRepository.Get(id);
        if (eve == null)
        {
        return NotFound();
        }
        return Ok(_mapper.Map<EventDto>(eve));
    }

    [HttpPost]
    public IActionResult CreateEvent([FromBody] CreateEventDto createEventDto)
    {
        var eve = _mapper.Map<Event>(createEventDto);
        _eventRepository.Insert(eve);
        _eventRepository.Save();
        var eventDto = _mapper.Map<EventDto>(eve);
        return CreatedAtRoute("GetEvent", new { id = eve.Id }, eventDto);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateEvent(int id, [FromBody] UpdateEventDto updateEventDto)
    {
        var eve = _eventRepository.Get(id);
        if (eve == null)
        {
            return NotFound();
        }
        _mapper.Map(updateEventDto, eve);
        _eventRepository.Update(eve);
        _eventRepository.Save();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteEvent(int id)
    {
        var eve = _eventRepository.Get(id);
        if (eve == null)
        {
            return NotFound();
        }
        _eventRepository.Delete(id);
        _eventRepository.Save();
        return NoContent();
    }









}