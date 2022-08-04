using MotoGuild_API.Models.Post;
using MotoGuild_API.Models.User;
using MotoGuild_API.Models.Stops;

namespace MotoGuild_API.Models.Ride
{
    public class RideDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserSelectedDataDto Owner { get; set; }
        public string Description { get; set; } = String.Empty;
        public ICollection<UserSelectedDataDto>? Participants { get; set; } = new List<UserSelectedDataDto>();
        public string StartPlace { get; set; }
        public string EndingPlace { get; set; }
        public ICollection<StopsDto>? Stops { get; set; }
        public DateTime StartTime { get; set; }
        public int Estimation { get; set; }
        public ICollection<PostDto> Posts { get; set; } = new List<PostDto>();
        public int MinimumRating { get; set; }
        
    }
}
