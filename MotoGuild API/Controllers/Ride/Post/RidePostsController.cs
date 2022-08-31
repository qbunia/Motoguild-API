using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MotoGuild_API.Dto.PostDtos;
using MotoGuild_API.Repository.Interface;

namespace MotoGuild_API.Controllers.Ride.Post
{
    [ApiController]
    [Route("api/ride/{rideId:int}/post")]
    public class RidePostsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        public RidePostsController(IPostRepository postRepositort, IMapper mapper)
        {
            _postRepository = postRepositort;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetRidePosts(int rideId)
        {
            var posts = _postRepository.GetAllRide(rideId);
            return Ok(_mapper.Map<List<PostDto>>(posts));
        }

        [HttpGet("{postId:int}", Name = "GetRidePost")]
        public IActionResult GetRidePost(int postId)
        {
            var post = _postRepository.Get(postId);
            return Ok(_mapper.Map<List<PostDto>>(post));
        }

        [HttpPost]
        public IActionResult CreateRidePost(int rideId, [FromBody] CreatePostDto createPostDto)
        {
            var post = _mapper.Map<Domain.Post>(createPostDto);
            _postRepository.InsertToRide(post, rideId);
            _postRepository.Save();
            var postDto = _mapper.Map<PostDto>(post);
            return CreatedAtRoute("GetRidePost", new { id = postDto.Id }, postDto);
        }

        [HttpDelete("{postId:int}")]
        public IActionResult DeleteRidePost(int postId)
        {
            var post = _postRepository.Get(postId);
            if (post == null) return NotFound();
            _postRepository.Delete(postId);
            _postRepository.Save();
            return Ok();
        }

    }
}
