
namespace MotoGuild_API.Dto.CommentDtos
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
