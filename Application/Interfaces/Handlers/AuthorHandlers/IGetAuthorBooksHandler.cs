using Application.DTO.Response;
using Application.UseCases.AuthorUseCases;

namespace Application.Interfaces.Handlers.AuthorHandlers
{
    public interface IGetAuthorBooksHandler
    {
        public Task<ICollection<BookResponse>> Handle(GetAuthorBooksUseCase usecase, CancellationToken token);
    }
}
