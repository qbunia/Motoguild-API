using System.Net;
using System.Runtime.ExceptionServices;
using Data;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Models.Comment;
using MotoGuild_API.Models.Group;
using MotoGuild_API.Models.Post;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Controllers
{
    [ApiController]
    [Route("api/groups/{groupId:int}/posts")]
    public class GroupPostsController : ControllerBase
    {
        private MotoGuildDbContext _db;

        public GroupPostsController(MotoGuildDbContext dbContext)
        {
            _db = dbContext;
        }

        [HttpGet]
        public IActionResult GetPosts(int groupId)
        {

            var group = _db.Groups
                .Include(g => g.Posts)
                .FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                return NotFound();
            }

            var postsId = group.Posts.Select(p => p.Id);

            var posts = _db.Posts
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .Where(p => postsId.Contains(p.Id)).ToList();

            if (posts == null)
            {
                return NotFound();
            }
            var postsDto = GetPostsDtos(posts);
            return Ok(postsDto);
        }

        [HttpGet("{id:int}", Name = "GetPost")]
        public IActionResult GetPost(int groupId, int id)
        {
            var group = _db.Groups
                .Include(g => g.Posts)
                .FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                return NotFound();
            }

            var post = group.Posts.FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            var postDto = GetPostDto(post);
            return Ok(postDto);
        }

        [HttpPost]
        public IActionResult CreatePost(int groupId, [FromBody] CreatePostDto createPostDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var post = SavePostToDataBase(createPostDto, groupId);
            var postDto = GetPostDto(post);
            return CreatedAtRoute("GetPost", new { id = postDto.Id }, postDto);
        }

        private Post SavePostToDataBase(CreatePostDto createUserDto, int groupId)
        {
            var author = _db.Users.FirstOrDefault(u => u.Id == createUserDto.Author.Id);
            var commentsId = createUserDto.Comments.Select(c => c.Id).ToList();
            var comments = _db.Comments.Where(c => commentsId.Contains(c.Id)).ToList();
            Post post = new Post()
            {
                Author = author,
                Comments = comments,
                Content = createUserDto.Content,
                CreateTime = createUserDto.CreateTime,
            };
            var group = _db.Groups.FirstOrDefault(g => g.Id == groupId);
            group.Posts.Add(post);
            _db.SaveChanges();
            return post;
        }

        private void AddParticipantToGroup(Group group, User participant)
        {
            group.Participants.Add(participant);
            _db.SaveChanges();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteParticipantByUserId(int groupId, int id)
        {

            var group = _db.Groups
                .Include(g => g.Participants)
                .FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                return NotFound();
            }

            var participant = _db.Users.FirstOrDefault(p => p.Id == id);

            if (participant == null)
            {
                return NotFound();
            }

            DeleteParticipantFromGroup(group, participant);
            //var participantDto = GetParticipantDto(participant);
            return Ok(participant);
        }

        private void DeleteParticipantFromGroup(Group group, User participant)
        {
            group.Participants.Remove(participant);
            _db.SaveChanges();
        }

        private List<PostDto> GetPostsDtos(List<Post> posts)
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

                var commentsDto = new List<CommentDto>();
                foreach (var comment in post.Comments)
                {
                    var commentauthorDto = new UserSelectedDataDto()
                    {
                        Email = comment.Author.Email,
                        Id = comment.Author.Id,
                        Rating = comment.Author.Rating,
                        UserName = comment.Author.UserName
                    };
                    commentsDto.Add(new CommentDto()
                    {
                        Author = commentauthorDto,
                        Content = comment.Content,
                        CreateTime = comment.CreateTime,
                        Id = comment.Id
                    });
                }
                postsDtos.Add(new PostDto()
                {
                    Author = authorDto,
                    Content = post.Content,
                    CreateTime = post.CreateTime,
                    Id = post.Id,
                    Comments = commentsDto
                });
            }
            return postsDtos;
        }

        private PostDto GetPostDto(Post post)
        {
            var authorDto = new UserSelectedDataDto()
            {
                Email = post.Author.Email,
                Id = post.Author.Id,
                Rating = post.Author.Rating,
                UserName = post.Author.UserName
            };

            var commentsDto = new List<CommentDto>();
            foreach (var comment in post.Comments)
            {
                var commentauthorDto = new UserSelectedDataDto()
                {
                    Email = comment.Author.Email,
                    Id = comment.Author.Id,
                    Rating = comment.Author.Rating,
                    UserName = comment.Author.UserName
                };
                commentsDto.Add(new CommentDto()
                {
                    Author = commentauthorDto,
                    Content = comment.Content,
                    CreateTime = comment.CreateTime,
                    Id = comment.Id
                });
            }
            var postDto = new PostDto()
            {
                Author = authorDto,
                Content = post.Content,
                CreateTime = post.CreateTime,
                Id = post.Id,
                Comments = commentsDto
            };
            return postDto;
        }
    }
}
