using Data;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Models.Comment;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Controllers;

[ApiController]
[Route("api/feeds/{feedId:int}/posts/{postId:int}/comments")]
public class FeedPostsCommentController : ControllerBase
{
    private readonly MotoGuildDbContext _db;

    public FeedPostsCommentController(MotoGuildDbContext dbContext)
    {
        _db = dbContext;
    }

    [HttpGet]
    public IActionResult GetFeedPostComments(int feedId, int postId)
    {
        var feed = _db.Feed
            .Include(g => g.Posts)
            .FirstOrDefault(g => g.Id == feedId);
        if (feed == null) return NotFound();

        var post = _db.Posts.Include(p => p.Comments).ThenInclude(c => c.Author).FirstOrDefault(p => p.Id == postId);

        if (post == null || !feed.Posts.Contains(post)) return NotFound();

        var commentsId = post.Comments.Select(c => c.Id);

        var comments = _db.Comments.Where(c => commentsId.Contains(c.Id)).ToList();

        var commentsDto = GetFeedPostCommentsDtos(comments);
        return Ok(commentsDto);
    }

    [HttpGet("{id:int}", Name = "GetFeedPostComment")]
    public IActionResult GetFeedPostComment(int feedId, int postId, int id)
    {
        var feed = _db.Feed
            .Include(g => g.Posts)
            .FirstOrDefault(g => g.Id == feedId);
        if (feed == null) return NotFound();

        var post = _db.Posts.Include(p => p.Comments).ThenInclude(c => c.Author).FirstOrDefault(p => p.Id == postId);

        if (post == null || !feed.Posts.Contains(post)) return NotFound();

        var comment = _db.Comments.FirstOrDefault(c => c.Id == id);

        if (comment == null) return NotFound();

        var commentDto = GetFeedPostCommentDto(comment);
        return Ok(commentDto);
    }

    [HttpPost]
    public IActionResult CreateFeedPostComment(int feedId, int postId, [FromBody] CreateCommentDto createCommentDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var feed = _db.Feed
            .Include(g => g.Posts)
            .FirstOrDefault(g => g.Id == feedId);
        if (feed == null) return NotFound();

        var post = _db.Posts.Include(p => p.Comments).FirstOrDefault(p => p.Id == postId);

        if (post == null || !feed.Posts.Contains(post)) return NotFound();
        var comment = SaveFeedPostCommentToDataBase(createCommentDto, post);
        var commentDto = GetFeedPostCommentDto(comment);
        return CreatedAtRoute("GetFeedPostComment", new { feedId, postId, id = commentDto.Id }, commentDto);
    }

    private Comment SaveFeedPostCommentToDataBase(CreateCommentDto createCommentDto, Post post)
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
    public IActionResult DeleteFeedPostComment(int feedId, int postId, int id)
    {
        var route = _db.Feed
            .Include(g => g.Posts)
            .FirstOrDefault(g => g.Id == feedId);
        if (route == null) return NotFound();

        var post = _db.Posts.Include(p => p.Comments).ThenInclude(c => c.Author).FirstOrDefault(p => p.Id == postId);

        if (post == null || !route.Posts.Contains(post)) return NotFound();

        var comment = _db.Comments.FirstOrDefault(c => c.Id == id);

        if (comment == null) return NotFound();

        _db.Comments.Remove(comment);
        _db.SaveChanges();
        return Ok();
    }


    private List<CommentDto> GetFeedPostCommentsDtos(List<Comment> comments)
    {
        var commentsDtos = new List<CommentDto>();
        foreach (var comment in comments)
        {
            var authorDto = new UserSelectedDataDto
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

    private CommentDto GetFeedPostCommentDto(Comment comment)
    {
        var authorDto = new UserSelectedDataDto
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