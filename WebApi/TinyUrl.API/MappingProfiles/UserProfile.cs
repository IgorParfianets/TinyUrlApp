using AutoMapper;
using TinyUrl.API.Models.Request;
using TinyUrl.Core.DataTransferObjects;
using TinyUrl.Database.Entities;

namespace TinyUrl.API.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegistrationUserRequestModel, UserDto>();

            CreateMap<UserDto, User>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(model => Guid.NewGuid())); ;

            CreateMap<User, UserDto>();
        }
    }
}
