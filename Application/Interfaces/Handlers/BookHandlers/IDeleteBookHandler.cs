using Application.DTO.Response;
using Application.UseCases.BookUseCases;

namespace Application.Interfaces.Handlers.BookHandlers
{
    public interface IDeleteBookHandler
    {
        public Task<ActionSuccessStatusResponse> Handle(DeleteBookUseCase usecase, CancellationToken token);
    }
}
