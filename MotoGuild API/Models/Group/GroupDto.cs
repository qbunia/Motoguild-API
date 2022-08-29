using MotoGuild_API.Models.Post;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Models.Group
{

    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserDto Owner { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime CreationDate { get; set; }
        public List<UserDto> Participants { get; set; }
        public List<UserDto> PendingUsers { get; set; }
        public List<PostDto> Posts { get; set; }
        public double Rating { get; set; }
    }

    public class SelectedGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserDto Owner { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime CreationDate { get; set; }
        public List<UserDto> Participants { get; set; }
        public double Rating { get; set; }
    }
}
