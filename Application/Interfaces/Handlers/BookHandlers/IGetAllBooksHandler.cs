using Application.DTO.Response;
using Application.UseCases.BookUseCases;

namespace Application.Interfaces.Handlers.BookHandlers
{
    public interface IGetAllBooksHandler
    {
        public Task<ICollection<BookResponse>> Handle(GetAllBooksUseCase usecase, CancellationToken token);
    }
}
