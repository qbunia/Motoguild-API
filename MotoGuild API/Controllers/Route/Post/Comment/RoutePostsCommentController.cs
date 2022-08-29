using Data;
using Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Models.Comment;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Controllers;

[ApiController]
[Route("api/routes/{routeId:int}/posts/{postId:int}/comments")]
[EnableCors("AllowAnyOrigin")]
public class RoutePostsCommentController : ControllerBase
{
    private readonly MotoGuildDbContext _db;

    public RoutePostsCommentController(MotoGuildDbContext dbContext)
    {
        _db = dbContext;
    }

    [HttpGet]
    public IActionResult GetRoutePostComments(int routeId, int postId)
    {
        var route = _db.Routes
            .Include(g => g.Posts)
            .FirstOrDefault(g => g.Id == routeId);
        if (route == null) return NotFound();

        var post = _db.Posts.Include(p => p.Comments).ThenInclude(c => c.Author).FirstOrDefault(p => p.Id == postId);

        if (post == null || !route.Posts.Contains(post)) return NotFound();

        var commentsId = post.Comments.Select(c => c.Id);

        var comments = _db.Comments.Where(c => commentsId.Contains(c.Id)).ToList();

        var commentsDto = GetRoutePostCommentsDtos(comments);
        return Ok(commentsDto);
    }

    [HttpGet("{id:int}", Name = "GetRoutePostComment")]
    public IActionResult GetRoutePostComment(int routeId, int postId, int id)
    {
        var route = _db.Routes
            .Include(g => g.Posts)
            .FirstOrDefault(g => g.Id == routeId);
        if (route == null) return NotFound();

        var post = _db.Posts.Include(p => p.Comments).ThenInclude(c => c.Author).FirstOrDefault(p => p.Id == postId);

        if (post == null || !route.Posts.Contains(post)) return NotFound();

        var comment = _db.Comments.FirstOrDefault(c => c.Id == id);

        if (comment == null) return NotFound();

        var commentDto = GetRoutePostCommentDto(comment);
        return Ok(commentDto);
    }

    [HttpPost]
    public IActionResult CreateRoutePostComment(int routeId, int postId, [FromBody] CreateCommentDto createCommentDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var route = _db.Routes
            .Include(g => g.Posts)
            .FirstOrDefault(g => g.Id == routeId);
        if (route == null) return NotFound();

        var post = _db.Posts.Include(p => p.Comments).FirstOrDefault(p => p.Id == postId);

        if (post == null || !route.Posts.Contains(post)) return NotFound();
        var comment = SaveRoutePostCommentToDataBase(createCommentDto, post);
        var commentDto = GetRoutePostCommentDto(comment);
        return CreatedAtRoute("GetRoutePostComment", new { routeId, postId, id = commentDto.Id }, commentDto);
    }

    private Comment SaveRoutePostCommentToDataBase(CreateCommentDto createCommentDto, Post post)
    {
        var author = _db.Users.FirstOrDefault(u => u.Id == createCommentDto.Author.Id);
        var comment = new Comment
        {
            Author = author,
            Content = createCommentDto.Content,
            CreateTime = DateTime.Now
        };
        post.Comments.Add(comment);
        _db.SaveChanges();
        return comment;
    }


    [HttpDelete("{id:int}")]
    public IActionResult DeleteGroupPostComment(int routeId, int postId, int id)
    {
        var route = _db.Routes
            .Include(g => g.Posts)
            .FirstOrDefault(g => g.Id == routeId);
        if (route == null) return NotFound();

        var post = _db.Posts.Include(p => p.Comments).ThenInclude(c => c.Author).FirstOrDefault(p => p.Id == postId);

        if (post == null || !route.Posts.Contains(post)) return NotFound();

        var comment = _db.Comments.FirstOrDefault(c => c.Id == id);

        if (comment == null) return NotFound();

        _db.Comments.Remove(comment);
        _db.SaveChanges();
        return Ok();
    }


    private List<CommentDto> GetRoutePostCommentsDtos(List<Comment> comments)
    {
        var commentsDtos = new List<CommentDto>();
        foreach (var comment in comments)
        {
            var authorDto = new UserDto
            {
                Email = comment.Author.Email,
                Id = comment.Author.Id,
                Rating = comment.Author.Rating,
                UserName = comment.Author.UserName
            };

            commentsDtos.Add(new CommentDto
            {
                Author = authorDto,
                Content = comment.Content,
                CreateTime = comment.CreateTime,
                Id = comment.Id
            });
        }

        return commentsDtos;
    }

    private CommentDto GetRoutePostCommentDto(Comment comment)
    {
        var authorDto = new UserDto
        {
            Email = comment.Author.Email,
            Id = comment.Author.Id,
            Rating = comment.Author.Rating,
            UserName = comment.Author.UserName
        };

        var commentDto = new CommentDto
        {
            Author = authorDto,
            Content = comment.Content,
            CreateTime = comment.CreateTime,
            Id = comment.Id
        };
        return commentDto;
    }
}