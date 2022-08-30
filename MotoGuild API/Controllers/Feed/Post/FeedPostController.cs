using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MotoGuild_API.Dto.PostDtos;
using MotoGuild_API.Repository.Interface;

namespace MotoGuild_API.Controllers
{
    [ApiController]
    [Route("api/feed/{feedId:int}/post")]
    [EnableCors("AllowAnyOrigin")]
    public class FeedPostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public FeedPostController(IPostRepository postRepositort, IMapper mapper)
        {
            _postRepository = postRepositort;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetPostsFeed(int feedId)
        {
            var posts = _postRepository.GetAllFeed(feedId);
            return Ok(_mapper.Map<List<PostDto>>(posts));
        }

        [HttpGet("{postId:int}", Name = "GetFeedPost")]
        public IActionResult GetPostFeed(int postId)
        {
            var post = _postRepository.Get(postId);
            if (post == null) return NotFound(); 
            return Ok(_mapper.Map<PostDto>(post));
        }

        [HttpPost]
        public IActionResult CreatePost(int feedId, [FromBody] CreatePostDto createPostDto)
        {
            var post = _mapper.Map<Post>(createPostDto);
            _postRepository.InsertToFeed(post, feedId);
            _postRepository.Save();
            var postDto = _mapper.Map<PostDto>(post);
            return CreatedAtRoute("GetFeedPost", new { id = postDto.Id }, postDto);
        }

        [HttpDelete("{postId:int}")]
        public IActionResult DeletePost(int postId)
        {
            var post = _postRepository.Get(postId);
            if (post == null) return NotFound();
            _postRepository.Delete(postId);
            _postRepository.Save();
            return Ok();
        }
    }
}
