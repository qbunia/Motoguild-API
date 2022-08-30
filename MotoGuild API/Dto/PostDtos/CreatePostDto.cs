namespace MotoGuild_API.Dto.PostDtos
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
