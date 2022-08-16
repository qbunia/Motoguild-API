using System.ComponentModel.DataAnnotations;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Models.Group
{
    public class UpdateGroupDto
    {
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
    }
}
