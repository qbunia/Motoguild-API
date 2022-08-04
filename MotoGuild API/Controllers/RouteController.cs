using Microsoft.AspNetCore.Mvc;

namespace MotoGuild_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
            return GetRoute(id);
        }
        
        //[HttpPost]
        //public IActionResult CreateRoute([FromBody] CreateRouteDto createRouteDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    int newId = GetNewId();
        //    var route = SaveRouteToDataManager(createRouteDto, newId);
        //    return CreatedAtRoute("GetRoute", new { id = route.Id }, route);
        //}
        
        //[HttpDelete("{id:int}")]
        //public IActionResult DeleteRoute(int id)
        //{
        //    var route = DataManager.Current.Routes.FirstOrDefault(r => r.Id == id);
        //    if (route == null || routeSelectedData == null)
        //    {
        //        return NotFound();
        //    }
        //    DataManager.Current.Routes.Remove(route);
        //    return Ok();
        //}
        
        //[HttpPut("{id:int}")]
        //public IActionResult UpdateRoute(int id, [FromBody] UpdateRouteDto updateRouteDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var route = DataManager.Current.Routes.FirstOrDefault(r => r.Id == id);
        //    if (route == null)
        //    {
        //        return NotFound();
        //    }
        //    route = SaveRouteToDataManager(updateRouteDto, id);
        //    return Ok(route);
        //}

        //[HttpGet("{id:int}/stops", Name = "GetRouteStops")]
        //public IActionResult GetRouteStops(int id)
        //{
        //    var route = DataManager.Current.Routes.FirstOrDefault(r => r.Id == id);
        //    if (route == null)
        //    {
        //        return NotFound();
        //    }
        //    var stops = DataManager.Current.Stops.Where(s => s.RouteId == id);
        //    return Ok(stops);
        //}

        
        
        
        
            
            
    }
}
