using FluentValidation;

namespace Organization.Domain.Entity
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public int? OfficeId { get; set; } = null;
        public Office? Office { get; set; } = null;

        public ICollection<Team> Teams { get; set; } = new List<Team>();

        public Employee(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }

    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            RuleFor(e => e.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(e => e.LastName).NotEmpty().MaximumLength(100);
            RuleFor(e => e.Age).GreaterThanOrEqualTo(18).WithMessage("Employee age must be at least 18 years old");

        }

    }
}
