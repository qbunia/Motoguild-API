
using Data;
using Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Models.Feed;
using MotoGuild_API.Models.Post;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Controllers
{
    [ApiController]
    [Route("api/feeds")]
    [EnableCors("AllowAnyOrigin")]
    public class FeedController : ControllerBase
    {
        private MotoGuildDbContext _db;

        public FeedController(MotoGuildDbContext dbContext)
        {
            _db = dbContext;
        }

        [HttpGet("{id:int}", Name = "GetFeed")]
        public IActionResult GetFeed(int id)
        {
            var feed = _db.Feed.Include(f => f.Posts).ThenInclude(p => p.Author).FirstOrDefault(u => u.Id == id);
            if (feed == null)
            {
                return NotFound();
            }

            var FeedDto = GetFeedDto(feed);
            return Ok(FeedDto);
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

            _db.Feed.Add(feed);
            _db.SaveChanges();
            return CreatedAtRoute("GetFeed", new { id = feed.Id }, feed);
        }

        private FeedDto GetFeedDto(Feed feed)
        {
            var postsDtos = new List<PostDto>();
            foreach (var post in feed.Posts)
            {
                var authorDto = new UserDto()
                {
                    Email = post.Author.Email,
                    Id = post.Author.Id,
                    Rating = post.Author.Rating,
                    UserName = post.Author.UserName
                };

                postsDtos.Add(new PostDto()
                {
                    Author = authorDto,
                    Content = post.Content,
                    CreateTime = post.CreateTime,
                    Id = post.Id
                });
            }
            var feedDto = new FeedDto()
                {Id = feed.Id, Posts = postsDtos };
            return feedDto;
        }
    }
}
