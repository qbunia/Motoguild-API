using MotoGuild_API.Models.Post;
using MotoGuild_API.Models.Stops;
using MotoGuild_API.Models.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MotoGuild_API.Models.Ride
{
    public class UpdateRideDto
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
        
        public int OwnerId { get; set; }
        public List<UserSelectedDataDto> Participants { get; set; }
        public List<PostDto> Posts { get; set; }

        public ICollection<StopDto>? Stops { get; set; }




    }
}
