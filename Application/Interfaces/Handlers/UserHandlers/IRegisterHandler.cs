using Application.DTO.Response;
using Application.UseCases.UserUseCases;

namespace Application.Interfaces.Handlers.UserHandlers
{
    public interface IRegisterHandler
    {
        public Task<TokenResponse> Handle(RegisterUseCase usecase, CancellationToken token);
    }
}
