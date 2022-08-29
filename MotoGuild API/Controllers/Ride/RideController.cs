using Data;
using Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Models.Ride;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAnyOrigin")]
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
            Domain.Ride ride = _db.Rides
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

            var ride = SaveRideToDatabase(createRideDto);
            var rideDto = GetRideDto(ride);
            return CreatedAtRoute("GetRide", new {id = rideDto.Id}, rideDto);
        }

        private bool UserExists(int id)
        {
            return _db.Users.FirstOrDefault(u => u.Id == id) != null;
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateRide(int id, [FromBody] UpdateRideDto updateRideDto)
        {
            var ride = _db.Rides.FirstOrDefault(r => r.Id == id);
            if (ride == null)
            {
                return NotFound();
            }
            UpdateRideData(ride, updateRideDto);
            return NoContent();
        }

        private void UpdateRideData(Domain.Ride ride, UpdateRideDto updateRideDto)
        {
            ride.Name = updateRideDto.Name;
            ride.Description = updateRideDto.Description;
            ride.StartPlace = updateRideDto.StartPlace;
            ride.StartTime = updateRideDto.StartTime;
            ride.EndingPlace = updateRideDto.EndingPlace;          
            ride.Estimation = updateRideDto.Estimation;
            ride.MinimumRating = updateRideDto.MinimumRating;
           
            _db.SaveChanges();
        }


        
        [HttpDelete("{id:int}")]
        public IActionResult DeleteRide(int id)
        {
            var ride = _db.Rides.FirstOrDefault(r => r.Id == id);
            if (ride == null)
            {
                return NotFound();
            }
            _db.Rides.Remove(ride);
            _db.SaveChanges();
            return Ok();
        }
        
        private Domain.Ride SaveRideToDatabase(CreateRideDto createRideDto)
        {
            User owner = _db.Users.FirstOrDefault(o => o.Id == createRideDto.Owner.Id);
            var participantsList = new List<User>();
            participantsList.Add(owner);

            
            var ride = new Domain.Ride
            {
                Name = createRideDto.Name,
                Description = createRideDto.Description,
                StartPlace = createRideDto.StartPlace,
                StartTime = createRideDto.StartTime,
                EndingPlace = createRideDto.EndingPlace,
                Owner = owner,
                Participants = participantsList,
                Estimation = createRideDto.Estimation,
                MinimumRating = createRideDto.MinimumRating,
                Posts = new List<Post>(),
                Stops = new List<Stop>()


            };
            
            _db.Rides.Add(ride);
            _db.SaveChanges();
            return ride;
        }
        
        private void UpdateRideInDatabase(Domain.Ride ride, UpdateRideDto updateRideDto)
        {
            ride.Name = updateRideDto.Name;
            ride.Description = updateRideDto.Description;
            ride.StartPlace = updateRideDto.StartPlace;
            ride.StartTime = updateRideDto.StartTime;
            ride.EndingPlace = updateRideDto.EndingPlace;
            _db.SaveChanges();
        }
        
        private RideDto GetRideDto(Domain.Ride ride)
        {
            return new RideDto
            {
                Id = ride.Id,
                Name = ride.Name,
                Description = ride.Description,
                StartPlace = ride.StartPlace,
                StartTime = ride.StartTime,
                EndingPlace = ride.EndingPlace,
               
            };
        }
        
        private List<RideDto> GetRidesDto(List<Domain.Ride> rides)
        {
            return rides.Select(r => new RideDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                StartPlace = r.StartPlace,
                StartTime = r.StartTime,
                EndingPlace = r.EndingPlace,
               

            }).ToList();
        }
        
        
    }
}