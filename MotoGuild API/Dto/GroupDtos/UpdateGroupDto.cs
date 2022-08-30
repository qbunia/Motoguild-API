using MotoGuild_API.Dto.UserDtos;
using System.ComponentModel.DataAnnotations;

namespace MotoGuild_API.Dto.GroupDtos
{
    public class UpdateGroupDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public UserDto Owner { get; set; }

        [Required]
        public bool IsPrivate { get; set; }
    }
}
