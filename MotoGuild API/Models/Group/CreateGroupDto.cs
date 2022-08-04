using System.ComponentModel.DataAnnotations;

namespace MotoGuild_API.Models.Group
{
    public class CreateGroupDto
    {
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
        [Required]
        public int OwnerId { get; set; }
        [Required]
        public bool IsPrivate { get; set; }

    }
}
