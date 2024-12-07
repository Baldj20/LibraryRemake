using Application.DTO.Response;
using Application.UseCases.BookUseCases;

namespace Application.Interfaces.Handlers.BookHandlers
{
    public interface IGetBookByISBNHandler
    {
        public Task<BookResponse> Handle(GetBookByISBNUseCase usecase);
    }
}
