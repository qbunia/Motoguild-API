using AutoMapper;
using Domain;
using MotoGuild_API.Dto.CommentDtos;
using MotoGuild_API.Dto.EventDtos;
using MotoGuild_API.Dto.FeedDtos;
using MotoGuild_API.Dto.GroupDtos;
using MotoGuild_API.Dto.PostDtos;
using MotoGuild_API.Dto.RideDtos;
using MotoGuild_API.Dto.RouteDtos;
using MotoGuild_API.Dto.StopDtos;
using MotoGuild_API.Dto.UserDtos;
using Route = Domain.Route;

namespace MotoGuild_API.Helpers;

public class ApplicationMapper : Profile
{
    public ApplicationMapper()
    {
        CreateMap<Group, SelectedGroupDto>().ReverseMap();
        CreateMap<CreateGroupDto, Group>().ReverseMap();
        CreateMap<UpdateGroupDto, Group>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<Ride, RideDto>().ReverseMap();
        CreateMap<Route, RouteDto>().ReverseMap();
        CreateMap<Post, PostDto>().ReverseMap();
        CreateMap<Post, CreatePostDto>().ReverseMap();
        CreateMap<CreateRideDto, Ride>().ReverseMap();
        CreateMap<CreateUserDto, User>().ReverseMap();
        CreateMap<UpdateUserDto, User>().ReverseMap();
        CreateMap<CreateRideDto, Ride>().ReverseMap();
        CreateMap<Stop, StopDto>().ReverseMap();
        CreateMap<Stop, CreateStopDto>().ReverseMap();
        CreateMap<Stop, UpdateStopDto>().ReverseMap();
        CreateMap<Route, FullRouteDto>().ReverseMap();
        CreateMap<CreateRouteDto, Route>().ReverseMap();
        CreateMap<UpdateRouteDto, Route>().ReverseMap();
        CreateMap<CreateEventDto, Event>().ReverseMap();
        CreateMap<Event, EventDto>().ReverseMap();
        CreateMap<UpdateEventDto, Event>().ReverseMap();

        CreateMap<Comment, CommentDto>().ReverseMap();
        CreateMap<Comment, CreateCommentDto>().ReverseMap();
        CreateMap<FeedDto, Feed>().ReverseMap();
    }
}