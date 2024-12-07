using Application.DTO.Response;
using Application.Exceptions;
using Application.Interfaces.Handlers.BookHandlers;
using Application.UseCases.BookUseCases;
using AutoMapper;
using Domain.Interfaces.Repositories;

namespace Application.Handlers.BookHandlers
{
    public class GetBookByISBNHandler : IGetBookByISBNHandler
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public GetBookByISBNHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<BookResponse> Handle(GetBookByISBNUseCase usecase)
        {
            var ISBN = usecase.ISBN;

            var book = await unitOfWork.Books.GetByISBN(ISBN);

            if (book == null) throw new NotFoundException("Book with this ISBN not found");

            return mapper.Map<BookResponse>(book);
        }
    }
}
