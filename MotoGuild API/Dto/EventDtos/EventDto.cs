
using MotoGuild_API.Dto.PostDtos;
using MotoGuild_API.Dto.UserDtos;

namespace MotoGuild_API.Dto.EventDtos
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public UserDtos.UserDto Owner { get; set; }
        public string Description { get; set; } = String.Empty;
        public ICollection<UserDtos.UserDto>? Participants { get; set; } = new List<UserDtos.UserDto>();
        public string Place { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public DateTime Stop{ get; set; }
        public ICollection<PostDto> Posts { get; set; } = new List<PostDto>();
    }
    public class EventSelectedDataDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

    }
}
