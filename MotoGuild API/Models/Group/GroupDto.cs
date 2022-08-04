using MotoGuild_API.Models.Post;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Models.Group
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserSelectedDataDto Owner { get; set; }
        public ICollection<UserSelectedDataDto> Members { get; set; } = new List<UserSelectedDataDto>();
        public double Rating 
        {
            get
            {
                return (Members.Sum(x => x.Rating) + Owner.Rating)/(Members.Count+1);
            }
        }
        public ICollection<PostDto> Posts { get; set; } = new List<PostDto>();
        public bool IsPrivate { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<UserSelectedDataDto> PendingMembers { get; set; } = new List<UserSelectedDataDto>();
    }

    public class GroupSelectedDataDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserSelectedDataDto Owner { get; set; }
        public ICollection<UserSelectedDataDto> Members { get; set; } = new List<UserSelectedDataDto>();
        public double Rating
        {
            get
            {
                return (Members.Sum(x => x.Rating) + Owner.Rating) / (Members.Count + 1);
            }
        }
        public bool IsPrivate { get; set; }
    }
}
