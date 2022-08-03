namespace MotoGuild_API.Models.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public double Rating { get; set; }
        public ICollection<GroupDto> Groups { get; set; } = new List<GroupDto>();
        public ICollection<EventDto> Events { get; set; } = new List<EventDto>();
        public ICollection<RideDto> Rides { get; set; } = new List<RideDto>();
        public ICollection<RouteDto> Routes { get; set; } = new List<RouteDto>();
    }

    public class UserSelectedDataDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public double Rating { get; set; }
    }
}
