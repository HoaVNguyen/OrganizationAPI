using AutoMapper;
using Organization.Domain.Entity;

namespace Organization.Features.ParishFeatures
{
    public class ParishMappingProfile : Profile
    {
        public ParishMappingProfile()
        {
            CreateMap<Parish, ParishDto>();
            CreateMap<ParishForCreationDto, Parish>();
            CreateMap<Parish, ParishForCreationDto>();


        }
    }
}
