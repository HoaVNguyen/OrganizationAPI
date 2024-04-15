using AutoMapper;

using Organization.Domain.Entity;
using Organization.Features.Addition.DTO;

namespace Organization.Features.Addition.Map
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDto>().PreserveReferences();
            CreateMap<EmployeeForCreationDto, Employee>();
            CreateMap<Employee, EmployeeForCreationDto>();

            CreateMap<Office, OfficeDto>();
            CreateMap<OfficeForCreationDto, Office>();
            CreateMap<Office, OfficeForCreationDto>();

            CreateMap<Parish, ParishDto>();
            CreateMap<ParishForCreationDto, Parish>();
            CreateMap<Parish, ParishForCreationDto>();

            CreateMap<Team, TeamDto>().PreserveReferences();
            CreateMap<TeamForCreationDto, Team>();
            CreateMap<Team, TeamForCreationDto>();

        }
    }
}
