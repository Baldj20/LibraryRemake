using Application.DTO.Response;
using Application.UseCases.AuthorUseCases;

namespace Application.Interfaces.Handlers.AuthorHandlers
{
    public interface IGetAuthorByIdHandler
    {
        public Task<AuthorResponse> Handle(GetAuthorByIdUseCase usecase, CancellationToken token);
    }
}
