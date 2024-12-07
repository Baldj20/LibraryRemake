using Application.DTO.Response;
using Application.UseCases.BookUseCases;

namespace Application.Interfaces.Handlers.BookHandlers
{
    public interface IAddBookHandler
    {
        public Task<ActionSuccessStatusResponse> Handle(AddBookUseCase usecase, CancellationToken token);
    }
}
