using AutoMapper;
using Domain;
using MotoGuild_API.Models.Group;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Group, SelectedGroupDto>();
            CreateMap<CreateGroupDto, Group>();
            CreateMap<UpdateGroupDto, Group>();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
