using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MotoGuild_API.Dto.CommentDtos;
using MotoGuild_API.Repository.Interface;

namespace MotoGuild_API.Controllers.Comment
{
    [ApiController]
    [Route("api/post/{postId:int}/comment")]

    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentsController(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetPostComments(int postId)
        {
            if (postId != 0)
            {
                var comments = _commentRepository.GetAll(postId);
                return Ok(_mapper.Map<List<CommentDto>>(comments));
            }
            else
            {
                var comments = _commentRepository.GetAll();
                return Ok(_mapper.Map<List<CommentDto>>(comments));
            }
        }

        [HttpGet("{commentId:int}", Name = "GetPostComment")]
        public IActionResult GetPostComment(int commentId)
        {
            var comment = _commentRepository.Get(commentId);
            return Ok(_mapper.Map<CommentDto>(comment));
        }

        [HttpPost]
        public IActionResult CreatePostComment([FromBody] CreateCommentDto createCommentDto, int postId)
        {
            createCommentDto.CreateTime = DateTime.Now;
            var comment = _mapper.Map<Domain.Comment>(createCommentDto);
            _commentRepository.Insert(comment, postId);
            _commentRepository.Save();
            var commentDto = _mapper.Map<CommentDto>(comment);
            return CreatedAtRoute("GetPostComment", new { commentId = commentDto.Id, postId = postId }, commentDto);

        }

        [HttpDelete("{commetId:int}")]
        public IActionResult DeletePostComment(int commetId)
        {
            var comment = _commentRepository.Get(commetId);
            if (comment == null) return NotFound();
            _commentRepository.Delete(commetId);
            _commentRepository.Save();
            return Ok();
        }
    }
}
