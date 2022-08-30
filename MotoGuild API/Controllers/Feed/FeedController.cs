
using AutoMapper;
using Data;
using Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Models.Feed;
using MotoGuild_API.Models.Post;
using MotoGuild_API.Models.User;
using MotoGuild_API.Repository.Interface;

namespace MotoGuild_API.Controllers
{
    [ApiController]
    [Route("api/feeds")]
    [EnableCors("AllowAnyOrigin")]
    public class FeedController : ControllerBase
    {
        private readonly IFeedRepository _feedRepository;
        private readonly IMapper _mapper;

        public FeedController(IFeedRepository feedRepository, IMapper mapper)
        {
            _feedRepository = feedRepository;
            _mapper = mapper;
        }

        [HttpGet("{id:int}", Name = "GetFeed")]
        public IActionResult GetFeed(int id)
        {
            var feed = _feedRepository.Get(id);
            if (feed == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<FeedDto>(feed));
        }

        [HttpPost]
        public IActionResult CreateFeed()
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var feed = new Feed()
            {
                Posts = new List<Post>()
            };

            _feedRepository.Insert(feed);
            _feedRepository.Save();
            var feedDto = _mapper.Map<FeedDto>(feed);
            return CreatedAtRoute("GetFeed", new { id = feedDto.Id }, feedDto);
        }

    }
}
