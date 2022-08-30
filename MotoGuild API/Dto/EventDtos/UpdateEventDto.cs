using MotoGuild_API.Dto.PostDtos;
using MotoGuild_API.Dto.UserDtos;
using System.ComponentModel.DataAnnotations;

namespace MotoGuild_API.Dto.EventDtos
{
    public class UpdateEventDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; } = String.Empty;
        public ICollection<UserDto>? Participants { get; set; } = new List<UserDto>();
        [Required]
        public string Place { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Start { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Stop { get; set; }
        public ICollection<PostDto> Posts { get; set; } = new List<PostDto>();
    }
}
