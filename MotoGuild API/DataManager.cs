﻿using MotoGuild_API.Models.Event;
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
        public List<GroupDto> Groups { get; set; }
        public List<EventDto> Events { get; set; }
        public List<RideDto> Rides { get; set; }
        public List<RouteDto> Routes { get; set; }

        public DataManager()
        {
            Users = new List<UserDto>()
            {
                new UserDto(){Id = 1, UserName = "Xzibit",Email  = "xzibit@gmail.com", PhoneNumber = 123456789, Rating = 4.9}
            };
            Groups = new List<GroupDto>();
            Events = new List<EventDto>();
            Routes = new List<RouteDto>()
            {
                new RouteDto(){Id = 1, Name = "Szlakiem z pustym bakiem", Description = "Opis trasy", StartPlace = "Gdzieśtam", EndingPlace = "Gdzieśtam jeszcze dalej", Rating = 5}
            };
            
            Rides = new List<RideDto>();
        }

    }
}
