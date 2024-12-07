using Application.DTO.Response;
using Application.UseCases.AuthorUseCases;

namespace Application.Interfaces.Handlers.AuthorHandlers
{
    public interface IAddAuthorHandler
    {
        public Task<ActionSuccessStatusResponse> Handle(AddAuthorUseCase usecase, CancellationToken token);
    }
}
