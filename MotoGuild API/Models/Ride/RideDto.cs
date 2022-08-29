using MotoGuild_API.Models.Post;
using MotoGuild_API.Models.User;
using MotoGuild_API.Models.Stops;
using MotoGuild_API.Models.Route;

namespace MotoGuild_API.Models.Ride
{
    public class RideDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = String.Empty;
        public ICollection<UserDto>? Participants { get; set; } = new List<UserDto>();
        public RouteDto Route { get; set; }
        public DateTime StartTime { get; set; }
        public int Estimation { get; set; }
        public ICollection<PostDto> Posts { get; set; } = new List<PostDto>();
        public int MinimumRating { get; set; }
        public string Owner { get; set; }

    }
}
