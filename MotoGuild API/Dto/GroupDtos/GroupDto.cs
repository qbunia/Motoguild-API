
using MotoGuild_API.Dto.PostDtos;
using MotoGuild_API.Dto.UserDtos;

namespace MotoGuild_API.Dto.GroupDtos
{ 
    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserDtos.UserDto Owner { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime CreationDate { get; set; }
        public List<UserDtos.UserDto> Participants { get; set; }
        public List<UserDtos.UserDto> PendingUsers { get; set; }
        public List<PostDto> Posts { get; set; }
        public double Rating { get; set; }
    }

    public class SelectedGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserDtos.UserDto Owner { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime CreationDate { get; set; }
        public List<UserDtos.UserDto> Participants { get; set; }
        public double Rating { get; set; }
    }
}
