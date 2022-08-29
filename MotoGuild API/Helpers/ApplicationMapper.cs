using AutoMapper;
using Domain;
using MotoGuild_API.Models.Comment;
using MotoGuild_API.Models.Group;
using MotoGuild_API.Models.Post;
using MotoGuild_API.Models.Ride;
using MotoGuild_API.Models.Route;
using MotoGuild_API.Models.Stops;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Group, SelectedGroupDto>().ReverseMap();
            CreateMap<CreateGroupDto, Group>().ReverseMap();
            CreateMap<UpdateGroupDto, Group>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Ride, RideDto>().ReverseMap();
            CreateMap<Domain.Route, RouteDto>().ReverseMap();
            CreateMap<Post, PostDto>().ReverseMap();
            CreateMap<CreateRideDto, Ride>().ReverseMap();
            CreateMap<CreateUserDto, User>().ReverseMap();
            CreateMap<UpdateUserDto, User>().ReverseMap();
            CreateMap<Post, PostDto>().ReverseMap();
            CreateMap<CreateRideDto, Ride>().ReverseMap();
            CreateMap<Stop, StopDto>().ReverseMap();
            CreateMap<Domain.Route, FullRouteDto>().ReverseMap();
            CreateMap<CreateRouteDto, Domain.Route>().ReverseMap();
            CreateMap<UpdateRouteDto, Domain.Route>().ReverseMap();

        }
    }
}
