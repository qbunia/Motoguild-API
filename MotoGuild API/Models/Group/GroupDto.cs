using MotoGuild_API.Models.Post;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Models.Group
{
    public class GroupParticipantsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserSelectedDataDto Owner { get; set; }

        public bool IsPrivate { get; set; }

        public DateTime CreationDate { get; set; }
    }

    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserSelectedDataDto Owner { get; set; }

        public bool IsPrivate { get; set; }

        public DateTime CreationDate { get; set; }

        public List<UserSelectedDataDto> Participants { get; set; }
        public List<UserSelectedDataDto> PendingUsers { get; set; }
        public List<PostDto> Posts { get; set; }
    }

    public class SelectedGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserSelectedDataDto Owner { get; set; }

        public bool IsPrivate { get; set; }

        public DateTime CreationDate { get; set; }

        public List<UserSelectedDataDto> Participants { get; set; }
        public double Rating { get; set; }
    }
}
