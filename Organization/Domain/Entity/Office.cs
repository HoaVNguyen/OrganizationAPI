using FluentValidation;

namespace Organization.Domain.Entity
{
    public class Office
    {
        public int OfficeId { get; set; }
        public string Name { get; set; }
        public ICollection<Employee>? Employees { get; set; } = new List<Employee>();

        public int ParishId { get; set; }
        public Parish Parish { get; set; }
        public Office(string name)
        {
            Name = name;
        }
    }

    public class OfficeValidator : AbstractValidator<Office>
    {
        public OfficeValidator()
        {
            RuleFor(o => o.Name).NotEmpty().MaximumLength(50);
        }
    }
}
