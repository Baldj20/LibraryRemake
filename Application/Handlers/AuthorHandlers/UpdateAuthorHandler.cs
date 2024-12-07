using Application.DTO.Request;
using Application.DTO.Response;
using Application.Exceptions;
using Application.Interfaces.Handlers.AuthorHandlers;
using Application.Interfaces.Services;
using Application.UseCases.AuthorUseCases;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using FluentValidation;
using Newtonsoft.Json;

namespace Application.Handlers.AuthorHandlers
{
    public class UpdateAuthorHandler : IUpdateAuthorHandler
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IValidator<AuthorRequest> validator;
        private readonly IImageService imageService;
        public UpdateAuthorHandler(IUnitOfWork unitOfWork, IMapper mapper,
            IValidator<AuthorRequest> validator, IImageService imageService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.validator = validator;
            this.imageService = imageService;
        }
        public async Task<ActionSuccessStatusResponse> Handle(UpdateAuthorUseCase usecase, CancellationToken token)
        {
            var id = usecase.Id;
            var authorRequest = usecase.AuthorRequest;

            var validationResult = validator.Validate(authorRequest);
            if (!validationResult.IsValid) throw new BadRequestException(validationResult);

            var authorFromDB = await unitOfWork.Authors.GetByIdWithBooks(id, token);
            if (authorFromDB == null) throw new NotFoundException("Author to update not found");

            token.ThrowIfCancellationRequested();

            authorFromDB.Name = authorRequest.Name ?? authorFromDB.Name;
            authorFromDB.Surname = authorRequest.Surname ?? authorFromDB.Surname;
            authorFromDB.BirthDate = authorRequest.BirthDate == DateTime.MinValue? 
                authorFromDB.BirthDate: authorRequest.BirthDate;
            authorFromDB.Country = authorRequest.Country ?? authorFromDB.Country;

            foreach (var book in authorFromDB.Books)
            {
                await unitOfWork.Books.Delete(book, token);
            }

            var books = mapper
                .Map<List<Book>>(JsonConvert.DeserializeObject<List<BookRequest>>(authorRequest.BooksJson));

            foreach (var book in books)
            {
                await unitOfWork.Books.Add(book, token);
            }
            
            authorFromDB.Books = books;

            if (authorRequest.Image != null)
            {
                authorFromDB.ImagePath = await imageService.Upload(authorRequest.Image, id);
            }

            await unitOfWork.Authors.Update(authorFromDB, token);

            await unitOfWork.CompleteAsync(token);

            return new ActionSuccessStatusResponse
            {
                Success = true,
                Message = "Author updated successfully"
            };
        }
    }
}
