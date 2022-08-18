
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
    [Route("api/feeds/{feedId:int}/posts")]
    [EnableCors("AllowAnyOrigin")]
    public class FeedPostController : ControllerBase
    {
        private MotoGuildDbContext _db;

        public FeedPostController(MotoGuildDbContext dbContext)
        {
            _db = dbContext;
        }

        [HttpGet]
        public IActionResult GetFeedPosts(int feedId)
        {

            var feed = _db.Feed
                .Include(g => g.Posts)
                .FirstOrDefault(g => g.Id == feedId);
            if (feed == null)
            {
                return NotFound();
            }

            var postsId = feed.Posts.Select(p => p.Id);

            var posts = _db.Posts
                .Include(p => p.Author)
                .Where(p => postsId.Contains(p.Id)).ToList();

            if (posts == null)
            {
                return NotFound();
            }
            var postsDto = GetFeedPostsDtos(posts);
            return Ok(postsDto);
        }

        [HttpGet("{id:int}", Name = "GetFeedPost")]
        public IActionResult GetFeedPost(int feedId, int id)
        {
            var feed = _db.Feed
                .Include(g => g.Posts)
                .FirstOrDefault(g => g.Id == feedId);
            if (feed == null)
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

            var postDto = GetFeedPostDto(post);
            return Ok(postDto);
        }

        [HttpPost]
        public IActionResult CreateFeedPost(int feedId, [FromBody] CreatePostDto createPostDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var feed = _db.Feed.Include(g => g.Posts).FirstOrDefault(g => g.Id == feedId);
            if (feed == null)
            {
                return NotFound();
            }
            var post = SaveFeedPostToDataBase(createPostDto, feed);
            var postDto = GetFeedPostDto(post);
            return CreatedAtRoute("GetFeedPost", new { feedId = feedId, id = postDto.Id }, postDto);
        }

        private Post SaveFeedPostToDataBase(CreatePostDto createUserDto, Feed feed)
        {
            var author = _db.Users.FirstOrDefault(u => u.Id == createUserDto.Author.Id);
            Post post = new Post()
            {
                Author = author,
                Comments = new List<Comment>(),
                Content = createUserDto.Content,
                CreateTime = DateTime.Now,
            };
            feed.Posts.Add(post);
            _db.SaveChanges();
            return post;
        }


        [HttpDelete("{id:int}")]
        public IActionResult DeleteFeedPost(int feedId, int id)
        {
            var feed = _db.Feed.Include(g => g.Posts).FirstOrDefault(u => u.Id == feedId);
            if (feed == null)
            {
                return NotFound();
            }

            var post = _db.Posts.FirstOrDefault(p => p.Id == id);
            if (!feed.Posts.Contains(post))
            {
                return NotFound();
            }

            _db.Posts.Remove(post);
            _db.SaveChanges();
            return Ok();
        }


        private List<PostDto> GetFeedPostsDtos(List<Post> posts)
        {
            var postsDtos = new List<PostDto>();
            foreach (var post in posts)
            {
                var authorDto = new UserSelectedDataDto()
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
            return postsDtos;
        }

        private PostDto GetFeedPostDto(Post post)
        {
            var authorDto = new UserSelectedDataDto()
            {
                Email = post.Author.Email,
                Id = post.Author.Id,
                Rating = post.Author.Rating,
                UserName = post.Author.UserName
            };

            var postDto = new PostDto()
            {
                Author = authorDto,
                Content = post.Content,
                CreateTime = post.CreateTime,
                Id = post.Id
            };
            return postDto;
        }
    }
}
