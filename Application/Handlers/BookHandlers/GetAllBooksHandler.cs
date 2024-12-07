using Application.DTO.Response;
using Application.Exceptions;
using Application.Interfaces.Handlers.BookHandlers;
using Application.UseCases.BookUseCases;
using AutoMapper;
using Domain.Interfaces.Repositories;

namespace Application.Handlers.BookHandlers
{
    public class GetAllBooksHandler : IGetAllBooksHandler
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public GetAllBooksHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<ICollection<BookResponse>> Handle(GetAllBooksUseCase usecase, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var books = await unitOfWork.Books.GetAll(token);

            if (books.Count == 0) throw new NotFoundException("Books not found");

            return mapper.Map<ICollection<BookResponse>>(books);
        }
    }
}
