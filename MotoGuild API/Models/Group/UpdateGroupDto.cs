using System.ComponentModel.DataAnnotations;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Models.Group
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
