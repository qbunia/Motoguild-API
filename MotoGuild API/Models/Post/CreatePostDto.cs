using MotoGuild_API.Models.Comment;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Models.Post
{
    public class CreatePostDto
    {
        public UserPostDto Author { get; set; }
        public string Content { get; set; }
    }

    public class UserPostDto
    {
        public int Id { get; set; }
    }
}
