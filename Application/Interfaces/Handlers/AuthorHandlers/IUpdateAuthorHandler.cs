using Application.DTO.Response;
using Application.UseCases.AuthorUseCases;

namespace Application.Interfaces.Handlers.AuthorHandlers
{
    public interface IUpdateAuthorHandler
    {
        public Task<ActionSuccessStatusResponse> Handle(UpdateAuthorUseCase usecase, CancellationToken token);
    }
}
