using Application.DTO.Response;
using Application.UseCases.UserUseCases;

namespace Application.Interfaces.Handlers.UserHandlers
{
    public interface IRegisterBookForUserHandler
    {
        public Task<ActionSuccessStatusResponse> Handle(RegisterBookForUserUseCase usecase, CancellationToken token);
    }
}
