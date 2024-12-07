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
    public class AddAuthorHandler : IAddAuthorHandler
    {
        private readonly IValidator<AuthorRequest> validator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IImageService imageService;
        public AddAuthorHandler(IValidator<AuthorRequest> validator,
            IUnitOfWork unitOfWork, IMapper mapper,
            IImageService imageService)
        {
            this.validator = validator;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.imageService = imageService;
        }
        public async Task<ActionSuccessStatusResponse> Handle(AddAuthorUseCase usecase, CancellationToken token)
        {
            var authorRequest = usecase.AuthorRequest;
            var validationResult = validator.Validate(authorRequest);
            if (!validationResult.IsValid) throw new BadRequestException(validationResult);

            token.ThrowIfCancellationRequested();
            
            var books = JsonConvert.DeserializeObject<List<BookRequest>>(authorRequest.BooksJson);

            var author = mapper.Map<Author>(authorRequest);

            var image = authorRequest.Image;
            var imagePath = await imageService.Upload(image, author.Id);

            token.ThrowIfCancellationRequested();

            author.ImagePath = imagePath;
            author.Books = mapper.Map<List<Book>>(books);

            await unitOfWork.Authors.Add(author, token);

            await unitOfWork.CompleteAsync(token);

            return new ActionSuccessStatusResponse
            {
                Success = true,
                Message = "Author added successfully"
            };
        }
    }
}
