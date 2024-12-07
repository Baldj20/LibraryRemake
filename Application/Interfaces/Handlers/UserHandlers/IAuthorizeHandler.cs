using Application.DTO.Response;
using Application.UseCases.UserUseCases;

namespace Application.Interfaces.Handlers.UserHandlers
{
    public interface IAuthorizeHandler
    {
        public Task<TokenResponse> Handle(AuthorizeUseCase usecase, CancellationToken token);
    }
}
