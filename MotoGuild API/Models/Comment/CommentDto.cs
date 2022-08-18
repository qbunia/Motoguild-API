using MotoGuild_API.Models.User;

namespace MotoGuild_API.Models.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }
        public UserDto Author { get; set; }
        public DateTime CreateTime { get; set; }
        public string Content { get; set; }
    }

    
}
