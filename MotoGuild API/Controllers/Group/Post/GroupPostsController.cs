﻿using Data;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Models.Post;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Controllers;

[ApiController]
[Route("api/groups/{groupId:int}/posts")]
public class GroupPostsController : ControllerBase
{
    private readonly MotoGuildDbContext _db;

    public GroupPostsController(MotoGuildDbContext dbContext)
    {
        _db = dbContext;
    }

    [HttpGet]
    public IActionResult GetGroupPosts(int groupId)
    {
        var group = _db.Groups
            .Include(g => g.Posts)
            .FirstOrDefault(g => g.Id == groupId);
        if (group == null) return NotFound();

        var postsId = group.Posts.Select(p => p.Id);

        var posts = _db.Posts
            .Include(p => p.Author)
            .Where(p => postsId.Contains(p.Id)).ToList();

        if (posts == null) return NotFound();
        var postsDto = GetGroupPostsDtos(posts);
        return Ok(postsDto);
    }

    [HttpGet("{id:int}", Name = "GetGroupPost")]
    public IActionResult GetGroupPost(int groupId, int id)
    {
        var group = _db.Groups
            .Include(g => g.Posts)
            .FirstOrDefault(g => g.Id == groupId);
        if (group == null) return NotFound();
        var post = _db.Posts
            .Include(p => p.Author)
            .FirstOrDefault(p => p.Id == id);

        if (post == null) return NotFound();

        var postDto = GetGroupPostDto(post);
        return Ok(postDto);
    }

    [HttpPost]
    public IActionResult CreateGroupPost(int groupId, [FromBody] CreatePostDto createPostDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var group = _db.Groups.Include(g => g.Posts).FirstOrDefault(g => g.Id == groupId);
        if (group == null) return NotFound();
        var post = SaveGroupPostToDataBase(createPostDto, group);
        var postDto = GetGroupPostDto(post);
        return CreatedAtRoute("GetGroupPost", new {groupId, id = postDto.Id}, postDto);
    }

    private Post SaveGroupPostToDataBase(CreatePostDto createUserDto, Group group)
    {
        var author = _db.Users.FirstOrDefault(u => u.Id == createUserDto.Author.Id);
        var post = new Post
        {
            Author = author,
            Comments = new List<Comment>(),
            Content = createUserDto.Content,
            CreateTime = DateTime.Now
        };
        group.Posts.Add(post);
        _db.SaveChanges();
        return post;
    }


    [HttpDelete("{id:int}")]
    public IActionResult DeleteGroupPost(int groupId, int id)
    {
        var group = _db.Groups.Include(g => g.Posts).FirstOrDefault(u => u.Id == groupId);
        if (group == null) return NotFound();

        var post = _db.Posts.FirstOrDefault(p => p.Id == id);
        if (!group.Posts.Contains(post)) return NotFound();

        _db.Posts.Remove(post);
        _db.SaveChanges();
        return Ok();
    }


    private List<PostDto> GetGroupPostsDtos(List<Post> posts)
    {
        var postsDtos = new List<PostDto>();
        foreach (var post in posts)
        {
            var authorDto = new UserSelectedDataDto
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

    private PostDto GetGroupPostDto(Post post)
    {
        var authorDto = new UserSelectedDataDto
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