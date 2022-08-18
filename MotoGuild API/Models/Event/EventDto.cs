using MotoGuild_API.Models.Post;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Models.Event
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public UserDto Owner { get; set; }
        public string Description { get; set; } = String.Empty;
        public ICollection<UserDto>? Participants { get; set; } = new List<UserDto>();
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
