using MotoGuild_API.Models.Post;
using MotoGuild_API.Models.Route;
using MotoGuild_API.Models.Stops;
using MotoGuild_API.Models.User;
using System.ComponentModel.DataAnnotations;

namespace MotoGuild_API.Models.Ride
{
    public class CreateRideDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public RouteDto Route { get; set; }
        public DateTime StartTime { get; set; }
        public UserDto Owner { get; set; }
        public int MinimumRating { get; set; }
        public int Estimation { get; set; }

        public ICollection<UserDto>? Participants { get; set; } = new List<UserDto>();

    }

}
