using AutoMapper;
using RainfallAPI.Application.Models;
using RainfallAPI.Domain.Entities;

namespace RainfallAPI.Application.Mappers
{
    /// <summary>
    /// Responsible for defining mapping profiles for AutoMapper.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// </summary>
        public MappingProfile()
        {
            // Maps Item to RainfallReading
            CreateMap<Item, RainfallReading>()
                .ForMember(dest => dest.DateMeasured, opt => opt.MapFrom(src => src.DateTime))
                .ForMember(dest => dest.AmountMeasured, opt => opt.MapFrom(src => (decimal)src.Value));
        }
    }
}
