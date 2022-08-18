using Data;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Models.Route;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Controllers.Route
{
    [ApiController]
    [Route("api/routes")]
    public class RouteController : ControllerBase
    {
        private readonly MotoGuildDbContext _db;

        public RouteController(MotoGuildDbContext dbContext)
        {
            _db = dbContext;
        }

        [HttpGet]
        public IActionResult GetRoutes()
        {
            var routes = _db.Routes
                .Include(r => r.Owner)
                .ToList();
            var routesDto = GetRoutesDtos(routes);
            return Ok(routesDto);
        }

        [HttpGet("{id:int}", Name = "GetRoute")]
        public IActionResult GetRoute(int id, [FromQuery] bool selectedData = false)
        {
            var route = _db.Routes
                .Include(r => r.Owner)
                .FirstOrDefault(r => r.Id == id);
            if (route == null) return NotFound();

            var routeDto = GetRouteDto(route);
            return Ok(routeDto);
        }

        [HttpPost]
        public IActionResult CreateRoute([FromBody] CreateRouteDto createRouteDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var route = SaveRouteToDataBase(createRouteDto);
            var routeDto = GetRouteDto(route);
            return CreatedAtRoute("GetRoute", new { id = routeDto.Id }, routeDto);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteRoute(int id)
        {
            var route = _db.Routes
                .FirstOrDefault(r => r.Id == id);
            if (route == null) return NotFound();

            _db.Routes.Remove(route);
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateRoute(int id, [FromBody] UpdateRouteDto updateRouteDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var route = _db.Routes.FirstOrDefault(i => i.Id == id);
            if (route == null) return NotFound();
            UpdateGroupData(route, updateRouteDto);
            return NoContent();
        }

        private void UpdateGroupData(Domain.Route route, UpdateRouteDto updateGroupDto)
        {
            route.Description = updateGroupDto.Description;
            route.Name = updateGroupDto.Name;
            route.StartPlace = updateGroupDto.StartPlace;
            route.EndingPlace = updateGroupDto.EndingPlace;
            route.Rating = updateGroupDto.Rating;

            _db.SaveChanges();
        }


        private Domain.Route SaveRouteToDataBase(CreateRouteDto createRouteDto)
        {
            var owner = _db.Users.FirstOrDefault(u => u.Id == createRouteDto.Owner.Id);
            var route = new Domain.Route
            {
                Description = createRouteDto.Description,
                EndingPlace = createRouteDto.EndingPlace,
                Name = createRouteDto.Name,
                Owner = owner,
                Posts = new List<Post>(),
                Rating = createRouteDto.Rating,
                StartPlace = createRouteDto.StartPlace,
                Stops = new List<Stop>()
            };
            _db.Routes.Add(route);
            _db.SaveChanges();
            return route;
        }


        private List<RouteDto> GetRoutesDtos(List<Domain.Route> routes)
        {
            var routeDtos = new List<RouteDto>();
            foreach (var route in routes)
            {
                var owner = _db.Users.FirstOrDefault(u => u.Id == route.Owner.Id);
                var ownerDto = new UserDto()
                {
                    Email = owner.Email,
                    Id = owner.Id,
                    Rating = owner.Rating,
                    UserName = owner.UserName
                };
                routeDtos.Add(new RouteDto
                {
                    Description = route.Description,
                    EndingPlace = route.EndingPlace,
                    Id = route.Id,
                    Name = route.Name,
                    Rating = route.Rating,
                    StartPlace = route.StartPlace,
                    Owner = ownerDto
                });
            }

            return routeDtos;
        }

        private RouteDto GetRouteDto(Domain.Route route)
        {
            var owner = _db.Users.FirstOrDefault(u => u.Id == route.Owner.Id);
            var ownerDto = new UserDto()
            {
                Email = owner.Email,
                Id = owner.Id,
                Rating = owner.Rating,
                UserName = owner.UserName
            };
            var routeDto = new RouteDto
            {
                Description = route.Description,
                EndingPlace = route.EndingPlace,
                Id = route.Id,
                Name = route.Name,
                Rating = route.Rating,
                StartPlace = route.StartPlace,
                Owner = ownerDto
            };
            return routeDto;
        }
    }
}
