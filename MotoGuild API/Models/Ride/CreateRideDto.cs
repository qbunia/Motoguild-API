using MotoGuild_API.Models.Post;
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
        public string StartPlace { get; set; }
        [Required]
        public string EndingPlace { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        public UserRideDto Owner { get; set; }
        public int MinimumRating { get; set; }
        public int Estimation { get; set; }

    }

    public class UserRideDto
    {
        public int Id { get; set; }
    }
}
