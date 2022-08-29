using MotoGuild_API.Models.Post;
using MotoGuild_API.Models.Stops;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Models.Route
{
    public class FullRouteDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StartPlace { get; set; }
        public string EndingPlace { get; set; }
        public int Rating { get; set; }
        public UserDto Owner { get; set; }
        public List<StopDto> Stops { get; set; }
        public List<PostDto> Posts { get; set; }
    }

    public class RouteDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StartPlace { get; set; }
        public string EndingPlace { get; set; }
        public int Rating { get; set; }
        public UserDto Owner { get; set; }
        public List<StopDto> Stops { get; set; }
    }

}
