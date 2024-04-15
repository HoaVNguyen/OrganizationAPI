namespace Organization.Features.Addition.DTO
{
    public class OfficeDto
    {
        public int OfficeId { get; set; }
        public string Name { get; set; }
        public IEnumerable<EmployeeForCreationDto> Employees { get; set; }
        public int ParishId { get; set; }
    }

    public class OfficeForCreationDto
    {
        public string Name { get; set; }
        public int ParishId { get; set; }
    }
}
