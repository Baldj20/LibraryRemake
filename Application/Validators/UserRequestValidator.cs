using Application.DTO.Request;
using FluentValidation;

namespace Application.Validators
{
    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        public UserRequestValidator()
        {
            RuleFor(u => u.UserLogin)
                .NotEmpty().WithMessage("Login is required");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(16).WithMessage("Password length must be at least 16 characters");
        }
    }
}
