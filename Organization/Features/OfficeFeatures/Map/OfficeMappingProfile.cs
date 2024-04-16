using AutoMapper;
using Organization.Domain.Entity;
using Organization.Features.OfficeFeatures.DTO;

namespace Organization.Features.OfficeFeatures.Map
{
    public class OfficeMappingProfile : Profile
    {
        public OfficeMappingProfile()
        {

            CreateMap<Office, OfficeDto>();
            CreateMap<OfficeForCreationDto, Office>();
            CreateMap<Office, OfficeForCreationDto>();


        }
    }
}
