using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserToReturnDto>();
            CreateMap<Friend, FriendToReturnDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.UserId))
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.User.FirstName))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.User.LastName))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.User.Email));
            CreateMap<Message, MessageToReturnDto>();
        }
    }
}