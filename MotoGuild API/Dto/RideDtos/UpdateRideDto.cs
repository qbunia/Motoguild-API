
using MotoGuild_API.Dto.PostDtos;
using MotoGuild_API.Dto.StopDtos;
using MotoGuild_API.Dto.UserDtos;
using System.ComponentModel.DataAnnotations;


namespace MotoGuild_API.Dto.RideDtos
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
        
        public UserDtos.UserDto Owner { get; set; }
        public ICollection<UserDtos.UserDto> Participants { get; set; } = new List<UserDtos.UserDto>();
        public ICollection<PostDto>? Posts { get; set; } = new List<PostDto>();

        public ICollection<StopDto>? Stops { get; set; } = new List<StopDto>();

        public int MinimumRating { get; set; }
        public int Estimation { get; set; }

    }
}
