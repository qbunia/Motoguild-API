using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using MotoGuild_API.Dto.PostDtos;
using MotoGuild_API.Repository.Interface;

namespace MotoGuild_API.Controllers;

[ApiController]
[Route("api/group/{groupId:int}/post")]
public class GroupPostsController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;

    public GroupPostsController(IPostRepository postRepositort, IMapper mapper)
    {
        _postRepository = postRepositort;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetGroupPosts(int groupId)
    {
        var posts = _postRepository.GetAllGroup(groupId);
        return Ok(_mapper.Map<List<PostDto>>(posts));
    }

    [HttpGet("{postId:int}", Name = "GetGroupPost")]
    public IActionResult GetGroupPost(int postId)
    {
        var post = _postRepository.Get(postId);
        if (post == null) return NotFound();
        return Ok(_mapper.Map<PostDto>(post));
    }

    [HttpPost]
    public IActionResult CreateGroupPost(int groupId, [FromBody] CreatePostDto createPostDto)
    {
        createPostDto.CreateTime = DateTime.Now;
        var post = _mapper.Map<Post>(createPostDto);
        _postRepository.InsertToGroup(post, groupId);
        _postRepository.Save();
        var postDto = _mapper.Map<PostDto>(post);
        return CreatedAtRoute("GetGroupPost", new { groupId= groupId, postId = postDto.Id }, postDto);
    }

    [HttpDelete("{postId:int}")]
    public IActionResult DeleteGroupPost(int postId)
    {
        var post = _postRepository.Get(postId);
        if (post == null) return NotFound();
        _postRepository.Delete(postId);
        _postRepository.Save();
        return Ok();
    }
}