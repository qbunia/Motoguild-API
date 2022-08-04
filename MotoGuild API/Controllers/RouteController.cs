using Microsoft.AspNetCore.Mvc;
using MotoGuild_API.Models.Route;

namespace MotoGuild_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RouteController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetRoutes()
        {
            var routes = DataManager.Current.Routes;
            return Ok(routes);
        }
        
        [HttpGet("{id:int}", Name = "GetRoute")]
        public IActionResult GetRoute(int id)
        {
            var route = DataManager.Current.Routes.FirstOrDefault(r => r.Id == id);
            if (route == null)
            {
                return NotFound();
            }
            return Ok(route);
        }

        [HttpPost]
        public IActionResult CreateRoute([FromBody] CreateRouteDto createRouteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int newId = GetNewId();
            var route = SaveRouteToDataManager(createRouteDto, newId);
            return CreatedAtRoute("GetRoute", new { id = route.Id }, route);
        }

        private int GetNewId()
        {
            var lastId = DataManager.Current.Routes.Max(r => r.Id);
            return lastId + 1;
        }

        private RouteDto SaveRouteToDataManager(CreateRouteDto createRouteDto, int id)
        {
            RouteDto route = new RouteDto();
            {
                route.Id = id;
                route.Name = createRouteDto.Name;
                route.Description = createRouteDto.Description;
                route.StartPlace = createRouteDto.StartPlace;
                route.EndingPlace = createRouteDto.EndingPlace;
            };
            DataManager.Current.Routes.Add(route);
            return route;
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteRoute(int id)
        {
            var route = DataManager.Current.Routes.FirstOrDefault(r => r.Id == id);
            if (route == null)
            {
                return NotFound();
            }
            DataManager.Current.Routes.Remove(route);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateRoute(int id, [FromBody] UpdateRouteDto updateRouteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var route = DataManager.Current.Routes.FirstOrDefault(r => r.Id == id);
            if (route == null)
            {
                return NotFound();
            }
            route.Name = updateRouteDto.Name;
            route.Description = updateRouteDto.Description;
            route.StartPlace = updateRouteDto.StartPlace;
            route.EndingPlace = updateRouteDto.EndingPlace;
            return NoContent();
        }











    }
}
