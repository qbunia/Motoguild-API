using Data;
using Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Models.Post;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Controllers.Ride.Post
{
    [ApiController]
    [Route("api/rides/{rideId:int}/posts")]
    [EnableCors("AllowAnyOrigin")]
    public class RidePostsController : ControllerBase
    {
        private MotoGuildDbContext _db;

        public RidePostsController(MotoGuildDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetRidePosts(int rideId)
        {
            var posts = _db.Rides.Include(r => r.Posts)
                .Where(r => r.Id == rideId).Select(r => r.Posts).First().ToList();
            var postsDto = GetRidePostsDtos(posts);
            return Ok(postsDto);
        }

        private List<PostDto> GetRidePostsDtos(List<Domain.Post> posts)
        {
            var postsDto = new List<PostDto>();
            //foreach (var post in posts)
            //{
            //    var authorDto = new UserSelectedDataDto
            //    {
            //        Email = post.Author.Email,
            //        Id = post.Author.Id,
            //        Rating = post.Author.Rating,
            //        UserName = post.Author.UserName
            //    };


            //    postsDto.Add(new PostDto
            //    {
            //        Author = authorDto,
            //        Content = post.Content,
            //        CreateTime = post.CreateTime,
            //        Id = post.Id
            //    });
            //}
            return postsDto;
        }

        [HttpGet("{id:int}", Name = "GetRidePost")]
        public IActionResult GetRidePost(int rideId, int id)
        {
            var ride = _db.Rides
                .Include(r => r.Posts)
                .FirstOrDefault(r => r.Id == rideId);
            if (ride == null)
            {
                return NotFound();
            }
            var post = _db.Posts
                .Include(p => p.Author)
                .FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            var postDto = GetRidePostDto(post);
            return Ok(postDto);
        }

        private PostDto GetRidePostDto(Domain.Post post)
        {
            //var authorDto = new UserSelectedDataDto
            //{
            //    Email = post.Author.Email,
            //    Id = post.Author.Id,
            //    Rating = post.Author.Rating,
            //    UserName = post.Author.UserName
            //};

            //var postDto = new PostDto
            //{
            //    Author = authorDto,
            //    Content = post.Content,
            //    CreateTime = post.CreateTime,
            //    Id = post.Id
            //};
            return  new PostDto();
        }

        [HttpPost]
        public IActionResult CreateRidePost(int rideId, [FromBody] CreatePostDto createPostDto)
        {
            if (createPostDto == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ride = _db.Rides.FirstOrDefault(r => r.Id == rideId);
            if (ride == null)
            {
                return NotFound();
            }
            var user = _db.Users.FirstOrDefault(u => u.Id == createPostDto.Author.Id);
            if (user == null)
            {
                return NotFound();
            }
            var post = SaveRideToDataBase(createPostDto, ride);
            _db.Posts.Add(post);
            _db.SaveChanges();
            var postDto = GetRidePostDto(post);
            return CreatedAtRoute("GetRidePost", new { rideId, id = post.Id }, postDto);
        }

        private Domain.Post SaveRideToDataBase(CreatePostDto createPostDto, Domain.Ride ride)
        {
            var author = _db.Users.FirstOrDefault(u => u.Id == createPostDto.Author.Id);
            var post = new Domain.Post
            {
                Author = author,
                Comments = new List<Domain.Comment>(),
                Content = createPostDto.Content,
                CreateTime = DateTime.Now
            };
            ride.Posts.Add(post);
            _db.SaveChanges();
            return post;
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteRidePost(int rideId, int id)
        {
            var ride = _db.Rides.FirstOrDefault(r => r.Id == rideId);
            if (ride == null)
            {
                return NotFound();
            }
            var post = _db.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            _db.Posts.Remove(post);
            _db.SaveChanges();
            return NoContent();
        }

    }
}
