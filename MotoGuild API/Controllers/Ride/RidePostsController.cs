//using System.Net;
//using System.Runtime.ExceptionServices;
//using Data;
//using Domain;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using MotoGuild_API.Models.Comment;
//using MotoGuild_API.Models.Group;
//using MotoGuild_API.Models.Post;
//using MotoGuild_API.Models.User;

//namespace MotoGuild_API.Controllers;


//[ApiController]
//[Route("api/groups/{groupId:int}/posts")]
//public class RidePostsController
//{
//    private MotoGuildDbContext _db;

//    public RidePostsController(MotoGuildDbContext dbContext)
//    {
//        _db = dbContext;
//    }

//    [HttpGet]
//    public IActionResult GetRidePosts(int rideId)
//    {
//        var ride = _db.Rides
//            .Include(r => r.Posts)
//            .FirstOrDefault(r => r.Id == rideId);
//        if (ride == null)
//        {
//            return NotFound();
//        }

//        var postsId = ride.Posts.Select(p => p.Id);

//        var posts = _db.Posts
//            .Include(p => p.Author)
//            .Include(p => p.Comments)
//            .Where(p => postsId.Contains(p.Id)).ToList();
//        if (posts == null)
//        {
//            return NotFound();
//        }
//        var postsDto = GetPostsDtos(posts);
//        return Ok(postsDto);
//    }

//    [HttpGet("{id:int}", Name = "GetRidePost")]
//    public IActionResult GetRidePost(int groupId, int id)
//    {
//        var group = _db.Groups
//            .Include(g => g.Posts)
//            .FirstOrDefault(g => g.Id == groupId);
//        if (group == null)
//        {
//            return NotFound();
//        }

//        var postsId = group.Posts.Select(p => p.Id);

//        var posts = _db.Posts
//            .Include(p => p.Author)
//            .Include(p => p.Comments)
//            .Where(p => postsId.Contains(p.Id)).ToList();
//        if (posts == null)
//        {
//            return NotFound();
//        }
//        var postsDto = GetPostDtos(post);
//        return Ok(postDto);
//    }

//    [HttpPost]
//    public IActionResult CreateRidePost(int groupId, [FromBody] CreatePostDto postDto)
//    {
        
//        if (!ModelState.IsValid)
//        {
//            return BadRequest(ModelState);
//        }
//        var group = _db.Groups.FirstOrDefault(g => g.Id == groupId);
//        if (group == null)
//        {
//            return NotFound();
//        }
//        var post = new Post
//        {
//            Author = _db.Users.FirstOrDefault(u => u.Id == postDto.AuthorId),
//            Content = postDto.Content,
//            CreatedAt = DateTime.Now,
//            Group = group
//        };
//        _db.Posts.Add(post);
//        _db.SaveChanges();
//        var postDto = GetPostDtos(post);
//        return CreatedAtRoute("GetRidePost", new { groupId = groupId, id = post.Id }, postDto);
//    }


//}