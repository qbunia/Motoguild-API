using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MotoGuild_API.Dto.RouteDtos;
using MotoGuild_API.Repository.Interface;

namespace MotoGuild_API.Controllers.Route
{
    [ApiController]
    [Route("api/routes")]
    [EnableCors("AllowAnyOrigin")]
    public class RouteController : ControllerBase
    {

        private readonly IRouteRepository _routeRepository;
        private readonly IMapper _mapper;

        public RouteController(IRouteRepository routeRepository, IMapper mapper)
        {
            _routeRepository = routeRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetRoutes([FromQuery] bool orderByRating = false)
        {
            if (!orderByRating)
            {
                var routes = _routeRepository.GetAll();
                return Ok(_mapper.Map<List<RouteDto>>(routes));
            }
            else
            {
                var selectedRoutes = _routeRepository.GetFiveOrderByRating();
                return Ok(_mapper.Map<List<RouteDto>>(selectedRoutes));
            }

        }

        [HttpGet("{id:int}", Name = "GetRoute")]
        public IActionResult GetRoute(int id)
        {
            var route = _routeRepository.Get(id);
            if (route == null) return NotFound();
            return Ok(_mapper.Map<FullRouteDto>(route));

        }

        [HttpPost]
        public IActionResult CreateRoute([FromBody] CreateRouteDto createRouteDto)
        {
            var route = _mapper.Map<Domain.Route>(createRouteDto);
            _routeRepository.Insert(route);
            _routeRepository.Save();
            var routeDto = _mapper.Map<FullRouteDto>(route);
            return CreatedAtRoute("GetRoute", new { id = routeDto.Id }, routeDto);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteRoute(int id)
        {
            var route = _routeRepository.Get(id);
            if (route == null) return NotFound();

            _routeRepository.Delete(id);
            _routeRepository.Save();
            return Ok();

        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateRoute(int id, [FromBody] UpdateRouteDto updateRouteDto)
        {
            updateRouteDto.Id = id;
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updateRoute = _mapper.Map<Domain.Route>(updateRouteDto);
            _routeRepository.Update(updateRoute);
            _routeRepository.Save();
            return NoContent();

        }

    }
}
