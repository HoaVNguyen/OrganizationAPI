using Organization.Features.EmployeeFeatures.DTO;

namespace Organization.Features.Addition.DTO
{
    public class TeamDto
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<EmployeeForCreationDto> employees { get; set; } = new List<EmployeeForCreationDto>();
    }
    public class TeamForCreationDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
