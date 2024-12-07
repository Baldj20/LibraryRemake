using Application.DTO.Request;
using FluentValidation;

namespace Application.Validators
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(u => u.OldLogin)
                .NotEmpty().WithMessage("OldLogin is required");

            RuleFor(u => u.NewLogin)
                .NotEmpty().WithMessage("NewLogin is required");

            RuleFor(u => u.NewPassword)
                .MinimumLength(16).WithMessage("Password length must be at least 16 characters");
        }
    }
}
