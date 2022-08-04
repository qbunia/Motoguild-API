using MotoGuild_API.Models.Post;
using MotoGuild_API,Models.Stops;
namespace MotoGuild_API.Models.Route
{
    public class RouteDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<StopsDto>? Stops { get; set; }
        public string StartPlace { get; set; }
        public string EndingPlace { get; set; }
        public ICollection<PostDto> Posts { get; set; } = new List<PostDto>();
        public int Rating { get; set; }
    }
}
