using Application.DTO.Request;
using Application.DTO.Response;
using Application.Exceptions;
using Application.Interfaces.Handlers.BookHandlers;
using Application.UseCases.BookUseCases;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using FluentValidation;

namespace Application.Handlers.BookHandlers
{
    public class AddBookHandler : IAddBookHandler
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IValidator<BookRequest> validator;
        public AddBookHandler(IUnitOfWork unitOfWork, IMapper mapper,
            IValidator<BookRequest> validator)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.validator = validator;
        }
        public async Task<ActionSuccessStatusResponse> Handle(AddBookUseCase usecase, CancellationToken token)
        {
            var bookRequest = usecase.BookRequest;

            var validationResult = validator.Validate(bookRequest);
            if (!validationResult.IsValid) throw new BadRequestException(validationResult);

            token.ThrowIfCancellationRequested();

            var ISBN = bookRequest.ISBN;
            var authorId = bookRequest.AuthorId;

            var bookFromDB = await unitOfWork.Books.GetByISBN(ISBN);
            if (bookFromDB != null) throw new AlreadyExistException("Cannot add book because book with this ISBN already exists");

            var bookAuthor = await unitOfWork.Authors.GetById(authorId, token);
            if (bookAuthor == null) throw new NotFoundException("Cannot add book because book author not found");

            token.ThrowIfCancellationRequested();

            var book = mapper.Map<Book>(bookRequest);

            token.ThrowIfCancellationRequested();

            bookAuthor.Books.Add(book);
            await unitOfWork.Books.Add(book, token);

            await unitOfWork.CompleteAsync(token);

            return new ActionSuccessStatusResponse
            {
                Success = true,
                Message = "Book added successfully"
            };
        }
    }
}
