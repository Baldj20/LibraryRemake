using Application.DTO.Request;
using FluentValidation;

namespace Application.Validators
{
    public class BookRequestValidator : AbstractValidator<BookRequest>
    {
        public BookRequestValidator()
        {
            RuleFor(b => b.ISBN)
                .NotEmpty().WithMessage("ISBN cannot be empty")
                .Matches(@"^(?:\d{9}[\dX]|\d{13}|\d{3}-\d{1,5}-\d{1,7}-\d{1,7}-\d{1})$")
                .WithMessage("Invalid format");

            RuleFor(b => b.Title)
                .NotEmpty().WithMessage("Book must have a title")
                .MaximumLength(50).WithMessage("Book title is too long");

            RuleFor(b => b.Genre)
                .NotEmpty().WithMessage("Book must have a genre")
                .MaximumLength(20).WithMessage("Book genre is too long");

            RuleFor(b => b.Count)
                .NotEmpty()
                .GreaterThan(0).WithMessage("Invalid book count");
        }
    }
}
