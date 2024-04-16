using AutoMapper;

using Organization.Domain.Entity;
using Organization.Features.Addition.DTO;

namespace Organization.Features.Addition.Map
{
    public class TeamMappingProfile : Profile
    {
        public TeamMappingProfile()
        {
            CreateMap<Team, TeamDto>().PreserveReferences();
            CreateMap<TeamForCreationDto, Team>();
            CreateMap<Team, TeamForCreationDto>();

        }
    }
}
