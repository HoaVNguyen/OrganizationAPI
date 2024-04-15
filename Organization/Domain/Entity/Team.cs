using FluentValidation;

namespace Organization.Domain.Entity
{
    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public Team(string name)
        {
            Name = name;
        }
    }

    public class TeamValidator : AbstractValidator<Team>
    {
        public TeamValidator()
        {
            RuleFor(t => t.Name).NotEmpty().MaximumLength(50);
            RuleFor(t => t.Description).MaximumLength(200);
        }
    }
}
