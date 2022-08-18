using MotoGuild_API.Models.Comment;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Models.Post
{
    public class PostDto
    {
        public int Id { get; set; }
        public UserDto Author { get; set; }
        public DateTime CreateTime { get; set; }
        public string Content { get; set; }
    }

}
