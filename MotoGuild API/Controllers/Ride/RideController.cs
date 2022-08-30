using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MotoGuild_API.Dto.RideDtos;
using MotoGuild_API.Repository.Interface;

namespace MotoGuild_API.Controllers;

[ApiController]
[Route("api/rides")]
[EnableCors("AllowAnyOrigin")]

public class RideController : ControllerBase
{
    private readonly IRideRepository _rideRepository;
    private readonly IMapper _mapper;

    public RideController(IRideRepository rideRepository, IMapper mapper)
    {
        _rideRepository = rideRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetRides()
    {
        var rides = _rideRepository.GetAll();
        return Ok(_mapper.Map<List<RideDto>>(rides));
    }

    [HttpGet("{rideId:int}", Name = "GetRide")]
    public IActionResult GetRide(int rideId)
    {
        var ride = _rideRepository.Get(rideId);
        if (ride == null) return NotFound();
        return Ok(_mapper.Map<RideDto>(ride));
    }

    [HttpPost]
    public IActionResult CreateRide([FromBody] CreateRideDto createRideDto)
    {
        var ride = _mapper.Map<Domain.Ride>(createRideDto);
        _rideRepository.Insert(ride);
        _rideRepository.Save();
        var rideDto = _mapper.Map<RideDto>(ride);
        return CreatedAtRoute("GetRide", new { id = rideDto.Id }, rideDto);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteRide(int id)
    {
        var ride = _rideRepository.Get(id);
        if (ride == null) return NotFound();
        _rideRepository.Delete(id);
        _rideRepository.Save();
        return NoContent();
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateRide(int id, [FromBody] UpdateRideDto updateRideDto)
    {
        var ride = _rideRepository.Get(id);
        if (ride == null) return NotFound();
        _mapper.Map(updateRideDto, ride);
        _rideRepository.Update(ride);
        _rideRepository.Save();
        return NoContent();
    }





}