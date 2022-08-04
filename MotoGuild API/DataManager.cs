using MotoGuild_API.Models.Event;
using MotoGuild_API.Models.Group;
using MotoGuild_API.Models.Post;
using MotoGuild_API.Models.Ride;
using MotoGuild_API.Models.Route;
using MotoGuild_API.Models.User;

namespace MotoGuild_API
{
    public class DataManager
    {
        public static DataManager Current { get; } = new DataManager();
        public List<UserDto> Users { get; set; }
        public List<GroupDto> Groups { get; set; }
        public List<EventDto> Events { get; set; }
        public List<EventSelectedDataDto> EventSelectedData { get; set; }
        public List<RideDto> Rides { get; set; }
        public List<RouteDto> Routes { get; set; }

        private UserDto _firstUser = new UserDto()
            { Id = 1, UserName = "Xzibit", Email = "xzibit@gmail.com", PhoneNumber = 123456789, Rating = 4.9 };

        private UserSelectedDataDto owner = new UserSelectedDataDto()
        {
            Id = 2,
            UserName = "Grzegorz",
            Email = "grzegorz@gmail.com",
            Rating = 4.0
        };

        public DataManager()
        {
            Users = new List<UserDto>()
            {
               _firstUser
            };
            Groups = new List<GroupDto>()
            {
                new GroupDto()
                {
                    Id = 1, IsPrivate = true, Members = new List<UserSelectedDataDto>(), Name = "MC", Owner = GetFirstUserWithSelectedData(), 
                    CreationDate = DateTime.Now, PendingMembers = new List<UserSelectedDataDto>(), Posts = new List<PostDto>()
                }
            };
            Events = new List<EventDto>()
            {
                new EventDto() 
                {
                    Id = 1,
                    Name = "Dni Limanowej",
                    Owner = owner,
                    Description = "Koncer jubileuszowy powstania miasta",
                    Participants = new List<UserSelectedDataDto>(){ new UserSelectedDataDto {Id = 3, UserName = "Franek", Email = "franek@gmail.com", Rating = 3.0 } },
                    Place = "LDK",
                    Start = DateTime.Now,
                    Stop = DateTime.Now,
                    Posts = new List<PostDto>(){ new PostDto() { Author = owner, CreateTime = DateTime.Now, Content = "Napewno będzie świetna zabawz jak co roku :)"}}
                }
            };
            EventSelectedData = new List<EventSelectedDataDto>();
            Routes = new List<RouteDto>();
            Routes = new List<RouteDto>()
            {
                new RouteDto(){Id = 1, Name = "Szlakiem z pustym bakiem", Description = "Opis trasy", StartPlace = "Gdzieśtam", EndingPlace = "Gdzieśtam jeszcze dalej", Rating = 5},
                new RouteDto(){Id = 2, Name = "Bakiem pustym z szlakiem", Description = "Opis trasy", StartPlace = "2Gdzieśtam", EndingPlace = "2Gdzieśtam jeszcze dalej", Rating = 5}
            };
            
            Rides = new List<RideDto>();
        }

        private UserSelectedDataDto GetFirstUserWithSelectedData()
        {
            return new UserSelectedDataDto()
                { Email = _firstUser.Email, Rating = _firstUser.Rating, Id = _firstUser.Id, UserName = _firstUser.UserName };
        }

    }
}
