using MotoGuild_API.Models.Event;
using MotoGuild_API.Models.Group;
using MotoGuild_API.Models.Ride;
using MotoGuild_API.Models.Route;
using MotoGuild_API.Models.User;

namespace MotoGuild_API
{
    public class DataManager
    {
        public static DataManager Current { get; } = new DataManager();
        public List<UserDto> Users { get; set; }
        public List<UserSelectedDataDto> UsersWithSelectedData { get; set; }
        public List<GroupDto> Groups { get; set; }
        public List<EventDto> Events { get; set; }
        public List<EventSelectedDataDto> EventSelectedData { get; set; }
        public List<RideDto> Rides { get; set; }
        public List<RouteDto> Routes { get; set; }

        public DataManager()
        {
            Users = new List<UserDto>()
            {
                new UserDto(){Id = 1, UserName = "Xzibit",Email  = "xzibit@gmail.com", PhoneNumber = 123456789, Rating = 4.9}
            };
            UsersWithSelectedData = new List<UserSelectedDataDto>()
            {
                new UserSelectedDataDto(){Id = 1, UserName = "Xzibit", Email = "xzibit@gmail.com", Rating = 4.9}
            };
            Groups = new List<GroupDto>();
            Events = new List<EventDto>();
            EventSelectedData = new List<EventSelectedDataDto>();
            Routes = new List<RouteDto>();
            Rides = new List<RideDto>();
        }

    }
}
