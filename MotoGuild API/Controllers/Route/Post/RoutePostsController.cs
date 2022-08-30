using Data;
using Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Dto.PostDtos;
using MotoGuild_API.Dto.UserDtos;

namespace MotoGuild_API.Controllers;

[ApiController]
[Route("api/routes/{routeId:int}/posts")]
[EnableCors("AllowAnyOrigin")]
public class RoutePostsController : ControllerBase
{
    private readonly MotoGuildDbContext _db;

    public RoutePostsController(MotoGuildDbContext dbContext)
    {
        _db = dbContext;
    }

    [HttpGet]
    public IActionResult GetRoutePosts(int routeId)
    {
        var route = _db.Routes
            .Include(g => g.Posts)
            .FirstOrDefault(g => g.Id == routeId);
        if (route == null) return NotFound();

        var postsId = route.Posts.Select(p => p.Id);

        var posts = _db.Posts
            .Include(p => p.Author)
            .Where(p => postsId.Contains(p.Id)).ToList();

        if (posts == null) return NotFound();
        var postsDto = GetRoutePostsDtos(posts);
        return Ok(postsDto);
    }

    [HttpGet("{id:int}", Name = "GetRoutePost")]
    public IActionResult GetRoutePost(int routeId, int id)
    {
        var route = _db.Routes
            .Include(g => g.Posts)
            .FirstOrDefault(g => g.Id == routeId);
        if (route == null) return NotFound();
        var post = _db.Posts
            .Include(p => p.Author)
            .FirstOrDefault(p => p.Id == id);

        if (post == null) return NotFound();

        var postDto = GetRoutePostDto(post);
        return Ok(postDto);
    }

    [HttpPost]
    public IActionResult CreateGroupPost(int routeId, [FromBody] CreatePostDto createPostDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var route = _db.Routes.Include(g => g.Posts).FirstOrDefault(g => g.Id == routeId);
        if (route == null) return NotFound();
        var post = SaveRoutePostToDataBase(createPostDto, route);
        var postDto = GetRoutePostDto(post);
        return CreatedAtRoute("GetRoutePost", new { routeId, id = postDto.Id }, postDto);
    }

    private Post SaveRoutePostToDataBase(CreatePostDto createUserDto, Domain.Route route)
    {
        var author = _db.Users.FirstOrDefault(u => u.Id == createUserDto.Author.Id);
        var post = new Post
        {
            Author = author,
            Comments = new List<Domain.Comment>(),
            Content = createUserDto.Content,
            CreateTime = DateTime.Now
        };
        route.Posts.Add(post);
        _db.SaveChanges();
        return post;
    }


    [HttpDelete("{id:int}")]
    public IActionResult DeleteGroupPost(int routeId, int id)
    {
        var route = _db.Routes.Include(g => g.Posts).FirstOrDefault(u => u.Id == routeId);
        if (route == null) return NotFound();

        var post = _db.Posts.FirstOrDefault(p => p.Id == id);
        if (!route.Posts.Contains(post)) return NotFound();

        _db.Posts.Remove(post);
        _db.SaveChanges();
        return Ok();
    }


    private List<PostDto> GetRoutePostsDtos(List<Post> posts)
    {
        var postsDtos = new List<PostDto>();
        foreach (var post in posts)
        {
            var authorDto = new UserDto
            {
                Email = post.Author.Email,
                Id = post.Author.Id,
                Rating = post.Author.Rating,
                UserName = post.Author.UserName
            };

            postsDtos.Add(new PostDto
            {
                Author = authorDto,
                Content = post.Content,
                CreateTime = post.CreateTime,
                Id = post.Id
            });
        }

        return postsDtos;
    }

    private PostDto GetRoutePostDto(Post post)
    {
        var authorDto = new UserDto
        {
            Email = post.Author.Email,
            Id = post.Author.Id,
            Rating = post.Author.Rating,
            UserName = post.Author.UserName
        };

        var postDto = new PostDto
        {
            Author = authorDto,
            Content = post.Content,
            CreateTime = post.CreateTime,
            Id = post.Id
        };
        return postDto;
    }
}