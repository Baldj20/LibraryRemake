using Application.DTO.Response;
using Application.UseCases.BookUseCases;

namespace Application.Interfaces.Handlers.BookHandlers
{
    public interface IUpdateBookHandler
    {
        public Task<ActionSuccessStatusResponse> Handle(UpdateBookUseCase usecase, CancellationToken token);
    }
}
