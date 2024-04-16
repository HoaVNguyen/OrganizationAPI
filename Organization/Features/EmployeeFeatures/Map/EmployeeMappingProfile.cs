using AutoMapper;
using Organization.Domain.Entity;
using Organization.Features.EmployeeFeatures.DTO;

namespace Organization.Features.EmployeeFeatures.Map
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {
            CreateMap<Employee, EmployeeDto>().PreserveReferences();
            CreateMap<EmployeeForCreationDto, Employee>();
            CreateMap<Employee, EmployeeForCreationDto>();
        }
    }
}
