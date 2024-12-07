using Application.DTO.Request;
using FluentValidation;

namespace Application.Validators
{
    public class AddUserRequestValidator : AbstractValidator<AddUserRequest>
    {
        public AddUserRequestValidator()
        {
            RuleFor(u => u.UserLogin)
                .NotEmpty().WithMessage("Login is required");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(16).WithMessage("Password length must be at least 16 characters");

            RuleFor(u => u.Role)
                .Must(role => new[] { "Admin", "User" }.Contains(role))
                .WithMessage("Role must be Admin or User");
        }
    }
}
