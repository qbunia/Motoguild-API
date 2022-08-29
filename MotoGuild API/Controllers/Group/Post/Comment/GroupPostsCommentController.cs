using AutoMapper;
using Data;
using Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Models.Comment;
using MotoGuild_API.Models.User;
using MotoGuild_API.Repository.Interface;
using System.Xml.Linq;

namespace MotoGuild_API.Controllers;

[ApiController]
[Route("api/groups/{groupId:int}/posts/{postId:int}/comments")]
[EnableCors("AllowAnyOrigin")]
public class GroupPostsCommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;

    public GroupPostsCommentController(ICommentRepository commentRepository, IMapper mapper)
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetGroupPostComments(int postId)
    {
        var comments = _commentRepository.GetAll(postId);
        return Ok(_mapper.Map<List<CommentDto>>(comments));
    }

    [HttpGet("{id:int}", Name = "GetGroupPostComment")]
    public IActionResult GetGroupPostComment(int postId)
    {
        var comment = _commentRepository.Get(postId);
        return Ok(_mapper.Map<List<CommentDto>>(comment));
    }

    [HttpPost]
    public IActionResult CreateGroupPostComment(int groupId, int postId, [FromBody] CreateCommentDto createCommentDto)
    {
         var comment = _mapper.Map<Comment>(createCommentDto);
        _commentRepository.Insert(comment);
        _commentRepository.Save();
        var commentDto = _mapper.Map<CommentDto>(comment);
        return CreatedAtRoute("GetComment", new { id = commentDto.Id }, commentDto);

    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteGroupPostComment(int commetId)
    {
        var comment = _commentRepository.Get(commetId);
        if (comment == null) return NotFound();
        _commentRepository.Delete(commetId);
        _commentRepository.Save();
        return Ok();
    }
}