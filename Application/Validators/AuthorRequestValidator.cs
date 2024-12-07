using Application.DTO.Request;
using FluentValidation;

namespace Application.Validators
{
    public class AuthorRequestValidator : AbstractValidator<AuthorRequest>
    {
        public AuthorRequestValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty().WithMessage("Name is required");

            RuleFor(a => a.Surname)
                .NotEmpty().WithMessage("Surname is required");

            RuleFor(a => a.BirthDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Birthdate cannot be in the future");

            RuleFor(a => a.Country)
                .Matches(@"^[A-Za-z]*$").WithMessage("Invalid format");
        }
    }
}
