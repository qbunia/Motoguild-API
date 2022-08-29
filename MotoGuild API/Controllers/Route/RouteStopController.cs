using System.Linq;
using Data;
using Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Models.Route;
using MotoGuild_API.Models.Stops;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Controllers.Route
{
    [ApiController]
    [Route("api/routes/{routeId:int}/stops")]
    [EnableCors("AllowAnyOrigin")]
    public class RouteStopController : ControllerBase
    {
        private readonly MotoGuildDbContext _db;

        public RouteStopController(MotoGuildDbContext dbContext)
        {
            _db = dbContext;
        }

        [HttpGet]
        public IActionResult GetRouteStops(int routeId)
        {
            var route = _db.Routes
                .Include(r => r.Stops)
                .FirstOrDefault(r => r.Id == routeId);

            if (route == null)
            {
                return NotFound();
            }

            var stopsId = route.Stops.Select(s => s.Id).ToList();
            var stops = _db.Stops.Where(s => stopsId.Contains(s.Id)).ToList();

            var stopsDto = GetRouteStopsDtos(stops);
            return Ok(stopsDto);
        }

        [HttpGet("{id:int}", Name = "GetRouteStop")]
        public IActionResult GetRouteStop(int routeId,int id, [FromQuery] bool selectedData = false)
        {
            var route = _db.Routes
                .Include(r => r.Stops)
                .FirstOrDefault(r => r.Id == routeId);

            if (route == null)
            {
                return NotFound();
            }
            var stop = _db.Stops.FirstOrDefault(s => s.Id == id);

            if (stop == null || !route.Stops.Contains(stop))
            {
                return NotFound();
            }

            var stopDto = GetRouteStopDto(stop);
            return Ok(stopDto);
        }

        [HttpPost]
        public IActionResult CreateRouteStop(int routeId,[FromBody] CreateStopDto createStopDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var route = _db.Routes
                .Include(r => r.Stops)
                .FirstOrDefault(r => r.Id == routeId);

            if (route == null)
            {
                return NotFound();
            }

            var stop = SaveRouteToDataBase(createStopDto, route);
            var stopDto = GetRouteStopDto(stop);
            return CreatedAtRoute("GetRouteStop", new { id = stopDto.Id, routeId =  routeId}, stopDto);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteRouteStop(int routeId,int id)
        {
            var route = _db.Routes
                .Include(r => r.Stops)
                .FirstOrDefault(r => r.Id == routeId);

            if (route == null)
            {
                return NotFound();
            }
            var stop = _db.Stops.FirstOrDefault(s => s.Id == id);

            if (stop == null || !route.Stops.Contains(stop))
            {
                return NotFound();
            }

            _db.Stops.Remove(stop);
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateRouteStop(int routeId,int id, [FromBody] UpdateStopDto updateStopDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var route = _db.Routes
                .Include(r => r.Stops)
                .FirstOrDefault(r => r.Id == routeId);

            if (route == null)
            {
                return NotFound();
            }
            var stop = _db.Stops.FirstOrDefault(s => s.Id == id);

            if (stop == null || !route.Stops.Contains(stop))
            {
                return NotFound();
            }

            UpdateRouteStopData(stop, updateStopDto);
            return NoContent();
        }

        private void UpdateRouteStopData(Stop stop, UpdateStopDto updateStopDto)
        {
            stop.Description = updateStopDto.Description;
            stop.Name = updateStopDto.Name;
            stop.Place = updateStopDto.Place;

            _db.SaveChanges();
        }


        private Stop SaveRouteToDataBase(CreateStopDto createStopDto, Domain.Route route)
        {
            var stop = new Stop()
            {
                Description = createStopDto.Description,
                Name = createStopDto.Name,
                Place = createStopDto.Place
            };
            route.Stops.Add(stop);
            _db.SaveChanges();
            return stop;
        }


        private List<StopDto> GetRouteStopsDtos(List<Stop> stops)
        {
            var stopsDtos = new List<StopDto>();
            foreach (var stop in stops)
            {
                stopsDtos.Add(new StopDto
                {
                    Description = stop.Description,
                    Id = stop.Id,
                    Name = stop.Name,
                    Place = stop.Place
                });
            }

            return stopsDtos;
        }

        private StopDto GetRouteStopDto(Stop stop)
        {
            var stopDto = new StopDto
            {
                Description = stop.Description,
                Id = stop.Id,
                Name = stop.Name,
                Place = stop.Place
            };
            

            return stopDto;
        }
    }
}
