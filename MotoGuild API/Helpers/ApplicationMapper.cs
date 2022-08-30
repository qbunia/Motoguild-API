using AutoMapper;
using Domain;
using MotoGuild_API.Dto.CommentDtos;
using MotoGuild_API.Dto.FeedDtos;
using MotoGuild_API.Dto.GroupDtos;
using MotoGuild_API.Dto.PostDtos;
using MotoGuild_API.Dto.RideDtos;
using MotoGuild_API.Dto.RouteDtos;
using MotoGuild_API.Dto.StopDtos;
using MotoGuild_API.Dto.UserDtos;

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
            CreateMap<Stop, CreateStopDto>().ReverseMap();
            CreateMap<Stop, UpdateStopDto>().ReverseMap();
            CreateMap<Domain.Route, FullRouteDto>().ReverseMap();
            CreateMap<CreateRouteDto, Domain.Route>().ReverseMap();
            CreateMap<UpdateRouteDto, Domain.Route>().ReverseMap();
            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<FeedDto, Feed>().ReverseMap();

        }
    }
}
