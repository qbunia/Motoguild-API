using MotoGuild_API.Models.Event;
using MotoGuild_API.Models.Group;
using MotoGuild_API.Models.Ride;
using MotoGuild_API.Models.Route;

namespace MotoGuild_API.Models.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int? PhoneNumber { get; set; }
        public double Rating { get; set; }

    }

    public class UserSelectedDataDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public double Rating { get; set; }
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            var other = obj as UserSelectedDataDto;
            return this.Id == other.Id;
        }
    }
}
