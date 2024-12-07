using Application.DTO.Response;
using Application.UseCases.BookUseCases;

namespace Application.Interfaces.Handlers.BookHandlers
{
    public interface IGetPagedBooksHandler
    {
        public Task<ICollection<BookResponse>> Handle(GetPagedBooksUseCase usecase, CancellationToken token);
    }
}
