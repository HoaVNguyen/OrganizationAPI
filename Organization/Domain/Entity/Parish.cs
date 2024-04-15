using FluentValidation;

namespace Organization.Domain.Entity
{
    public class Parish
    {
        public int ParishId { get; set; }
        public string Name { get; set; }
        public int ZipCode { get; set; }
        public Office? Office { get; set; } = null;

        public Parish(string name)
        {
            Name = name;
        }
    }

    public class ParishValidator : AbstractValidator<Parish>
    {
        public ParishValidator()
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(50);
            RuleFor(p => p.ZipCode).NotEmpty().InclusiveBetween(10000, 99999).WithMessage("Enter a valid zip code");
        }
    }
}
