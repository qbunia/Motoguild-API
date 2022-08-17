using Data;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Models.Ride;

namespace MotoGuild_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RideController : ControllerBase
    {
        private MotoGuildDbContext _db;

        public RideController(MotoGuildDbContext dbContext)
        {
            _db = dbContext;
        }

        [HttpGet]
        public IActionResult GetRides()
        {
            var rides = _db.Rides.Include(r => r.Owner).Include(r => r.Participants).ToList();

            var RidesDto = GetRidesDto(rides);
            return Ok(RidesDto);
        }

        [HttpGet("{id}", Name = "GetRide")]
        public IActionResult GetRide(int id, [FromQuery] bool selectedData = false)
        {
            Ride ride = _db.Rides
                .Include(r => r.Owner)
                .Include(r => r.Participants)
                .FirstOrDefault(r => r.Id == id);
            if (ride == null)
            {
                return NotFound();
            }

            var rideDto = GetRideDto(ride);
            return Ok(rideDto);
        }

        [HttpPost]
        public IActionResult CreateRide([FromBody] CreateRideDto createRideDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!UserExists(createRideDto.OwnerId))
            {
                ModelState.AddModelError(key: "Description", errorMessage: "User not found");
            }

            var ride = SaveRideToDatabase(createRideDto);
            var rideDto = GetRideDto(ride);
            return CreatedAtRoute("GetRode", new {id = rideDto.Id}, rideDto);
        }

        private bool UserExists(int id)
        {
            return _db.Users.FirstOrDefault(u => u.Id == id) != null;
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRide(int id, [FromBody] UpdateRideDto updateRideDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ride = _db.Rides.FirstOrDefault(r => r.Id == id);
            if (ride == null)
            {
                return NotFound();
            }
            UpdateRideInDatabase(ride, updateRideDto);
            return NoContent();
        }
        
        [HttpDelete("{id;int}")]
        public IActionResult DeleteRide(int id)
        {
            var ride = _db.Rides.FirstOrDefault(r => r.Id == id);
            if (ride == null)
            {
                return NotFound();
            }
            _db.Rides.Remove(ride);
            _db.SaveChanges();
            return NoContent();
        }
        
        private Ride SaveRideToDatabase(CreateRideDto createRideDto)
        {
            var ride = new Ride
            {
                Name = createRideDto.Name,
                Description = createRideDto.Description,
                Owner = createRideDto.Owner,
                StartPlace = createRideDto.StartPlace,
                StartTime = createRideDto.StartTime,
                EndPlace = createRideDto.EndPlace,
  
            };
            _db.Rides.Add(ride);
            _db.SaveChanges();
            return ride;
        }
        
        private void UpdateRideInDatabase(Ride ride, UpdateRideDto updateRideDto)
        {
            ride.Name = updateRideDto.Name;
            ride.Description = updateRideDto.Description;
            ride.Owner = updateRideDto.Owner;
            ride.StartPlace = updateRideDto.StartPlace;
            ride.StartTime = updateRideDto.StartTime;
            ride.EndPlace = updateRideDto.EndPlace;
            _db.SaveChanges();
        }
        
        private RideDto GetRideDto(Ride ride)
        {
            return new RideDto
            {
                Id = ride.Id,
                Name = ride.Name,
                Description = ride.Description,
                Owner = ride.Owner,
                StartPlace = ride.StartPlace,
                StartTime = ride.StartTime,
                EndPlace = ride.EndPlace,
               
            };
        }
        
        private List<RideDto> GetRidesDto(List<Ride> rides)
        {
            return rides.Select(r => new RideDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Owner = r.Owner,
                StartPlace = r.StartPlace,
                StartTime = r.StartTime,
                EndPlace = r.EndPlace,
               
            }).ToList();
        }
        
        
    }
}