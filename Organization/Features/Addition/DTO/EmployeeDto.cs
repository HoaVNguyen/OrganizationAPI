namespace Organization.Features.Addition.DTO
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<TeamForCreationDto> Teams { get; set; } = new List<TeamForCreationDto>();
    }
    public class EmployeeForCreationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

    }
}
