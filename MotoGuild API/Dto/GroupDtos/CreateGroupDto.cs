using System.ComponentModel.DataAnnotations;
using MotoGuild_API.Dto.UserDtos;

namespace MotoGuild_API.Dto.GroupDtos;

public class CreateGroupDto
{
    [Required] public string Name { get; set; }

    [Required] public string Description { get; set; }

    [Required] public UserDto Owner { get; set; }

    [Required] public bool IsPrivate { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.Now;

    public List<UserDto> Participants { get; set; } = new();
    public double Rating { get; set; } = 0;
}