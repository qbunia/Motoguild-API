using System.ComponentModel.DataAnnotations;

namespace MotoGuild_API.Models.Group
{
    public class GroupAddMemberDto
    {
        [Required]
        public int MemberId { get; set; }
    }
}
