using AutoMapper;
using RainfallAPI.Application.Response;

namespace RainfallAPI.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Item, RainfallReading>()
                .ForMember(dest => dest.DateMeasured, opt => opt.MapFrom(src => src.DateTime))
                .ForMember(dest => dest.AmountMeasured, opt => opt.MapFrom(src => (decimal)src.Value));
        }
    }
}
