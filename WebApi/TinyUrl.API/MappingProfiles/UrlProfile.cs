using AutoMapper;
using TinyUrl.API.Models.Request;
using TinyUrl.API.Models.Responce;
using TinyUrl.Core.DataTransferObjects;
using TinyUrl.Database.Entities;

namespace TinyUrl.API.MappingProfiles
{
    public class UrlProfile : Profile
    {
        public UrlProfile()
        {
            CreateMap<UrlResponceModel, UrlDto>()
                .ForMember(dto => dto.UrlCreated, opt => opt.MapFrom(model => DateTime.Now));

            CreateMap<UrlDto, Url>();

            CreateMap<Url, UrlDto>();
        }
    }
}
