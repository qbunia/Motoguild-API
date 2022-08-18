using MotoGuild_API.Models.User;

namespace MotoGuild_API.Models.Comment
{
    public class CreateCommentDto
    {
        public UserCommentDto Author { get; set; }
        public string Content { get; set; }
    }

    public class UserCommentDto
    {
        public int Id { get; set; }
    }
}
