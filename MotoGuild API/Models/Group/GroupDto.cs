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

    public class GroupSelectedDataDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserSelectedDataDto Owner { get; set; }

        public bool IsPrivate { get; set; }

        public DateTime CreationDate { get; set; }

        public List<UserSelectedDataDto> Participants { get; set; }
    }
}
