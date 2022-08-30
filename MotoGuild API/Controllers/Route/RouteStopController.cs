﻿using Data;
using Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Dto.StopDtos;

namespace MotoGuild_API.Controllers.Route
{
    [ApiController]
    [Route("api/routes/{routeId:int}/stops")]
    [EnableCors("AllowAnyOrigin")]
    public class RouteStopController : ControllerBase
    {
        private readonly IRouteStopsRepository _routeStopsRepository;
        private readonly IMapper _mapper;

        public RouteStopController(IRouteStopsRepository routeStopsRepository, IMapper mapper)
        {
            _routeStopsRepository = routeStopsRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetRouteStops(int routeId)
        {
            var stops = _routeStopsRepository.GetAll(routeId);
            return Ok(_mapper.Map<List<StopDto>>(stops));
        }

        [HttpGet("{id:int}", Name = "GetRouteStop")]
        public IActionResult GetRouteStop(int id, int routeId)
        {
            var stop = _routeStopsRepository.Get(id, routeId);
            if (stop == null) return NotFound();
            return Ok(_mapper.Map<StopDto>(stop));
        }

        [HttpPost]
        public IActionResult CreateRouteStop(int routeId, [FromBody] CreateStopDto createStopDto)
        {
            var stop = _mapper.Map<Stop>(createStopDto);
            _routeStopsRepository.Insert(stop, routeId);
            _routeStopsRepository.Save();
            var stopDto = _mapper.Map<StopDto>(stop);
            return CreatedAtRoute("GetRouteStop", new { id = stopDto.Id, routeId = routeId }, stopDto);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteRouteStop(int routeId, int id)
        {
            var stop = _routeStopsRepository.Get(id, routeId);
            if (stop == null) return NotFound();

            _routeStopsRepository.Delete(id, routeId);
            _routeStopsRepository.Save();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateRouteStop(int routeId, int id, [FromBody] UpdateStopDto updateStopDto)
        {
            if (!_routeStopsRepository.StopExistsInRoute(id, routeId)) return NotFound();
            updateStopDto.Id = id;
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updateStop = _mapper.Map<Stop>(updateStopDto);
            _routeStopsRepository.Update(updateStop);
            _routeStopsRepository.Save();
            return NoContent();
        }

    }
}
