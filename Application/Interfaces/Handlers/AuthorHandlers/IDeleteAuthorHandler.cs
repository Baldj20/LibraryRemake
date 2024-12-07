using Application.DTO.Response;
using Application.UseCases.AuthorUseCases;

namespace Application.Interfaces.Handlers.AuthorHandlers
{
    public interface IDeleteAuthorHandler
    {
        public Task<ActionSuccessStatusResponse> Handle(DeleteAuthorUseCase usecase, CancellationToken token);
    }
}
