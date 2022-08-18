using Data;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Models.Comment;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Controllers;

[ApiController]
[Route("api/groups/{groupId:int}/posts/{postId:int}/comments")]
public class GroupPostsCommentController : ControllerBase
{
    private readonly MotoGuildDbContext _db;

    public GroupPostsCommentController(MotoGuildDbContext dbContext)
    {
        _db = dbContext;
    }

    [HttpGet]
    public IActionResult GetGroupPostComments(int groupId, int postId)
    {
        var group = _db.Groups
            .Include(g => g.Posts)
            .FirstOrDefault(g => g.Id == groupId);
        if (group == null) return NotFound();

        var post = _db.Posts.Include(p => p.Comments).ThenInclude(c => c.Author).FirstOrDefault(p => p.Id == postId);

        if (post == null || !group.Posts.Contains(post)) return NotFound();

        var commentsId = post.Comments.Select(c => c.Id);

        var comments = _db.Comments.Where(c => commentsId.Contains(c.Id)).ToList();

        var commentsDto = GetGroupPostCommentsDtos(comments);
        return Ok(commentsDto);
    }

    [HttpGet("{id:int}", Name = "GetGroupPostComment")]
    public IActionResult GetGroupPostComment(int groupId, int postId, int id)
    {
        var group = _db.Groups
            .Include(g => g.Posts)
            .FirstOrDefault(g => g.Id == groupId);
        if (group == null) return NotFound();

        var post = _db.Posts.Include(p => p.Comments).ThenInclude(c => c.Author).FirstOrDefault(p => p.Id == postId);

        if (post == null || !group.Posts.Contains(post)) return NotFound();

        var comment = _db.Comments.FirstOrDefault(c => c.Id == id);

        if (comment == null) return NotFound();

        var commentDto = GetGroupPostCommentDto(comment);
        return Ok(commentDto);
    }

    [HttpPost]
    public IActionResult CreateGroupPostComment(int groupId, int postId, [FromBody] CreateCommentDto createCommentDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var group = _db.Groups
            .Include(g => g.Posts)
            .FirstOrDefault(g => g.Id == groupId);
        if (group == null) return NotFound();

        var post = _db.Posts.Include(p => p.Comments).FirstOrDefault(p => p.Id == postId);

        if (post == null || !group.Posts.Contains(post)) return NotFound();
        var comment = SaveGroupPostCommentToDataBase(createCommentDto, post);
        var commentDto = GetGroupPostCommentDto(comment);
        return CreatedAtRoute("GetGroupPostComment", new {groupId, postId, id = commentDto.Id}, commentDto);
    }

    private Comment SaveGroupPostCommentToDataBase(CreateCommentDto createCommentDto, Post post)
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
    public IActionResult DeleteGroupPostComment(int groupId, int postId, int id)
    {
        var group = _db.Groups
            .Include(g => g.Posts)
            .FirstOrDefault(g => g.Id == groupId);
        if (group == null) return NotFound();

        var post = _db.Posts.Include(p => p.Comments).ThenInclude(c => c.Author).FirstOrDefault(p => p.Id == postId);

        if (post == null || !group.Posts.Contains(post)) return NotFound();

        var comment = _db.Comments.FirstOrDefault(c => c.Id == id);

        if (comment == null) return NotFound();

        _db.Comments.Remove(comment);
        _db.SaveChanges();
        return Ok();
    }


    private List<CommentDto> GetGroupPostCommentsDtos(List<Comment> comments)
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

    private CommentDto GetGroupPostCommentDto(Comment comment)
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