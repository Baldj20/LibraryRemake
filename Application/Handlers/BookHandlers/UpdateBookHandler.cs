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
    public class UpdateBookHandler : IUpdateBookHandler
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IValidator<BookRequest> validator;
        public UpdateBookHandler(IUnitOfWork unitOfWork, IMapper mapper,
            IValidator<BookRequest> validator)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.validator = validator;
        }
        public async Task<ActionSuccessStatusResponse> Handle(UpdateBookUseCase usecase, CancellationToken token)
        {
            var bookRequest = usecase.BookRequest;

            token.ThrowIfCancellationRequested();

            var validationResult = validator.Validate(bookRequest);
            if (!validationResult.IsValid) throw new BadRequestException(validationResult);

            var ISBN = bookRequest.ISBN;

            token.ThrowIfCancellationRequested();

            var bookFromDB = await unitOfWork.Books.GetByISBN(ISBN);
            if (bookFromDB == null) throw new NotFoundException("Book to update not found");

            token.ThrowIfCancellationRequested();

            bookFromDB.Title = bookRequest.Title;
            bookFromDB.Genre = bookRequest.Genre;
            bookFromDB.Description = bookRequest.Description;
            bookFromDB.Count = bookRequest.Count;
            bookFromDB.AuthorId = bookRequest.AuthorId;

            var newAuthor = await unitOfWork.Authors.GetById(bookRequest.AuthorId, token);
            if (newAuthor == null) throw new NotFoundException("Author with given id not found");

            bookFromDB.Author = newAuthor;

            await unitOfWork.Books.Update(bookFromDB, token);

            await unitOfWork.CompleteAsync(token);

            return new ActionSuccessStatusResponse
            {
                Success = true,
                Message = "Book updated successfully"
            };
        }
    }
}
